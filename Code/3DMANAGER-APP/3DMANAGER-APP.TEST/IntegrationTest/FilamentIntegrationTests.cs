using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Managers;
using _3DMANAGER_APP.BLL.Mapper;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Filament;
using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Managers;
using _3DMANAGER_APP.TEST.E2ETest;
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
        private readonly IAwsS3Service _fakeService;
        public FilamentIntegrationTests(DatabaseFixture fixture)
        {
            _fixture = fixture;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }, NullLoggerFactory.Instance);

            _mapper = config.CreateMapper();
            _fakeService = new FakeAwsS3Service();
        }

        [Fact]
        public void Filament_ShouldReturnFilamentsAndCreate()
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
                NullLogger<FilamentManager>.Instance,
                _fakeService
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
            var filamentPost = manager.PostFilament(newFilament);
            Assert.Null(error);
            Assert.NotNull(filamentPost);
            Assert.NotEqual(0, filamentPost.Result.Data);
            var filamentsAfterPost = manager.GetFilamentList(1, out error);
            Assert.Null(error);
            Assert.Equal(filaments.Count + 1, filamentsAfterPost.Count);
            Assert.NotNull(filamentsAfterPost);
            Assert.NotEmpty(filamentsAfterPost);

        }
    }
}
