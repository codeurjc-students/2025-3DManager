using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Managers;
using _3DMANAGER_APP.BLL.Mapper;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Filament;
using _3DMANAGER_APP.BLL.Models.File;
using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Managers;
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
            var s3Mock = new Mock<IAwsS3Service>();

            s3Mock.Setup(x => x.UploadImageAsync(
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

            s3Mock.Setup(x => x.DeleteImageAsync(It.IsAny<string>()))
                  .Returns(Task.CompletedTask);

            s3Mock.Setup(x => x.GetPresignedUrl(It.IsAny<string>(), It.IsAny<int>()))
                  .Returns("https://fake-url.com/presigned/test.jpg");

            _fakeService = s3Mock.Object;
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

        [Fact]
        public void Filament_ShouldReturnDetail_AndUpdate()
        {
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER");

            var filamentDbManager = new FilamentDbManager(
                dataSource,
                NullLogger<FilamentDbManager>.Instance);

            var manager = new FilamentManager(
                filamentDbManager,
                _mapper,
                NullLogger<FilamentManager>.Instance,
                _fakeService);

            BaseError error;

            var filament = manager.GetFilamentDetail(1, 1, out error);
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

            var result = manager.UpdateFilament(request);
            Assert.True(result);

            var updated = manager.GetFilamentDetail(1, filament.FilamentId, out error);
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

            var created = await manager.PostFilament(newFilament);
            Assert.NotNull(created);
            Assert.True(created.Data > 0);

            int filamentId = created.Data;

            var deleteResponse = await manager.DeleteFilament(filamentId, 1);

            Assert.NotNull(deleteResponse);
            Assert.True(deleteResponse.Data);
            Assert.Null(deleteResponse.Error);

            var filaments = manager.GetFilamentList(1, out BaseError? error);
            Assert.Null(error);
            Assert.DoesNotContain(filaments, f => f.FilamentId == filamentId);
        }

    }
}
