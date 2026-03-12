using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Managers;
using _3DMANAGER_APP.BLL.Mapper;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.File;
using _3DMANAGER_APP.BLL.Models.Group;
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
    public class GroupIntegrationTests
    {
        private readonly DatabaseFixture _fixture;
        private readonly IMapper _mapper;
        private readonly IAwsS3Service _fakeS3Service;

        public GroupIntegrationTests(DatabaseFixture fixture)
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
                FileKey = "group/test.jpg",
                FileUrl = "https://fake-url.com/printers/test.jpg"
            });

            s3Mock.Setup(x => x.DeleteImageAsync(It.IsAny<string>()))
                  .Returns(Task.CompletedTask);

            s3Mock.Setup(x => x.GetPresignedUrl(It.IsAny<string>(), It.IsAny<int>()))
                  .Returns("https://fake-url.com/presigned/test.jpg");

            _fakeS3Service = s3Mock.Object;
        }

        [Fact]
        public void GetGroupDashboardData_ShouldReturnSuccess()
        {
            // Arrange
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER"
            );

            var groupDbManager = new GroupDbManager(
                dataSource,
                NullLogger<GroupDbManager>.Instance
            );

            var manager = new GroupManager(
                groupDbManager,
                _mapper,
                NullLogger<GroupManager>.Instance,
                _fakeS3Service
            );

            var result = manager.GetGroupDashboardData(1, out BaseError? error);

            Assert.Null(error);
            Assert.NotNull(result);
            Assert.IsType<GroupDashboardData>(result);

            Assert.True(result.GroupTotalPrints >= 0);
            Assert.True(result.GroupTotalFilament >= 0);
        }


    }
}
