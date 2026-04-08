using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Managers;
using _3DMANAGER_APP.BLL.Mapper;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.File;
using _3DMANAGER_APP.BLL.Models.User;
using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Managers;
using _3DMANAGER_APP.TEST.E2ETest;
using _3DMANAGER_APP.TEST.Fixture;
using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using MySqlX.XDevAPI;

namespace _3DMANAGER_APP.TEST.IntegrationTest
{
    [Collection("Database")]
    public class UserIntegrationTests
    {
        private readonly DatabaseFixture _fixture;
        private readonly IMapper _mapper;
        private readonly IAzureBlobStorageService _fakeService;
        private readonly INotificationManager _notificationManager;
        public UserIntegrationTests(DatabaseFixture fixture)
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
        public void User_ShouldReturnUser()
        {
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER"
            );

            var userDbManager = new UserDbManager(
                dataSource,
                NullLogger<UserDbManager>.Instance
            );

            var manager = new UserManager(
                userDbManager,
                _mapper,
                NullLogger<UserManager>.Instance,
                _fakeService,
                _notificationManager
            );

            BaseError? error;
            var users = manager.GetUserList(1, out error);

            Assert.Null(error);
            Assert.NotNull(users);
            Assert.NotEmpty(users);
        }

        [Fact]
        public void User_ShouldReturnDetail_AndUpdate()
        {
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER");

            var userDbManager = new UserDbManager(
                dataSource,
                NullLogger<UserDbManager>.Instance);

            var manager = new UserManager(userDbManager,
                _mapper,
                NullLogger<UserManager>.Instance,
                _fakeService, _notificationManager);

            BaseError? error;

            var user = manager.GetUserDetail(1, 1, out error);
            Assert.Null(error);
            Assert.NotNull(user);
            var request = new UserUpdateRequest
            {
                GroupId = 1,
                UserId = user.userId,
                UserName = "Integration User Updated",
                UserEmail = "integration@test.com"
            };
            var result = manager.UpdateUser(request);
            Assert.True(result);

            var updated = manager.GetUserDetail(1, user.userId, out error);
            Assert.Null(error);
            Assert.NotNull(updated);
            Assert.Equal("Integration User Updated", updated.userName);
            Assert.Equal("integration@test.com", updated.userEmail);
        }

    }
}
