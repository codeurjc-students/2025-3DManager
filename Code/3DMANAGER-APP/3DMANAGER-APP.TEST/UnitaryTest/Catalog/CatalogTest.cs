using _3DMANAGER_APP.BLL.Models.Catalog;
using _3DMANAGER_APP.BLL.Services;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.Filament;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace _3DMANAGER_APP.TEST.UnitaryTest.Catalogs
{
    public class CatalogTests
    {
        private readonly Mock<ILogger<CatalogService>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICatalogRepository> _catalogRepositoryMock;
        private readonly CatalogService _service;
        private readonly Mock<IPrinterRepository> _printerRepositoryMock;
        public CatalogTests()
        {
            _loggerMock = new Mock<ILogger<CatalogService>>();
            _mapperMock = new Mock<IMapper>();
            _catalogRepositoryMock = new Mock<ICatalogRepository>();
            _printerRepositoryMock = new Mock<IPrinterRepository>();
            _loggerMock = new Mock<ILogger<CatalogService>>();

            _service = new CatalogService(
                _catalogRepositoryMock.Object,
                _printerRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object

            );
        }

        [Fact]
        [Trait("Category", "Unitary")]
        public void GetFilamentType_WhenDbReturnsData_ShouldReturnMappedCatalogResponse()
        {

            var dbResponse = new List<CatalogResponseDbObject>
        {
            new CatalogResponseDbObject { Id = 1, Description = "PLA" },
            new CatalogResponseDbObject { Id = 2, Description = "ABS" }
        };

            var mappedResponse = new List<CatalogResponse>
        {
            new CatalogResponse { Id = 1, Description = "PLA" },
            new CatalogResponse { Id = 2, Description = "ABS" }
        };

            _catalogRepositoryMock
                .Setup(db => db.GetFilamentType())
                .Returns(dbResponse);

            _mapperMock
                .Setup(m => m.Map<List<CatalogResponse>>(dbResponse))
                .Returns(mappedResponse);


            var result = _service.GetFilamentType();


            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("PLA", result[0].Description);
            Assert.Equal("ABS", result[1].Description);

            _catalogRepositoryMock.Verify(db => db.GetFilamentType(), Times.Once);
            _mapperMock.Verify(m => m.Map<List<CatalogResponse>>(dbResponse), Times.Once);
        }


        [Fact]
        [Trait("Category", "Unitary")]
        public void GetFilamentCatalog_WhenDbReturnsData_ShouldReturnMappedCatalogResponse()
        {

            int groupId = 1;

            var dbResponse = new List<CatalogResponseDbObject>
    {
        new CatalogResponseDbObject { Id = 1, Description = "Filamento 1" },
        new CatalogResponseDbObject { Id = 2, Description = "Filamento 2" }
    };

            var mappedResponse = new List<CatalogResponse>
    {
        new CatalogResponse { Id = 1, Description = "Filamento 1" },
        new CatalogResponse { Id = 2, Description = "Filamento 2" }
    };

            _catalogRepositoryMock
                .Setup(db => db.GetFilamentCatalog(groupId))
                .Returns(dbResponse);

            _mapperMock
                .Setup(m => m.Map<List<CatalogResponse>>(dbResponse))
                .Returns(mappedResponse);


            var result = _service.GetFilamentCatalog(groupId);


            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            _catalogRepositoryMock.Verify(db => db.GetFilamentCatalog(groupId), Times.Once);
            _mapperMock.Verify(m => m.Map<List<CatalogResponse>>(dbResponse), Times.Once);
        }

        [Fact]
        [Trait("Category", "Unitary")]
        public void GetPrinterCatalog_WhenDbReturnsData_ShouldReturnMappedCatalogResponse()
        {

            int groupId = 2;

            var dbResponse = new List<CatalogResponseDbObject>
            {
                new CatalogResponseDbObject { Id = 1, Description = "Ender 3" }
            };

            var mappedResponse = new List<CatalogPrinterResponse>
            {
                new CatalogPrinterResponse { Id = 1, Description = "Ender 3 VE" , TimeVariation = 0}
            };

            _catalogRepositoryMock
                .Setup(db => db.GetPrinterCatalog(groupId))
                .Returns(dbResponse);

            _mapperMock
                .Setup(m => m.Map<List<CatalogPrinterResponse>>(dbResponse))
                .Returns(mappedResponse);

            var result = _service.GetPrinterCatalog(groupId);

            Assert.Single(result);

            _catalogRepositoryMock.Verify(db => db.GetPrinterCatalog(groupId), Times.Once);
            _mapperMock.Verify(m => m.Map<List<CatalogPrinterResponse>>(dbResponse), Times.Once);
        }


        [Fact]
        [Trait("Category", "Unitary")]
        public void GetPrintState_WhenDbReturnsData_ShouldReturnMappedCatalogResponse()
        {

            var dbResponse = new List<CatalogResponseDbObject>
    {
        new CatalogResponseDbObject { Id = 1, Description = "Completado" },
        new CatalogResponseDbObject { Id = 2, Description = "No Completado" }
    };

            var mappedResponse = new List<CatalogResponse>
    {
        new CatalogResponse { Id = 1, Description = "Completado" },
        new CatalogResponse { Id = 2, Description = "No Completado" }
    };

            _catalogRepositoryMock
                .Setup(db => db.GetPrintState())
                .Returns(dbResponse);

            _mapperMock
                .Setup(m => m.Map<List<CatalogResponse>>(dbResponse))
                .Returns(mappedResponse);


            var result = _service.GetPrintState();


            Assert.Equal(2, result.Count);

            _catalogRepositoryMock.Verify(db => db.GetPrintState(), Times.Once);
            _mapperMock.Verify(m => m.Map<List<CatalogResponse>>(dbResponse), Times.Once);
        }
    }
}
