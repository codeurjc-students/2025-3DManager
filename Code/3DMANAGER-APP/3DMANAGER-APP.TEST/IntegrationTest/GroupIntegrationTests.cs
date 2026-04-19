using _3DMANAGER_APP.TEST.E2ETest;
using _3DMANAGER_APP.TEST.Fixture;
using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace _3DMANAGER_APP.TEST.IntegrationTest
{
    [Collection("Database")]
    public class GroupIntegrationTests
    {
        private readonly DatabaseFixture _fixture;
        private readonly IMapper _mapper;
        private readonly IAzureBlobStorageService _fakeABSService;
        private readonly INotificationService _notificationService;

        public GroupIntegrationTests(DatabaseFixture fixture)
        {
            _fixture = fixture;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }, NullLoggerFactory.Instance);

            _mapper = config.CreateMapper();
            _fakeABSService = new FakeAzureBlobStorageService();
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
                FileKey = "group/test.jpg",
                FileUrl = "https://fake-url.com/printers/test.jpg"
            });

            absMock.Setup(x => x.DeleteImageAsync(It.IsAny<string>()))
                  .Returns(Task.CompletedTask);

            absMock.Setup(x => x.GetPresignedUrl(It.IsAny<string>(), It.IsAny<int>()))
                  .Returns("https://fake-url.com/presigned/test.jpg");

            _fakeABSService = absMock.Object;
        }

        [Fact]
        public void GetGroupDashboardData_ShouldReturnSuccess()
        {
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER"
            );

            var groupRepository = new GroupRepository(
                dataSource,
                NullLogger<GroupRepository>.Instance
            );

            var service = new GroupService(
                groupRepository,
                _mapper,
                NullLogger<GroupService>.Instance,
                _fakeABSService,
                _notificationService
            );

            var result = service.GetGroupDashboardData(1, out BaseError? error);

            Assert.Null(error);
            Assert.NotNull(result);
            Assert.IsType<GroupDashboardData>(result);

            Assert.True(result.GroupTotalPrints >= 0);
            Assert.True(result.GroupTotalFilament >= 0);
        }


    }
}
