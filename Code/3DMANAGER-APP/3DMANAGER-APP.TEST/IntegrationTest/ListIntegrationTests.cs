using _3DMANAGER_APP.BLL.Managers;
using _3DMANAGER_APP.BLL.Mapper;
using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Managers;
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

            var catalogDbManager = new CatalogDbManager(
                dataSource,
                NullLogger<CatalogDbManager>.Instance
            );

            var manager = new CatalogManager(
                catalogDbManager,
                _mapper,
                NullLogger<CatalogManager>.Instance
            );

            var states = manager.GetPrintState();
            Assert.NotNull(states);
            Assert.NotEmpty(states);

            var filamentTypes = manager.GetFilamentType();
            Assert.NotNull(filamentTypes);
            Assert.NotEmpty(filamentTypes);

            var filaments = manager.GetFilamentCatalog(1);
            Assert.NotNull(filaments);
            Assert.NotEmpty(filaments);

            var printers = manager.GetPrinterCatalog(1);
            Assert.NotNull(printers);
            Assert.NotEmpty(printers);

        }


    }

}
