using _3DMANAGER_APP.TEST.E2ETest;
using _3DMANAGER_APP.TEST.Fixture;
using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace _3DMANAGER_APP.TEST.IntegrationTest
{
    [Collection("Database")]
    public class PrintIntegrationTests
    {
        private readonly DatabaseFixture _fixture;
        private readonly IMapper _mapper;
        private readonly IAzureBlobStorageService _fakeService;
        private readonly INotificationService _notificationService;
        public PrintIntegrationTests(DatabaseFixture fixture)
        {
            _fixture = fixture;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }, NullLoggerFactory.Instance);

            _mapper = config.CreateMapper();
            _fakeService = new FakeAzureBlobStorageService();
            var absMock = new Mock<IAzureBlobStorageService>();

            absMock.Setup(x => x.UploadImageAsync(
                    It.IsAny<Stream>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<int>()
            ))
            .ReturnsAsync(new FileResponse
            {
                FileKey = "printers/test.jpg",
                FileUrl = "https://fake-url.com/printers/test.jpg"
            });

            absMock.Setup(x => x.DeleteImageAsync(It.IsAny<string>()))
                  .Returns(Task.CompletedTask);

            absMock.Setup(x => x.GetPresignedUrl(It.IsAny<string>(), It.IsAny<int>()))
                  .Returns("https://fake-url.com/presigned/test.jpg");

            _fakeService = absMock.Object;
        }

        [Fact]
        public async Task Print_ShouldReturnPrintsAndCreate()
        {
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER"
            );

            var printRepository = new PrintRepository(
                dataSource,
                NullLogger<PrintRepository>.Instance
            );

            var service = new PrintService(
                printRepository,
                _mapper,
                NullLogger<PrintService>.Instance,
                _fakeService,
                _notificationService
            );

            BaseError? error;
            PagedRequest pagedRequest = new PagedRequest { PageNumber = 1, PageSize = 10 };
            var prints = service.GetPrintList(1, pagedRequest, out error);

            Assert.Null(error);
            Assert.NotNull(prints.prints);
            Assert.NotEmpty(prints.prints);

            PrintRequest newPrint = new PrintRequest()
            {
                GroupId = 1,
                PrintName = "Test",
                PrintDescription = "Test",
                PrintFilament = 1,
                PrintFilamentUsed = 20,
                PrintPrinter = 1,
                PrintRealTime = 10,
                PrintTime = 10,
                PrintState = 1,
                UserId = 1
            };
            var printPost = await service.PostPrint(newPrint);
            Assert.Null(error);
            Assert.NotNull(printPost);
            Assert.NotEqual(0, printPost.Data);

            var printsAfterPost = service.GetPrintList(1, pagedRequest, out error);
            Assert.Null(error);
            Assert.Equal(prints.prints.Count + 1, printsAfterPost.prints.Count);
            Assert.NotNull(printsAfterPost);
            Assert.NotEmpty(printsAfterPost.prints);

        }

        [Fact]
        public void Print_ShouldReturnDetail_AndUpdate()
        {
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER");

            var printRepository = new PrintRepository(
                dataSource,
                NullLogger<PrintRepository>.Instance);

            var service = new PrintService(
                printRepository,
                _mapper,
                NullLogger<PrintService>.Instance,
                _fakeService, _notificationService);

            BaseError? error;

            var print = service.GetPrintDetail(1, 1, out error);
            Assert.Null(error);
            Assert.NotNull(print);

            var request = new PrintDetailRequest
            {
                GroupId = 1,
                PrintId = 1,
                PrintName = "Updated print",
                PrintDescription = "Updated description"
            };


            var result = service.UpdatePrint(request);
            Assert.True(result);

            var updated = service.GetPrintDetail(1, print.PrintId, out error);
            Assert.Null(error);
            Assert.NotNull(updated);
            Assert.Equal("Updated print", updated.PrintName);
            Assert.Equal("Updated description", updated.PrintDescription);
        }
        [Fact]
        public async Task DeletePrint_ShouldDeleteSuccessfully()
        {
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER"
            );

            var printRepository = new PrintRepository(
                dataSource,
                NullLogger<PrintRepository>.Instance
            );

            var service = new PrintService(
                printRepository,
                _mapper,
                NullLogger<PrintService>.Instance,
                _fakeService,
                _notificationService
            );

            var newPrint = new PrintRequest
            {
                GroupId = 1,
                PrintName = "Print to delete",
                PrintDescription = "Test delete",
                PrintFilament = 1,
                PrintFilamentUsed = 10,
                PrintPrinter = 1,
                PrintRealTime = 10,
                PrintTime = 10,
                PrintState = 1,
                UserId = 1
            };

            var created = await service.PostPrint(newPrint);
            Assert.NotNull(created);
            Assert.True(created.Data > 0);

            int printId = created.Data;

            var deleteResponse = await service.DeletePrint(printId, 1);

            Assert.NotNull(deleteResponse);
            Assert.True(deleteResponse.Data);
            Assert.Null(deleteResponse.Error);

            var prints = service.GetPrintList(1, new PagedRequest { PageNumber = 1, PageSize = 50 }, out BaseError? error);
            Assert.Null(error);
            Assert.DoesNotContain(prints.prints, p => p.PrintId == printId);
        }
    }
}
