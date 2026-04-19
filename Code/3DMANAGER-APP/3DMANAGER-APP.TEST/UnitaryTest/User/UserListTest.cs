using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace _3DMANAGER_APP.TEST.UnitaryTest.User
{

    public class UserListTest
    {
        private readonly Mock<ILogger<UserService>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserService _userService;
        private readonly Mock<IAzureBlobStorageService> _aBSService;
        private readonly INotificationService _notificationService;
        public UserListTest()
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
        public void GetUserList_WhenDbReturnsData_ShouldConvertUserHoursFromSecondsToFormattedString()
        {

            int groupId = 1;

            var dbResponse = new List<UserListResponseDbObject>
            {
                new UserListResponseDbObject
                {
                    UserId = 1,
                    UserName = "user1",
                    UserHours = 3720,
                    UserNumberPrints = 4
                },
                new UserListResponseDbObject
                {
                    UserId = 2,
                    UserName = "user2",
                    UserHours = 780,
                    UserNumberPrints = 4
                }
            };

            bool outError;

            _userRepositoryMock
                .Setup(db => db.GetUserList(groupId, out outError))
                .Returns(dbResponse);

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserListResponseDbObject, UserListResponse>()
                   .ForMember(dest => dest.UserHours,
                       opt => opt.MapFrom(src =>
                           $"{(int)TimeSpan.FromSeconds((double)src.UserHours).TotalHours}h " +
                           $"{TimeSpan.FromSeconds((double)src.UserHours).Minutes}min"));
            }, NullLoggerFactory.Instance);

            var realMapper = mapperConfig.CreateMapper();

            var service = new UserService(
                _userRepositoryMock.Object,
                realMapper,
                _loggerMock.Object,
                _aBSService.Object,
                _notificationService
            );


            var result = service.GetUserList(groupId, out BaseError? error);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Null(error);

            Assert.Equal("1h 2min", result[0].UserHours);
            Assert.Equal("0h 13min", result[1].UserHours);

            _userRepositoryMock.Verify(db => db.GetUserList(groupId, out outError), Times.Once);
        }
    }

}
