using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.User;
using _3DMANAGER_APP.BLL.Services;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.User;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace _3DMANAGER_APP.TEST.UnitaryTest.User
{
    public class PostNewUserTests
    {
        private readonly Mock<ILogger<UserService>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserService _userService;
        private readonly Mock<IAzureBlobStorageService> _aBSService;
        private readonly INotificationService _notificationService;

        public PostNewUserTests()
        {
            _loggerMock = new Mock<ILogger<UserService>>();
            _mapperMock = new Mock<IMapper>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _aBSService = new Mock<IAzureBlobStorageService>();

            _userService = new UserService(
                 _userRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object,
                 _aBSService.Object,
                 _notificationService
            );
        }

        [Fact]
        [Trait("Category", "Unitary")]
        public void PostNewUser_WhenDbReturnsSuccess_ShouldReturnTrueAndNoError()
        {
            var user = new UserCreateRequest
            {
                UserName = "testuser",
                UserEmail = "test@example.com",
                UserPassword = "password123"
            };

            var userDbObject = new UserCreateRequestDbObject { UserName = user.UserName, UserEmail = user.UserEmail };

            _mapperMock
                .Setup(m => m.Map<UserCreateRequestDbObject>(It.IsAny<UserCreateRequest>()))
                .Returns(userDbObject);

            int? errorDb = null;
            _userRepositoryMock
                .Setup(db => db.PostNewUser(It.IsAny<UserCreateRequestDbObject>(), out errorDb))
                .Returns(1);

            var result = _userService.PostNewUser(user);

            Assert.NotEqual(0, result.Result.Data);
            Assert.Null(result.Result.Error);
            _mapperMock.Verify(m => m.Map<UserCreateRequestDbObject>(It.IsAny<UserCreateRequest>()), Times.Once);
            _userRepositoryMock.Verify(db => db.PostNewUser(It.IsAny<UserCreateRequestDbObject>(), out errorDb), Times.Once);
        }

        [Fact]
        [Trait("Category", "Unitary")]
        public void PostNewUser_WhenDbReturnsConflictError_ShouldSetBaseErrorAndReturnFalse()
        {
            var user = new UserCreateRequest
            {
                UserName = "existinguser",
                UserEmail = "existing@example.com",
                UserPassword = "password123"
            };

            var userDbObject = new UserCreateRequestDbObject { UserName = user.UserName, UserEmail = user.UserEmail };

            _mapperMock
                .Setup(m => m.Map<UserCreateRequestDbObject>(It.IsAny<UserCreateRequest>()))
                .Returns(userDbObject);

            int? errorDb = 4091;
            _userRepositoryMock
                .Setup(db => db.PostNewUser(It.IsAny<UserCreateRequestDbObject>(), out errorDb))
                .Returns(0);

            var result = _userService.PostNewUser(user);

            Assert.NotNull(result.Result.Error);
            Assert.Equal(0, result.Result.Data);
            Assert.Equal(StatusCodes.Status409Conflict, result.Result.Error.Code);
            Assert.Contains("Ya existe una cuenta con ese nombre", result.Result.Error.Message);
        }
    }
}
