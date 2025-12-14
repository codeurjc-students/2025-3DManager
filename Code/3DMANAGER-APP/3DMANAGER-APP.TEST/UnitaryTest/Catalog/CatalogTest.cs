using _3DMANAGER_APP.BLL.Managers;
using _3DMANAGER_APP.BLL.Models.Catalog;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.Filament;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace _3DMANAGER_APP.TEST.UnitaryTest.Catalogs
{
    public class CatalogTests
    {
        private readonly Mock<ILogger<CatalogManager>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICatalogDbManager> _catalogDbManagerMock;
        private readonly CatalogManager _manager;

        public CatalogTests()
        {
            _loggerMock = new Mock<ILogger<CatalogManager>>();
            _mapperMock = new Mock<IMapper>();
            _catalogDbManagerMock = new Mock<ICatalogDbManager>();

            _manager = new CatalogManager(
                _catalogDbManagerMock.Object,
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

            _catalogDbManagerMock
                .Setup(db => db.GetFilamentType())
                .Returns(dbResponse);

            _mapperMock
                .Setup(m => m.Map<List<CatalogResponse>>(dbResponse))
                .Returns(mappedResponse);


            var result = _manager.GetFilamentType();


            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("PLA", result[0].Description);
            Assert.Equal("ABS", result[1].Description);

            _catalogDbManagerMock.Verify(db => db.GetFilamentType(), Times.Once);
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

            _catalogDbManagerMock
                .Setup(db => db.GetFilamentCatalog(groupId))
                .Returns(dbResponse);

            _mapperMock
                .Setup(m => m.Map<List<CatalogResponse>>(dbResponse))
                .Returns(mappedResponse);


            var result = _manager.GetFilamentCatalog(groupId);


            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            _catalogDbManagerMock.Verify(db => db.GetFilamentCatalog(groupId), Times.Once);
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

            var mappedResponse = new List<CatalogResponse>
    {
        new CatalogResponse { Id = 1, Description = "Ender 3 VE" }
    };

            _catalogDbManagerMock
                .Setup(db => db.GetPrinterCatalog(groupId))
                .Returns(dbResponse);

            _mapperMock
                .Setup(m => m.Map<List<CatalogResponse>>(dbResponse))
                .Returns(mappedResponse);


            var result = _manager.GetPrinterCatalog(groupId);


            Assert.Single(result);

            _catalogDbManagerMock.Verify(db => db.GetPrinterCatalog(groupId), Times.Once);
            _mapperMock.Verify(m => m.Map<List<CatalogResponse>>(dbResponse), Times.Once);
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

            _catalogDbManagerMock
                .Setup(db => db.GetPrintState())
                .Returns(dbResponse);

            _mapperMock
                .Setup(m => m.Map<List<CatalogResponse>>(dbResponse))
                .Returns(mappedResponse);


            var result = _manager.GetPrintState();


            Assert.Equal(2, result.Count);

            _catalogDbManagerMock.Verify(db => db.GetPrintState(), Times.Once);
            _mapperMock.Verify(m => m.Map<List<CatalogResponse>>(dbResponse), Times.Once);
        }
    }
}
