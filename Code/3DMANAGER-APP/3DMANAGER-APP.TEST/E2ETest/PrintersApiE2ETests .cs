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
            var response = await _client.GetAsync("/api/v1/printers/dashboard");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<Server.Models.CommonResponse<List<PrinterListObject>>>();
            Assert.NotNull(content);
            Assert.NotNull(content.Data);
            Assert.True(content.Data.Count > 0);
        }

        [Fact]
        public async Task GetPrinterList_ShouldReturnBadRequest_WhenServiceReturnsError()
        {
            var response = await _client.GetAsync("/api/v1/printers?forceError=true");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreatePrinter_ShouldReturnSuccess()
        {
            var form = new MultipartFormDataContent
            {
                { new StringContent("Test Printer"), "PrinterName" },
                { new StringContent("Test Model"), "PrinterModel" },
                { new StringContent("Test Description"), "PrinterDescription" },
                { new StringContent("1"), "PrinterStateId" }
            };

            var response = await _client.PostAsync("/api/v1/printers", form);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<CommonResponse<int>>();

            Assert.NotNull(content);
            Assert.True(content.Data > 0);
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
            var response = await _client.PutAsJsonAsync("/api/v1/printers/1", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<
                CommonResponse<bool>
            >();

            Assert.NotNull(content);
            Assert.True(content.Data);
        }


        [Fact]
        public async Task GetPrinterDetail_ShouldReturnSuccess()
        {
            var response = await _client.GetAsync("/api/v1/printers/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<CommonResponse<PrinterDetailObject>>();

            Assert.NotNull(content);
            Assert.NotNull(content.Data);
        }

        [Fact]
        public async Task GetPrinterDetail_ShouldReturnServerError_WhenPrinterDoesNotExist()
        {
            var response = await _client.GetAsync("/api/v1/printers/-1");

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task DeletePrinter_ShouldReturnSuccess()
        {

            var printerId = 4;

            var response = await _client.DeleteAsync($"/api/v1/printers/{printerId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<CommonResponse<bool>>();

            Assert.NotNull(content);
            Assert.True(content.Data);
            Assert.Null(content.Error);
        }

        [Fact]
        public async Task DeletePrinter_ShouldReturnServerError_WhenPrinterDoesNotExist()
        {
            var invalidPrinterId = -1;
            var response = await _client.DeleteAsync($"/api/v1/printers/{invalidPrinterId}");

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<CommonResponse<bool>>();

            Assert.NotNull(content);
            Assert.False(content.Data);
            Assert.NotNull(content.Error);
            Assert.Equal(500, content.Error.Code);
        }


    }
}
