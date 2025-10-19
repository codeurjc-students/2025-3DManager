using _3DMANAGER_APP.BLL.Managers;
using _3DMANAGER_APP.BLL.Models;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models;
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
            // Arrange
            var mockDb = new Mock<IPrinterDbManager>();
            var mockLogger = new Mock<ILogger<PrinterManager>>();
            var mockMapper = new Mock<IMapper>();

            // Simulación de lo que devuelve la BBDD
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

            // Configuramos el mapper
            mockMapper.Setup(m => m.Map<List<PrinterObject>>(dbPrinters))
                      .Returns(new List<PrinterObject>
                      {
                      new PrinterObject { PrinterName = "Impresora 1" },
                      new PrinterObject { PrinterName = "Impresora 2" }
                      });

            var manager = new PrinterManager(mockDb.Object, mockMapper.Object, mockLogger.Object);

            // Act
            var result = manager.GetPrinterList(out BaseError error);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Impresora 1", result[0].PrinterName);
            Assert.Null(error);
        }

        [Fact]
        [Trait("Category", "Unitary")]
        public void GetPrinterList_WhenDbReturnsError_ShouldSetBaseError()
        {
            // Arrange
            var mockDb = new Mock<IPrinterDbManager>();
            var mockLogger = new Mock<ILogger<PrinterManager>>();
            var mockMapper = new Mock<IMapper>();

            var dbError = new ErrorDbObject { code = 500, message = "DB Error" };

            mockDb.Setup(m => m.GetPrinterList(out It.Ref<ErrorDbObject>.IsAny))
              .Returns(new GetPrinterListCallback((out ErrorDbObject err) =>
              {
                  err = dbError;
                  return null!;
              }));

            var manager = new PrinterManager(mockDb.Object, mockMapper.Object, mockLogger.Object);

            // Act
            var result = manager.GetPrinterList(out BaseError error);

            // Assert
            Assert.Null(result); // no hay lista
            Assert.NotNull(error);
            Assert.Equal(500, error.code);
            Assert.Equal("DB Error", error.message);
        }
        private delegate List<PrinterDbObject> GetPrinterListCallback(out ErrorDbObject errorDb);

    }
}
