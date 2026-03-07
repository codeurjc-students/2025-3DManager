using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Managers;
using _3DMANAGER_APP.BLL.Mapper;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.File;
using _3DMANAGER_APP.BLL.Models.Print;
using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Managers;
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
        private readonly IAwsS3Service _fakeS3Service;

        public PrintIntegrationTests(DatabaseFixture fixture)
        {
            _fixture = fixture;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }, NullLoggerFactory.Instance);

            _mapper = config.CreateMapper();
            _fakeS3Service = new FakeAwsS3Service();
            var s3Mock = new Mock<IAwsS3Service>();

            s3Mock.Setup(x => x.UploadImageAsync(
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

            s3Mock.Setup(x => x.DeleteImageAsync(It.IsAny<string>()))
                  .Returns(Task.CompletedTask);

            s3Mock.Setup(x => x.GetPresignedUrl(It.IsAny<string>(), It.IsAny<int>()))
                  .Returns("https://fake-url.com/presigned/test.jpg");

            _fakeS3Service = s3Mock.Object;
        }

        [Fact]
        public void Print_ShouldReturnPrintsAndCreate()
        {
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER"
            );

            var printDbManager = new PrintDbManager(
                dataSource,
                NullLogger<PrintDbManager>.Instance
            );

            var manager = new PrintManager(
                printDbManager,
                _mapper,
                NullLogger<PrintManager>.Instance,
                _fakeS3Service
            );

            BaseError error;
            PagedRequest pagedRequest = new PagedRequest { PageNumber = 1, PageSize = 10 };
            var prints = manager.GetPrintList(1, pagedRequest, out error);

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
            var printPost = manager.PostPrint(newPrint);
            Assert.Null(error);
            Assert.NotEqual(0, printPost.Result.Data);

            var printsAfterPost = manager.GetPrintList(1, pagedRequest, out error);
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

            var printDbManager = new PrintDbManager(
                dataSource,
                NullLogger<PrintDbManager>.Instance);

            var manager = new PrintManager(
                printDbManager,
                _mapper,
                NullLogger<PrintManager>.Instance,
                _fakeS3Service);

            BaseError error;

            var print = manager.GetPrintDetail(1, 1, out error);
            Assert.Null(error);
            Assert.NotNull(print);

            var request = new PrintDetailRequest
            {
                GroupId = 1,
                PrintId = 1,
                PrintName = "Updated print",
                PrintDescription = "Updated description"
            };


            var result = manager.UpdatePrint(request);
            Assert.True(result);

            var updated = manager.GetPrintDetail(1, print.PrintId, out error);
            Assert.Null(error);
            Assert.NotNull(updated);
            Assert.Equal("Updated print", updated.PrintName);
            Assert.Equal("Updated description", updated.PrintDescription);
        }
    }
}
