using _3DMANAGER_APP.BLL.Mapper;
using _3DMANAGER_APP.BLL.Models.Printer;
using _3DMANAGER_APP.DAL.Models.Printer;
using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

namespace _3DMANAGER_APP.TEST.UnitaryTest
{
    public class PrinterMappingTest
    {
        [Fact]
        [Trait("Category", "Unitary")]
        public void PrinterDetailMapping_ShouldConvertHoursAndSuccessRate()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }, NullLoggerFactory.Instance);

            var mapper = mapperConfig.CreateMapper();

            var dbObject = new PrinterDetailDbObject
            {
                PrinterTotalHours = 10.5,
                PrinterTotalHoursMonth = 2.25,
                PrinterPrintsComplete = 8,
                PrinterPrintsNoComplete = 2
            };

            var result = mapper.Map<PrinterDetailObject>(dbObject);

            Assert.Equal("10 h 30 minutos", result.PrinterTotalHours);
            Assert.Equal("2 h 15 minutos", result.PrinterTotalHoursMonth);
            Assert.Equal(0.8f, result.PrinterSuccessRate);
        }

        [Fact]
        [Trait("Category", "Unitary")]
        public void PrinterDetailMapping_WhenNoPrints_ShouldReturnZeroSuccessRate()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }, NullLoggerFactory.Instance);

            var mapper = mapperConfig.CreateMapper();

            var dbObject = new PrinterDetailDbObject
            {
                PrinterPrintsComplete = 0,
                PrinterPrintsNoComplete = 0
            };

            var result = mapper.Map<PrinterDetailObject>(dbObject);

            Assert.Equal(0, result.PrinterSuccessRate);
        }
    }
}
