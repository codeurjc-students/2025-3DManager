using _3DMANAGER.API.Models;
using _3DMANAGER.BLL.Managers;
using _3DMANAGER.BLL.Mapper;
using _3DMANAGER.DAL.Base;
using _3DMANAGER.DAL.Managers;
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
            // Leer appsettings.json
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true) // opcional
                .AddUserSecrets<IntegrationTests>() // lee secretos del proyecto de test
                .Build();
            _connectionString = config.GetConnectionString("TestConnection");

            // Configurar AutoMapper
            var loggerFactory = NullLoggerFactory.Instance;

            var expression = new MapperConfigurationExpression();
            expression.AddProfile<AutoMapperProfile>();

            var configMapper = new MapperConfiguration(expression, NullLoggerFactory.Instance);
            _mapper = configMapper.CreateMapper();
        }

        [Fact]
        public void GetPrinterList_IntegrationTest_ShouldReturnPrinters()
        {
            // Arrange
            var dataSource = new MySQLDataSource(_connectionString, "3DMANAGER");
            var printerDbManager = new PrinterDbManager(dataSource, NullLogger<PrinterDbManager>.Instance);
            var printerManager = new PrinterManager(printerDbManager, _mapper, NullLogger<PrinterManager>.Instance);

            // Act
            BaseError error;
            var printers = printerManager.GetPrinterList(out error);

            // Assert

            Assert.Null(error); // No debe haber errores
            Assert.NotNull(printers);
            Assert.True(printers.Count > 0, "Debe devolver al menos una impresora de ejemplo");

            foreach (var printer in printers)
                Console.WriteLine(printer.PrinterName);
        }
    }
}
