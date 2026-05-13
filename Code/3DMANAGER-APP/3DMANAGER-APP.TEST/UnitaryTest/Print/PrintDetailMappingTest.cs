using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Mapper;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Services;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.File;
using _3DMANAGER_APP.DAL.Models.Print;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace _3DMANAGER_APP.TEST.UnitaryTest
{
    public class PrintDetailMappingTest
    {
        private readonly Mock<ILogger<PrintService>> _loggerMock;
        private readonly Mock<IPrintRepository> _printRepositoryMock;
        private readonly Mock<IAzureBlobStorageService> _absServiceMock;
        private readonly Mock<INotificationService> _notificationServiceMock;

        public PrintDetailMappingTest()
        {
            _loggerMock = new Mock<ILogger<PrintService>>();
            _printRepositoryMock = new Mock<IPrintRepository>();
            _absServiceMock = new Mock<IAzureBlobStorageService>();
            _notificationServiceMock = new Mock<INotificationService>();
        }

        [Fact]
        [Trait("Category", "Unitary")]
        public void GetPrintDetail_WhenPrintHasImage_ShouldSetHaveSTLToTrue()
        {
            int groupId = 1;
            int printId = 1;

            var dbObject = new PrintDetailDbObject
            {
                PrintId = printId,
                PrintName = "Test",
                PrintTimeImpression = 3660,
                PrintRealTimeImpression = 7200,
                FilamentCost = 2,
                PrintMaterialConsumed = 10,
                PrintImageData = new FileResponseDbObject
                {
                    FileKey = "prints/test.stl",
                    FileUrl = "fake"
                }
            };

            _printRepositoryMock
                .Setup(x => x.GetPrintDetail(groupId, printId))
                .Returns(dbObject);

            _absServiceMock
                .Setup(x => x.GetPresignedUrl(It.IsAny<string>(), It.IsAny<int>()))
                .Returns("https://fake-url.com/presigned/test.jpg");

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }, NullLoggerFactory.Instance);

            var mapper = mapperConfig.CreateMapper();

            var service = new PrintService(
                _printRepositoryMock.Object,
                mapper,
                _loggerMock.Object,
                _absServiceMock.Object,
                _notificationServiceMock.Object);

            var result = service.GetPrintDetail(groupId, printId, out BaseError? error);

            Assert.Null(error);
            Assert.True(result.PrintHaveSTL);
            Assert.Equal("1h 1min", result.PrintTimeImpression);
            Assert.Equal("2h 0min", result.PrintRealTimeImpression);
            Assert.Equal(20, result.PrintEstimedCost);
        }

        [Fact]
        [Trait("Category", "Unitary")]
        public void GetPrintDetail_WhenPrintHasNoImage_ShouldSetDefaultSTL()
        {
            int groupId = 1;
            int printId = 1;

            var dbObject = new PrintDetailDbObject
            {
                PrintId = printId,
                PrintName = "Test",
                PrintImageData = new FileResponseDbObject()
            };

            _printRepositoryMock
                .Setup(x => x.GetPrintDetail(groupId, printId))
                .Returns(dbObject);

            _absServiceMock
                .Setup(x => x.GetPresignedUrl(It.IsAny<string>(), It.IsAny<int>()))
                .Returns("https://fake-url.com/presigned/test.jpg");

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }, NullLoggerFactory.Instance);

            var mapper = mapperConfig.CreateMapper();

            var service = new PrintService(
                _printRepositoryMock.Object,
                mapper,
                _loggerMock.Object,
                _absServiceMock.Object,
                _notificationServiceMock.Object);

            var result = service.GetPrintDetail(groupId, printId, out BaseError? error);

            Assert.Null(error);
            Assert.False(result.PrintHaveSTL);
            Assert.Contains("https://fake-url.com/presigned/test.jpg", result.PrintImageData.FileUrl);
        }

        [Fact]
        [Trait("Category", "Unitary")]
        public void GetPrintList_ShouldCalculateTotalPages()
        {
            int totalItems = 25;
            bool errorDb = false;

            var dbResponse = new List<PrintListResponseDbObject>
        {
            new PrintListResponseDbObject
            {
                PrintId = 1,
                PrintName = "Test",
                PrintTime = 3600
            }
        };

            _printRepositoryMock
                .Setup(x => x.GetPrintList(1, 1, 10, out totalItems, out errorDb))
                .Returns(dbResponse);

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }, NullLoggerFactory.Instance);

            var mapper = mapperConfig.CreateMapper();

            var service = new PrintService(
                _printRepositoryMock.Object,
                mapper,
                _loggerMock.Object,
                _absServiceMock.Object,
                _notificationServiceMock.Object);

            var result = service.GetPrintList(
                1,
                new PagedRequest
                {
                    PageNumber = 1,
                    PageSize = 10
                },
                out BaseError? error);

            Assert.Null(error);
            Assert.Equal(25, result.TotalItems);
            Assert.Equal(3, result.TotalPages);
        }

        [Fact]
        [Trait("Category", "Unitary")]
        public async Task DeletePrintImage_WhenNoImage_ShouldReturnTrue()
        {
            bool errorDb = false;

            _printRepositoryMock
                .Setup(x => x.GetPrintImageData(1, 1, out errorDb))
                .Returns(new FileResponseDbObject
                {
                    FileKey = null
                });

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }, NullLoggerFactory.Instance);

            var mapper = mapperConfig.CreateMapper();

            var service = new PrintService(
                _printRepositoryMock.Object,
                mapper,
                _loggerMock.Object,
                _absServiceMock.Object,
                _notificationServiceMock.Object);

            var result = await service.DeletePrintImage(1, 1);

            Assert.True(result.Data);
            Assert.Null(result.Error);
        }

        [Fact]
        [Trait("Category", "Unitary")]
        public async Task UpdatePrintImage_WhenImageIsNull_ShouldReturn400()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }, NullLoggerFactory.Instance);

            var mapper = mapperConfig.CreateMapper();

            var service = new PrintService(
                _printRepositoryMock.Object,
                mapper,
                _loggerMock.Object,
                _absServiceMock.Object,
                _notificationServiceMock.Object);

            var result = await service.UpdatePrintImage(1, 1, null);

            Assert.NotNull(result.Error);
            Assert.Equal(400, result.Error.Code);
        }
    }
}
