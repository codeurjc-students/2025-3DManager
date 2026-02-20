using _3DMANAGER_APP.BLL.Models.Printer;
using _3DMANAGER_APP.Server.Models;
using _3DMANAGER_APP.TEST.Fixture;
using System.Net;
using System.Net.Http.Json;

namespace _3DMANAGER_APP.TEST.E2ETest
{
    [Collection("Database")]
    public class PrintersApiE2ETests : IClassFixture<AuthenticatedClientFixture>
    {
        private readonly HttpClient _client;

        public PrintersApiE2ETests(AuthenticatedClientFixture authFixture)
        {
            _client = authFixture.Client;
        }

        [Fact]
        public async Task GetPrinters_ShouldReturnPrinterList()
        {
            var response = await _client.GetAsync("/api/v1/printers/GetPrinterDashboardList");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<Server.Models.CommonResponse<List<PrinterListObject>>>();
            Assert.NotNull(content);
            Assert.True(content.Data.Count > 0);
        }

        [Fact]
        public async Task UpdatePrinter_ShouldUpdatePrinterSuccessfully()
        {
            var request = new PrinterDetailRequest
            {
                GroupId = 1,
                PrinterId = 1,
                PrinterName = "Printer Updated",
                PrinterDescription = "Updated description",
                PrinterModel = "Updated model",
                PrinterStateId = 2
            };
            var response = await _client.PutAsJsonAsync("/api/v1/printers/UpdatePrinter", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<
                CommonResponse<bool>
            >();

            Assert.NotNull(content);
            Assert.True(content.Data);
        }

    }
}
