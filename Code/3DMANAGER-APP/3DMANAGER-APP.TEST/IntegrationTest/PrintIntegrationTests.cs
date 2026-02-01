using _3DMANAGER_APP.BLL.Managers;
using _3DMANAGER_APP.BLL.Mapper;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Print;
using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Managers;
using _3DMANAGER_APP.TEST.Fixture;
using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

namespace _3DMANAGER_APP.TEST.IntegrationTest
{
    [Collection("Database")]
    public class PrintIntegrationTests
    {
        private readonly DatabaseFixture _fixture;
        private readonly IMapper _mapper;

        public PrintIntegrationTests(DatabaseFixture fixture)
        {
            _fixture = fixture;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }, NullLoggerFactory.Instance);

            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Print_ShouldReturnPrintsAndCreate()
        {
            var dataSource = new MySQLDataSource(
                _fixture.ConnectionString,
                "3DMANAGER"
            );

            var printDbManager = new PrintDbManager(
                dataSource,
                NullLogger<PrintDbManager>.Instance
            );

            var manager = new PrintManager(
                printDbManager,
                _mapper,
                NullLogger<PrintManager>.Instance
            );

            BaseError error;
            PagedRequest pagedRequest = new PagedRequest { PageNumber = 1, PageSize = 10 };
            var prints = manager.GetPrintList(1, pagedRequest, out error);

            Assert.Null(error);
            Assert.NotNull(prints.prints);
            Assert.NotEmpty(prints.prints);

            PrintRequest newPrint = new PrintRequest()
            {
                GroupId = 1,
                PrintName = "Test",
                PrintDescription = "Test",
                PrintFilament = 1,
                PrintFilamentUsed = 20,
                PrintPrinter = 1,
                PrintRealTime = 10,
                PrintTime = 10,
                PrintState = 1,
                UserId = 1
            };
            var printPost = manager.PostPrint(newPrint, out error);
            Assert.Null(error);
            Assert.True(printPost);

            var printsAfterPost = manager.GetPrintList(1, pagedRequest, out error);
            Assert.Null(error);
            Assert.Equal(prints.prints.Count + 1, printsAfterPost.prints.Count);
            Assert.NotNull(printsAfterPost);
            Assert.NotEmpty(printsAfterPost.prints);

        }
    }
}
