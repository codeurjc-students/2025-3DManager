using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Mapper;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.File;
using _3DMANAGER_APP.BLL.Models.User;
using _3DMANAGER_APP.BLL.Services;
using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Repositories;
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
        public async Task User_ShouldCreateSuccessfully()
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

            var request = new UserCreateRequest
            {
                UserName = $"usertest",
                UserEmail = $"user_test@test.com",
                UserPassword = "12345"
            };

            var result = await service.PostNewUser(request);

            Assert.NotNull(result);
            Assert.True(result.Data > 0);
            Assert.Null(result.Error);
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

        [Fact]
        public void GetUserDetail_ShouldUseDefaultImage_WhenNoImage()
        {
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER");

            var repo = new UserRepository(
                dataSource,
                NullLogger<UserRepository>.Instance);

            var service = new UserService(
                repo,
                _mapper,
                NullLogger<UserService>.Instance,
                _fakeService,
                _notificationService);

            BaseError? error;

            var user = service.GetUserDetail(1, out error);

            Assert.Null(error);
            Assert.NotNull(user);

            Assert.Contains("https://fake-url.com/presigned/test.jpg", user.UserImageData.FileUrl);
        }



        [Fact]
        public async Task DeleteUserImage_ShouldReturnTrue_WhenNoImage()
        {
            var dataSource = new MySQLDataSource(_fixture.ConnectionString, "3DMANAGER");

            var repo = new UserRepository(dataSource, NullLogger<UserRepository>.Instance);

            var service = new UserService(
                repo,
                _mapper,
                NullLogger<UserService>.Instance,
                _fakeService,
                _notificationService
            );

            var result = await service.DeleteUserImage(1);

            Assert.NotNull(result);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnError_WhenUsernameAlreadyExists()
        {
            var dataSource = new MySQLDataSource(_fixture.ConnectionString, "3DMANAGER");

            var repo = new UserRepository(dataSource, NullLogger<UserRepository>.Instance);

            var service = new UserService(
                repo,
                _mapper,
                NullLogger<UserService>.Instance,
                _fakeService,
                _notificationService
            );
            var request1 = new UserCreateRequest
            {
                UserName = "usertestNew",
                UserEmail = "user_testNew@test.com",
                UserPassword = "12345"
            };

            var result1 = await service.PostNewUser(request1);

            Assert.NotNull(result1);
            Assert.True(result1.Data > 0);
            Assert.Null(result1.Error);


            var request = new UserUpdateRequest
            {
                GroupId = 1,
                UserId = 1,
                UserName = "usertestNew",
                UserEmail = "newemail@test.com"
            };

            var result = service.UpdateUser(request, out BaseError? error);

            Assert.False(result);
            Assert.NotNull(error);
            Assert.Equal(409, error.code);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnError_WhenEmailAlreadyExists()
        {
            var dataSource = new MySQLDataSource(_fixture.ConnectionString, "3DMANAGER");

            var repo = new UserRepository(dataSource, NullLogger<UserRepository>.Instance);

            var service = new UserService(
                repo,
                _mapper,
                NullLogger<UserService>.Instance,
                _fakeService,
                _notificationService
            );

            var request1 = new UserCreateRequest
            {
                UserName = "usertest2",
                UserEmail = "user_test2@test.com",
                UserPassword = "12345"
            };

            var result1 = await service.PostNewUser(request1);

            Assert.NotNull(result1);
            Assert.True(result1.Data > 0);
            Assert.Null(result1.Error);

            var request = new UserUpdateRequest
            {
                GroupId = 1,
                UserId = 1,
                UserName = "uniqueName2",
                UserEmail = "user_test2@test.com"
            };

            var result = service.UpdateUser(request, out BaseError? error);

            Assert.False(result);
            Assert.NotNull(error);
            Assert.Equal(409, error.code);
        }

        [Fact]
        public void GetUserDetail_ShouldReturnError_WhenUserNotExists()
        {
            var dataSource = new MySQLDataSource(_fixture.ConnectionString, "3DMANAGER");

            var repo = new UserRepository(dataSource, NullLogger<UserRepository>.Instance);

            var service = new UserService(
                repo,
                _mapper,
                NullLogger<UserService>.Instance,
                _fakeService,
                _notificationService
            );

            var result = service.GetUserDetail(999999, out BaseError? error);

            Assert.NotNull(error);
            Assert.Equal(500, error.code);
        }

        [Fact]
        public async Task UpdateUserImage_ShouldReturnError_WhenImageIsNull()
        {
            var dataSource = new MySQLDataSource(_fixture.ConnectionString, "3DMANAGER");

            var repo = new UserRepository(dataSource, NullLogger<UserRepository>.Instance);

            var service = new UserService(
                repo,
                _mapper,
                NullLogger<UserService>.Instance,
                _fakeService,
                _notificationService
            );

            var result = await service.UpdateUserImage(1, null);

            Assert.NotNull(result);
            Assert.NotNull(result.Error);
            Assert.Equal(400, result.Error.Code);
        }


    }
}
