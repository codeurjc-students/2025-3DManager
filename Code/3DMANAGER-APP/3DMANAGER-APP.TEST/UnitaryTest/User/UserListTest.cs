using _3DMANAGER_APP.BLL.Managers;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.User;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.User;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace _3DMANAGER_APP.TEST.UnitaryTest.User
{

    public class UserListTest
    {
        private readonly Mock<ILogger<UserManager>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserDbManager> _userDbManagerMock;
        private readonly UserManager _manager;

        public UserListTest()
        {
            _loggerMock = new Mock<ILogger<UserManager>>();
            _mapperMock = new Mock<IMapper>();
            _userDbManagerMock = new Mock<IUserDbManager>();

            _manager = new UserManager(
                 _userDbManagerMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
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

            _userDbManagerMock
                .Setup(db => db.GetUserList(groupId))
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

            var manager = new UserManager(
                _userDbManagerMock.Object,
                realMapper,
                _loggerMock.Object
            );


            var result = manager.GetUserList(groupId, out BaseError? error);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Null(error);

            Assert.Equal("1h 2min", result[0].UserHours);
            Assert.Equal("0h 13min", result[1].UserHours);

            _userDbManagerMock.Verify(db => db.GetUserList(groupId), Times.Once);
        }
    }

}
