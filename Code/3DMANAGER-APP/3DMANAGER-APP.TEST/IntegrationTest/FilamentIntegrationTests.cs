using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Mapper;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Filament;
using _3DMANAGER_APP.BLL.Models.File;
using _3DMANAGER_APP.BLL.Services;
using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Repositories;
using _3DMANAGER_APP.TEST.E2ETest;
using _3DMANAGER_APP.TEST.Fixture;
using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace _3DMANAGER_APP.TEST.IntegrationTest
{
    [Collection("Database")]
    public class FilamentIntegrationTests
    {
        private readonly DatabaseFixture _fixture;
        private readonly IMapper _mapper;
        private readonly IAzureBlobStorageService _fakeService;
        private readonly INotificationService _notificationService;

        public FilamentIntegrationTests(DatabaseFixture fixture)
        {
            _fixture = fixture;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }, NullLoggerFactory.Instance);

            _mapper = config.CreateMapper();
            _fakeService = new FakeAzureBlobStorageService();
            var absMock = new Mock<IAzureBlobStorageService>();

            absMock.Setup(x => x.UploadImageAsync(
                    It.IsAny<Stream>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<int>()
            ))
            .ReturnsAsync(new FileResponse
            {
                FileKey = "printers/test.jpg",
                FileUrl = "https://fake-url.com/printers/test.jpg"
            });

            absMock.Setup(x => x.DeleteImageAsync(It.IsAny<string>()))
                  .Returns(Task.CompletedTask);

            absMock.Setup(x => x.GetPresignedUrl(It.IsAny<string>(), It.IsAny<int>()))
                  .Returns("https://fake-url.com/presigned/test.jpg");

            _fakeService = absMock.Object;
        }

        [Fact]
        public async Task Filament_ShouldReturnFilamentsAndCreate()
        {
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER"
            );

            var filamentRepository = new FilamentRepository(
                dataSource,
                NullLogger<FilamentRepository>.Instance
            );

            var service = new FilamentService(
                filamentRepository,
                _mapper,
                NullLogger<FilamentService>.Instance,
                _fakeService,
                _notificationService
            );

            BaseError? error;
            var filaments = service.GetFilamentList(1, out error);

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
            var filamentPost = await service.PostFilament(newFilament);
            Assert.Null(error);
            Assert.NotNull(filamentPost);
            Assert.NotEqual(0, filamentPost.Data);

            var filamentsAfterPost = service.GetFilamentList(1, out error);
            Assert.Null(error);
            Assert.Equal(filaments.Count + 1, filamentsAfterPost.Count);
            Assert.NotNull(filamentsAfterPost);
            Assert.NotEmpty(filamentsAfterPost);

        }

        [Fact]
        public void Filament_ShouldReturnDetail_AndUpdate()
        {
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER");

            var filamentRepository = new FilamentRepository(
                dataSource,
                NullLogger<FilamentRepository>.Instance);

            var service = new FilamentService(
                filamentRepository,
                _mapper,
                NullLogger<FilamentService>.Instance,
                _fakeService,
                _notificationService);

            BaseError? error;

            var filament = service.GetFilamentDetail(1, 1, out error);
            Assert.Null(error);
            Assert.NotNull(filament);

            var request = new FilamentUpdateRequest
            {
                GroupId = 1,
                FilamentId = filament.FilamentId,
                FilamentName = "E2E Filament Updated",
                FilamentColor = "#FFFFFF",
                FilamentCost = 25,
                FilamentDescription = "E2E Filament description updated",
                FilamentLenght = 100,
                FilamentTemperature = 220,
                ImageFile = null,

            };

            var result = service.UpdateFilament(request);
            Assert.True(result);

            var updated = service.GetFilamentDetail(1, filament.FilamentId, out error);
            Assert.Null(error);
            Assert.NotNull(updated);
            Assert.Equal("E2E Filament Updated", updated.FilamentName);
            Assert.Equal("#FFFFFF", updated.FilamentColor);
            Assert.Equal(25, updated.FilamentCost);
            Assert.Equal(100, updated.FilamentRemainingLenght);
            Assert.Equal(220, updated.FilamentTemperature);
        }

        [Fact]
        public async Task DeleteFilament_ShouldDeleteSuccessfully()
        {
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER"
            );

            var filamentRepository = new FilamentRepository(
                dataSource,
                NullLogger<FilamentRepository>.Instance
            );

            var service = new FilamentService(
                filamentRepository,
                _mapper,
                NullLogger<FilamentService>.Instance,
                _fakeService,
                _notificationService
            );

            // Creamos un filamento de prueba
            var newFilament = new FilamentRequest
            {
                GroupId = 1,
                FilamentName = "Filament prueba",
                FilamentDescription = "Test prueba",
                FilamentColor = "#ffffff",
                FilamentWeight = 1000,
                FilamentCost = 20,
                FilamentLenght = 300,
                FilamentTemperature = 200,
                FilamentThickness = 1,
                FilamentType = 1

            };

            var created = await service.PostFilament(newFilament);
            Assert.NotNull(created);
            Assert.True(created.Data > 0);

            int filamentId = created.Data;

            var deleteResponse = await service.DeleteFilament(filamentId, 1);

            Assert.NotNull(deleteResponse);
            Assert.True(deleteResponse.Data);

            var filaments = service.GetFilamentList(1, out BaseError? error);
            Assert.Null(error);
            Assert.DoesNotContain(filaments, f => f.FilamentId == filamentId);
        }

    }
}
