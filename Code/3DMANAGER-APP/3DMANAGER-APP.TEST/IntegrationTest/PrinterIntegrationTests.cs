using _3DMANAGER_APP.BLL.Managers;
using _3DMANAGER_APP.BLL.Mapper;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Printer;
using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Managers;
using _3DMANAGER_APP.TEST.Fixture;
using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

namespace _3DMANAGER_APP.TEST.IntegrationTest
{
    [Collection("Database")]
    public class PrinterIntegrationTests
    {
        private readonly DatabaseFixture _fixture;
        private readonly IMapper _mapper;

        public PrinterIntegrationTests(DatabaseFixture fixture)
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

            var printerDbManager = new PrinterDbManager(
                dataSource,
                NullLogger<PrinterDbManager>.Instance
            );

            var manager = new PrinterManager(
                printerDbManager,
                _mapper,
                NullLogger<PrinterManager>.Instance
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
            var printersPost = manager.PostPrinter(newPrinter, out error);
            Assert.Null(error);
            Assert.True(printersPost);

            var printersAfterPost = manager.GetPrinterDashboardList(1, out error);
            Assert.Null(error);
            Assert.Equal(printers.Count + 1, printersAfterPost.Count);
            Assert.NotNull(printersAfterPost);
            Assert.NotEmpty(printersAfterPost);

        }
    }
}
