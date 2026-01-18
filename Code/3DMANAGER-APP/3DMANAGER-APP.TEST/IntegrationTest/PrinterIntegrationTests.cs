using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Managers;
using _3DMANAGER_APP.BLL.Mapper;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.File;
using _3DMANAGER_APP.BLL.Models.Printer;
using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Managers;
using _3DMANAGER_APP.TEST.Fixture;
using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace _3DMANAGER_APP.TEST.IntegrationTest
{
    [Collection("Database")]
    public class PrinterIntegrationTests
    {
        private readonly DatabaseFixture _fixture;
        private readonly IMapper _mapper;
        private readonly IAwsS3Service _s3;


        public PrinterIntegrationTests(DatabaseFixture fixture)
        {
            _fixture = fixture;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }, NullLoggerFactory.Instance);

            _mapper = config.CreateMapper();

            //Mocking a AWS service. Test does not use AWS service
            var s3Mock = new Mock<IAwsS3Service>();

            s3Mock.Setup(x => x.UploadImageAsync(
                    It.IsAny<Stream>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()
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

            _s3 = s3Mock.Object;

        }

        [Fact]
        public void Printer_ShouldReturnPrintersAndCreate()
        {
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER"
            );

            var printerDbManager = new PrinterDbManager(
                dataSource,
                NullLogger<PrinterDbManager>.Instance
            );

            var manager = new PrinterManager(
                printerDbManager,
                _mapper,
                NullLogger<PrinterManager>.Instance,
                _s3
            );

            BaseError error;
            var printers = manager.GetPrinterDashboardList(1, out error);

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
            var printersPost = manager.PostPrinter(newPrinter);
            Assert.Null(error);

            var printersAfterPost = manager.GetPrinterDashboardList(1, out error);
            Assert.Null(error);
            Assert.Equal(printers.Count + 1, printersAfterPost.Count);
            Assert.NotNull(printersAfterPost);
            Assert.NotEmpty(printersAfterPost);

        }
    }
}
