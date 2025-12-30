using _3DMANAGER_APP.BLL.Managers;
using _3DMANAGER_APP.BLL.Mapper;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Filament;
using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Managers;
using _3DMANAGER_APP.TEST.Fixture;
using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

namespace _3DMANAGER_APP.TEST.IntegrationTest
{
    [Collection("Database")]
    public class FilamentIntegrationTests
    {
        private readonly DatabaseFixture _fixture;
        private readonly IMapper _mapper;

        public FilamentIntegrationTests(DatabaseFixture fixture)
        {
            _fixture = fixture;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }, NullLoggerFactory.Instance);

            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Printer_ShouldReturnPrintersAndCreate()
        {
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER"
            );

            var filamentDbManager = new FilamentDbManager(
                dataSource,
                NullLogger<FilamentDbManager>.Instance
            );

            var manager = new FilamentManager(
                filamentDbManager,
                _mapper,
                NullLogger<FilamentManager>.Instance
            );

            BaseError error;
            var filaments = manager.GetFilamentList(1, out error);

            Assert.Null(error);
            Assert.NotNull(filaments);
            Assert.NotEmpty(filaments);

            FilamentRequest newFilament = new FilamentRequest()
            {
                GroupId = 1,
                FilamentName = "New Filament Test",
                FilamentDescription = "Test Description",
                FilamentColor = "#ffffff",
                FilamentCost = 18,
                FilamentLenght = 200,
                FilamentTemperature = 205,
                FilamentThickness = 1,
                FilamentType = 1,
                FilamentWeight = 600,


            };
            var filamentPost = manager.PostFilament(newFilament, out error);
            Assert.Null(error);
            Assert.True(filamentPost);

            var filamentsAfterPost = manager.GetFilamentList(1, out error);
            Assert.Null(error);
            Assert.Equal(filaments.Count + 1, filamentsAfterPost.Count);
            Assert.NotNull(filamentsAfterPost);
            Assert.NotEmpty(filamentsAfterPost);

        }
    }
}
