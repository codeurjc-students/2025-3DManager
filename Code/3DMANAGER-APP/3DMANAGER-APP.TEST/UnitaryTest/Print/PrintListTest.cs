using _3DMANAGER_APP.BLL.Managers;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Print;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.Print;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace _3DMANAGER_APP.TEST.UnitaryTest.Print
{
    public class PrintListTest
    {
        private readonly Mock<ILogger<PrintManager>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IPrintDbManager> _printDbManagerMock;
        private readonly PrintManager _manager;

        public PrintListTest()
        {
            _loggerMock = new Mock<ILogger<PrintManager>>();
            _mapperMock = new Mock<IMapper>();
            _printDbManagerMock = new Mock<IPrintDbManager>();

            _manager = new PrintManager(
                 _printDbManagerMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        [Trait("Category", "Unitary")]
        public void GetPrintList_WhenDbReturnsData_ShouldConvertPrintTimeFromSecondsToFormattedString()
        {

            int groupId = 1;

            var dbResponse = new List<PrintListResponseDbObject>
    {
        new PrintListResponseDbObject
        {
            PrintId = 1,
            PrintName = "Test Print 1",
            PrintUserCreator = "user1",
            PrintDate = new DateTime(2024, 1, 1),
            PrintTime = 3720, // 1h 2min
            PrintFilamentConsumed = 12.5m
        },
        new PrintListResponseDbObject
        {
            PrintId = 2,
            PrintName = "Test Print 2",
            PrintUserCreator = "user2",
            PrintDate = new DateTime(2024, 1, 2),
            PrintTime = 780, // 0h 13min
            PrintFilamentConsumed = 5.3m
        }
    };

            _printDbManagerMock
                .Setup(db => db.GetPrintList(groupId))
                .Returns(dbResponse);


            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PrintListResponseDbObject, PrintListResponse>()
                   .ForMember(dest => dest.PrintTime,
                       opt => opt.MapFrom(src =>
                           $"{(int)TimeSpan.FromSeconds((double)src.PrintTime).TotalHours}h " +
                           $"{TimeSpan.FromSeconds((double)src.PrintTime).Minutes}min"));
            }, NullLoggerFactory.Instance);

            var realMapper = mapperConfig.CreateMapper();

            var manager = new PrintManager(
                _printDbManagerMock.Object,
                realMapper,
                _loggerMock.Object
            );


            var result = manager.GetPrintList(groupId, out BaseError? error);


            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Null(error);

            Assert.Equal("1h 2min", result[0].PrintTime);
            Assert.Equal("0h 13min", result[1].PrintTime);

            _printDbManagerMock.Verify(db => db.GetPrintList(groupId), Times.Once);
        }
    }
}
