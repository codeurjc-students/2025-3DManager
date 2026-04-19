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
        private readonly INotificationService _notificationService;
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

            var userRepository = new UserRepository(
                dataSource,
                NullLogger<UserRepository>.Instance
            );

            var service = new UserService(
                userRepository,
                _mapper,
                NullLogger<UserService>.Instance,
                _fakeService,
                _notificationService
            );

            BaseError? error;
            var users = service.GetUserList(1, out error);

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

            var userRepository = new UserRepository(
                dataSource,
                NullLogger<UserRepository>.Instance);

            var service = new UserService(userRepository,
                _mapper,
                NullLogger<UserService>.Instance,
                _fakeService, _notificationService);

            BaseError? error;

            var user = service.GetUserDetail(1, out error);
            Assert.Null(error);
            Assert.NotNull(user);
            var request = new UserUpdateRequest
            {
                GroupId = 1,
                UserId = user.userId,
                UserName = "Integration User Updated",
                UserEmail = "integration@test.com"
            };
            var result = service.UpdateUser(request, out BaseError? errorR);
            Assert.True(result);
            Assert.Null(errorR);
            var updated = service.GetUserDetail(user.userId, out error);
            Assert.Null(error);
            Assert.NotNull(updated);
            Assert.Equal("Integration User Updated", updated.userName);
            Assert.Equal("integration@test.com", updated.userEmail);
        }

    }
}
