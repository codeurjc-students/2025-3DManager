using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Mapper;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.File;
using _3DMANAGER_APP.BLL.Models.Group;
using _3DMANAGER_APP.BLL.Services;
using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Repositories;
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

        [Fact]
        public void GetGroupBasicData_ShouldReturnValidData()
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

            var result = service.GetGroupBasicData(1, out BaseError? error);

            Assert.Null(error);
            Assert.NotNull(result);
            Assert.Equal(1, result.GroupId);
        }

        [Fact]
        public void GetGroupBasicData_ShouldReturnError_WhenGroupDoesNotExist()
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

            var result = service.GetGroupBasicData(-1, out BaseError? error);

            Assert.NotNull(error);
            Assert.Equal(500, error.code);
        }

        [Fact]
        public void PostNewGroup_ShouldCreateSuccessfully()
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

            var request = new GroupRequest
            {
                GroupName = "Nuevo grupo",
                UserId = 1
            };

            var result = service.PostNewGroup(request, out BaseError? error);

            Assert.True(result);
            Assert.Null(error);
        }

        [Fact]
        public void PostNewGroup_ShouldReturnConflict_WhenNameExists()
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

            var groupName = $"Nuevo grupo";

            var request = new GroupRequest
            {
                GroupName = groupName,
                UserId = 1
            };

            service.PostNewGroup(request, out _);
            var result = service.PostNewGroup(request, out BaseError? error);

            Assert.False(result);
            Assert.NotNull(error);
            Assert.Equal(409, error.code);
        }

        [Fact]
        public void UpdateGroupData_ShouldUpdateSuccessfully()
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

            var request = new GroupRequest
            {
                GroupName = "Grupo Actualizado",
                UserId = 1
            };

            var result = service.UpdateGroupData(request, 1);

            Assert.True(result);
        }

    }
}
