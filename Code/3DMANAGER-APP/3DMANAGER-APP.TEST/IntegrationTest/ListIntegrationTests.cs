using _3DMANAGER_APP.BLL.Mapper;
using _3DMANAGER_APP.BLL.Services;
using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Repositories;
using _3DMANAGER_APP.TEST.Fixture;
using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

namespace _3DMANAGER_APP.TEST.IntegrationTest
{
    [Collection("Database")]
    public class ListIntegrationTests
    {
        private readonly DatabaseFixture _fixture;
        private readonly IMapper _mapper;

        public ListIntegrationTests(DatabaseFixture fixture)
        {
            _fixture = fixture;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }, NullLoggerFactory.Instance);

            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Lists_ShouldReturnList()
        {
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER"
            );

            var catalogRepository = new CatalogRepository(
                dataSource,
                NullLogger<CatalogRepository>.Instance
            );
            var printerRepository = new PrinterRepository(
               dataSource,
               NullLogger<PrinterRepository>.Instance
           );
            var service = new CatalogService(
                catalogRepository,
                printerRepository,
                _mapper,
                NullLogger<CatalogService>.Instance
            );

            var states = service.GetPrintState();
            Assert.NotNull(states);
            Assert.NotEmpty(states);

            var filamentTypes = service.GetFilamentType();
            Assert.NotNull(filamentTypes);
            Assert.NotEmpty(filamentTypes);

            var filaments = service.GetFilamentCatalog(1);
            Assert.NotNull(filaments);
            Assert.NotEmpty(filaments);

            var printers = service.GetPrinterCatalog(1);
            Assert.NotNull(printers);
            Assert.NotEmpty(printers);

        }


    }

}
