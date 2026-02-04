using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Managers;
using _3DMANAGER_APP.BLL.Models.User;
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
        private readonly Mock<ILogger<UserManager>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserDbManager> _userDbManagerMock;
        private readonly UserManager _manager;
        private readonly Mock<IAwsS3Service> _awsS3Service;


        public PostNewUserTests()
        {
            _loggerMock = new Mock<ILogger<UserManager>>();
            _mapperMock = new Mock<IMapper>();
            _userDbManagerMock = new Mock<IUserDbManager>();
            _awsS3Service = new Mock<IAwsS3Service>();

            _manager = new UserManager(
                 _userDbManagerMock.Object,
                _mapperMock.Object,
                _loggerMock.Object,
                 _awsS3Service.Object
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
            _userDbManagerMock
                .Setup(db => db.PostNewUser(It.IsAny<UserCreateRequestDbObject>(), out errorDb))
                .Returns(1);

            var result = _manager.PostNewUser(user);

            Assert.NotEqual(0, result.Result.Data);
            Assert.Null(result.Result.Error);
            _mapperMock.Verify(m => m.Map<UserCreateRequestDbObject>(It.IsAny<UserCreateRequest>()), Times.Once);
            _userDbManagerMock.Verify(db => db.PostNewUser(It.IsAny<UserCreateRequestDbObject>(), out errorDb), Times.Once);
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
            _userDbManagerMock
                .Setup(db => db.PostNewUser(It.IsAny<UserCreateRequestDbObject>(), out errorDb))
                .Returns(0);

            var result = _manager.PostNewUser(user);

            Assert.NotNull(result.Result.Error);
            Assert.Equal(0, result.Result.Data);
            Assert.Equal(StatusCodes.Status409Conflict, result.Result.Error.Code);
            Assert.Contains("Ya existe una cuenta con ese nombre", result.Result.Error.Message);
        }
    }
}
