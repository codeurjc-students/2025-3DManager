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
            Assert.NotNull(content.Data);
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

        //[Fact]
        //public async Task DeletePrinter_ShouldReturnSuccess()
        //{

        //    var printerId = 1;

        //    var response = await _client.DeleteAsync($"/api/v1/printers/DeletePrinter?printerId={printerId}");

        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        //    var content = await response.Content.ReadFromJsonAsync<CommonResponse<bool>>();

        //    Assert.NotNull(content);
        //    Assert.True(content.Data);
        //    Assert.Null(content.Error);
        //}

        [Fact]
        public async Task DeletePrinter_ShouldReturnServerError_WhenPrinterDoesNotExist()
        {
            var invalidPrinterId = -1;
            var response = await _client.DeleteAsync($"/api/v1/printers/DeletePrinter?printerId={invalidPrinterId}");

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<CommonResponse<bool>>();

            Assert.NotNull(content);
            Assert.False(content.Data);
            Assert.NotNull(content.Error);
            Assert.Equal(500, content.Error.Code);
        }


    }
}
