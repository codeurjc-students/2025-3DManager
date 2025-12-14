using _3DMANAGER_APP.BLL.Models.Printer;
using _3DMANAGER_APP.Server.Models;
using System.Net;
using System.Net.Http.Json;

namespace _3DMANAGER_APP.TEST.E2ETest
{
    public class PrintersApiE2ETests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PrintersApiE2ETests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetPrinters_ShouldReturnPrinterList()
        {
            var response = await _client.GetAsync("/api/Printer/GetPrinterDashboardList?groupId=4");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<CommonResponse<List<PrinterListObject>>>();
            Assert.NotNull(content);
            Assert.True(content.Data.Count > 0);
        }
    }
}
