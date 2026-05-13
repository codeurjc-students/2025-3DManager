using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Mapper;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.File;
using _3DMANAGER_APP.BLL.Models.Printer;
using _3DMANAGER_APP.BLL.Services;
using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Repositories;
using _3DMANAGER_APP.TEST.Fixture;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace _3DMANAGER_APP.TEST.IntegrationTest
{
    [Collection("Database")]
    public class PrinterIntegrationTests
    {
        private readonly DatabaseFixture _fixture;
        private readonly IMapper _mapper;
        private readonly IAzureBlobStorageService _aBSService;


        public PrinterIntegrationTests(DatabaseFixture fixture)
        {
            _fixture = fixture;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }, NullLoggerFactory.Instance);

            _mapper = config.CreateMapper();

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

            _aBSService = absMock.Object;

        }

        [Fact]
        public void Printer_ShouldReturnPrintersAndCreate()
        {
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER"
            );

            var printerRepository = new PrinterRepository(
                dataSource,
                NullLogger<PrinterRepository>.Instance
            );

            var service = new PrinterService(
                printerRepository,
                _mapper,
                NullLogger<PrinterService>.Instance,
                _aBSService
            );

            BaseError? error;
            var printers = service.GetPrinterDashboardList(1, out error);

            Assert.Null(error);
            Assert.NotNull(printers);
            Assert.NotEmpty(printers);

            PrinterRequest newPrinter = new PrinterRequest()
            {
                GroupId = 1,
                PrinterName = "New Printer",
                PrinterModel = "Model test",
                PrinterDescription = "NewDescription"
            };
            var printersPost = service.PostPrinter(newPrinter);
            Assert.Null(error);

            var printersAfterPost = service.GetPrinterDashboardList(1, out error);
            Assert.Null(error);
            Assert.Equal(printers.Count + 1, printersAfterPost.Count);
            Assert.NotNull(printersAfterPost);
            Assert.NotEmpty(printersAfterPost);

        }

        [Fact]
        public void GetPrinterDetail_ShouldReturnError_WhenPrinterDoesNotExist()
        {
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER"
            );

            var printerRepository = new PrinterRepository(
                dataSource,
                NullLogger<PrinterRepository>.Instance
            );

            var service = new PrinterService(
                printerRepository,
                _mapper,
                NullLogger<PrinterService>.Instance,
                _aBSService
            );

            var result = service.GetPrinterDetail(1, -999, out BaseError? error);

            Assert.NotNull(error);
            Assert.Equal(500, error.code);
            Assert.Equal(null, result.PrinterId);
        }

        [Fact]
        public async Task GetPrinterDashboardList_ShouldUseDefaultImage_WhenNoImage()
        {
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER"
            );

            var printerRepository = new PrinterRepository(
                dataSource,
                NullLogger<PrinterRepository>.Instance
            );

            var service = new PrinterService(
                printerRepository,
                _mapper,
                NullLogger<PrinterService>.Instance,
                _aBSService
            );

            var created = await service.PostPrinter(new PrinterRequest
            {
                GroupId = 1,
                PrinterName = $"Impresora Test POST",
                PrinterDescription = "Test",
                PrinterModel = "Test"
            });

            var list = service.GetPrinterDashboardList(1, out BaseError? error);

            Assert.Null(error);

            var printer = list.First(p => p.PrinterId == created.Data);

            Assert.NotNull(printer.PrinterImageData);
            Assert.Contains("https://fake-url.com/presigned/test.jpg", printer.PrinterImageData.FileUrl);
        }

        [Fact]
        public async Task PostPrinter_ShouldReturnConflict_WhenNameExists()
        {
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER"
            );

            var printerRepository = new PrinterRepository(
                dataSource,
                NullLogger<PrinterRepository>.Instance
            );

            var service = new PrinterService(
                printerRepository,
                _mapper,
                NullLogger<PrinterService>.Instance,
                _aBSService
            );


            var request = new PrinterRequest
            {
                GroupId = 1,
                PrinterName = "Impresora Test POST",
                PrinterDescription = "Test",
                PrinterModel = "Model"
            };

            await service.PostPrinter(request);
            var result = await service.PostPrinter(request);

            Assert.NotNull(result.Error);
            Assert.Equal(409, result.Error.Code);
        }
        [Fact]
        public void UpdatePrinter_ShouldModifyPrinterCorrectly()
        {
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER"
            );

            var printerRepository = new PrinterRepository(
                dataSource,
                NullLogger<PrinterRepository>.Instance
            );

            var service = new PrinterService(
                printerRepository,
                _mapper,
                NullLogger<PrinterService>.Instance,
                _aBSService
            );

            BaseError? error;

            var printers = service.GetPrinterDashboardList(1, out error);
            Assert.Null(error);
            Assert.NotEmpty(printers);

            var printer = printers.First();
            var request = new PrinterDetailRequest
            {
                GroupId = 1,
                PrinterId = 1,
                PrinterName = "Updated Name Test",
                PrinterDescription = "Updated Description Test",
                PrinterModel = "Updated Model Test",
                PrinterStateId = 2
            };

            var result = service.UpdatePrinter(request, out BaseError? errorR);

            Assert.True(result);
            Assert.Null(errorR);
            var updatedPrinters = service.GetPrinterDashboardList(1, out error);
            Assert.Null(error);

        }

        [Fact]
        public async Task UpdatePrinter_ShouldReturnError_WhenNameDuplicated()
        {
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER"
            );

            var printerRepository = new PrinterRepository(
                dataSource,
                NullLogger<PrinterRepository>.Instance
            );

            var service = new PrinterService(
                printerRepository,
                _mapper,
                NullLogger<PrinterService>.Instance,
                _aBSService
            );

            var name = "Impresora Duplicada";

            var p1 = await service.PostPrinter(new PrinterRequest
            {
                GroupId = 1,
                PrinterName = name,
                PrinterDescription = "Test",
                PrinterModel = "Model"
            });

            var p2 = await service.PostPrinter(new PrinterRequest
            {
                GroupId = 1,
                PrinterName = "Impresora No Duplicada",
                PrinterDescription = "Test",
                PrinterModel = "Model"
            });

            var request = new PrinterDetailRequest
            {
                GroupId = 1,
                PrinterId = p2.Data,
                PrinterName = name,
                PrinterDescription = "Updated",
                PrinterModel = "Updated",
                PrinterStateId = 1
            };

            var result = service.UpdatePrinter(request, out BaseError? error);

            Assert.False(result);
            Assert.NotNull(error);
        }


        [Fact]
        public async Task DeletePrinter_ShouldDeleteSuccessfully()
        {
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER"
            );

            var printerRepository = new PrinterRepository(
                dataSource,
                NullLogger<PrinterRepository>.Instance
            );

            var service = new PrinterService(
                printerRepository,
                _mapper,
                NullLogger<PrinterService>.Instance,
                _aBSService
            );

            var newPrinter = new PrinterRequest
            {
                GroupId = 1,
                PrinterName = "Impresora prueba",
                PrinterDescription = "Descripcion prueba",
                PrinterModel = "Modelo prueba"
            };

            var created = await service.PostPrinter(newPrinter);
            Assert.NotNull(created);
            Assert.True(created.Data > 0);

            int printerId = created.Data;

            var deleteResponse = await service.DeletePrinter(printerId, 1);

            Assert.NotNull(deleteResponse);
            Assert.True(deleteResponse.Data);

            var printers = service.GetPrinterDashboardList(1, out BaseError? error);
            Assert.Null(error);
            Assert.DoesNotContain(printers, p => p.PrinterId == printerId);
        }

        [Fact]
        public async Task DeletePrinterImage_ShouldReturnTrue_WhenNoImageExists()
        {
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER"
            );

            var printerRepository = new PrinterRepository(
                dataSource,
                NullLogger<PrinterRepository>.Instance
            );

            var service = new PrinterService(
                printerRepository,
                _mapper,
                NullLogger<PrinterService>.Instance,
                _aBSService
            );

            var created = await service.PostPrinter(new PrinterRequest
            {
                GroupId = 1,
                PrinterName = $"NoImageDelete_{Guid.NewGuid()}",
                PrinterDescription = "Test",
                PrinterModel = "Model"
            });

            var result = await service.DeletePrinterImage(created.Data, 1);

            Assert.True(result.Data);
        }

        [Fact]
        public async Task UpdatePrinterImage_ShouldReturnError_WhenImageIsNull()
        {
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER"
            );

            var printerRepository = new PrinterRepository(
                dataSource,
                NullLogger<PrinterRepository>.Instance
            );

            var service = new PrinterService(
                printerRepository,
                _mapper,
                NullLogger<PrinterService>.Instance,
                _aBSService
            );

            var result = await service.UpdatePrinterImage(1, 1, null);

            Assert.NotNull(result.Error);
            Assert.Equal(400, result.Error.Code);
        }

        [Fact]
        public async Task UpdatePrinterImage_ShouldUpdateSuccessfully()
        {
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER"
            );

            var printerRepository = new PrinterRepository(
                dataSource,
                NullLogger<PrinterRepository>.Instance
            );

            var service = new PrinterService(
                printerRepository,
                _mapper,
                NullLogger<PrinterService>.Instance,
                _aBSService
            );

            var created = await service.PostPrinter(new PrinterRequest
            {
                GroupId = 1,
                PrinterName = $"ImageUpdate_{Guid.NewGuid()}",
                PrinterDescription = "Test",
                PrinterModel = "Model"
            });

            var file = new FormFile(
                new MemoryStream(new byte[] { 1, 2, 3 }),
                0,
                3,
                "file",
                "test.jpg"
            )
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            var result = await service.UpdatePrinterImage(created.Data, 1, file);

            Assert.True(result.Data);
        }
    }
}
