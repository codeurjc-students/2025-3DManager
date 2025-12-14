using _3DMANAGER_APP.BLL.Managers;
using _3DMANAGER_APP.BLL.Mapper;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Managers;
using _3DMANAGER_APP.TEST.Models;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;

namespace _3DMANAGER.TEST.IntegrationTest
{
    public class IntegrationTests
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;

        public IntegrationTests()
        {

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets<IntegrationTests>()
                .Build();
            _connectionString = config.GetConnectionString("TestConnection");


            var loggerFactory = NullLoggerFactory.Instance;

            var expression = new MapperConfigurationExpression();
            expression.AddProfile<AutoMapperProfile>();

            var configMapper = new MapperConfiguration(expression, NullLoggerFactory.Instance);
            _mapper = configMapper.CreateMapper();
        }

        [Fact]
        public void GetPrinterList_IntegrationTest_ShouldReturnPrinters()
        {


            var isCI = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "CI";


            IPrinterDbManager printerDbManager;
            if (isCI)
            {
                printerDbManager = new FakePrinterDbManager();
            }
            else
            {
                var dataSource = new MySQLDataSource(_connectionString, "3DMANAGER");
                printerDbManager = new PrinterDbManager(dataSource, NullLogger<PrinterDbManager>.Instance);
            }

            var printerManager = new PrinterManager(printerDbManager, _mapper, NullLogger<PrinterManager>.Instance);

            BaseError error;
            var printers = printerManager.GetPrinterList(out error);


            Assert.Null(error);
            Assert.NotNull(printers);
            Assert.True(printers.Count > 0, "Debe devolver al menos una impresora de ejemplo");

            foreach (var printer in printers)
                Console.WriteLine(printer.PrinterName);
        }
    }
}
