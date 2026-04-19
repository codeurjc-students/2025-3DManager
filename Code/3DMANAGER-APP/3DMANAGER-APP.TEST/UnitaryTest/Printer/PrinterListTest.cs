using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace _3DMANAGER_APP.TEST.UnitaryTest
{
    public class PrinterListTest
    {
        [Fact]
        [Trait("Category", "Unitary")]
        public void GetPrinterList_WhenDbReturnsData_ShouldReturnMappedPrinters()
        {
            var mockDb = new Mock<IPrinterRepository>();
            var mockLogger = new Mock<ILogger<PrinterService>>();
            var mockMapper = new Mock<IMapper>();
            var mockABSService = new Mock<IAzureBlobStorageService>();

            var dbPrinters = new List<PrinterDbObject>
        {
            new PrinterDbObject { PrinterName = "HP" },
            new PrinterDbObject { PrinterName = "Epson" }
        };

            mockDb.Setup(m => m.GetPrinterList(out It.Ref<ErrorDbObject>.IsAny))
              .Returns(new GetPrinterListCallback((out ErrorDbObject err) =>
              {
                  err = null;
                  return dbPrinters;
              }));

            mockMapper.Setup(m => m.Map<List<PrinterObject>>(dbPrinters))
                      .Returns(new List<PrinterObject>
                      {
                      new PrinterObject { PrinterName = "Impresora 1" },
                      new PrinterObject { PrinterName = "Impresora 2" }
                      });

            var service = new PrinterService(mockDb.Object, mockMapper.Object, mockLogger.Object, mockABSService.Object);

            var result = service.GetPrinterList(out BaseError error);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Impresora 1", result[0].PrinterName);
            Assert.Null(error);
        }

        [Fact]
        [Trait("Category", "Unitary")]
        public void GetPrinterList_WhenDbReturnsError_ShouldSetBaseError()
        {
            var mockDb = new Mock<IPrinterRepository>();
            var mockLogger = new Mock<ILogger<PrinterService>>();
            var mockMapper = new Mock<IMapper>();
            var mockABSService = new Mock<IAzureBlobStorageService>();

            var dbError = new ErrorDbObject { code = 500, message = "DB Error" };

            mockDb.Setup(m => m.GetPrinterList(out It.Ref<ErrorDbObject>.IsAny))
              .Returns(new GetPrinterListCallback((out ErrorDbObject err) =>
              {
                  err = dbError;
                  return null!;
              }));

            var service = new PrinterService(mockDb.Object, mockMapper.Object, mockLogger.Object, mockABSService.Object);

            var result = service.GetPrinterList(out BaseError error);

            Assert.Null(result);
            Assert.NotNull(error);
            Assert.Equal(500, error.code);
            Assert.Equal("DB Error", error.message);
        }
        private delegate List<PrinterDbObject> GetPrinterListCallback(out ErrorDbObject errorDb);

    }
}
