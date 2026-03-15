using _3DMANAGER_APP.BLL.Models.Print;
using _3DMANAGER_APP.Server.Models;
using _3DMANAGER_APP.TEST.Fixture;
using System.Net;
using System.Net.Http.Json;

namespace _3DMANAGER_APP.TEST.E2ETest
{
    [Collection("Database")]
    public class PrintsApiE2ETests : IClassFixture<AuthenticatedClientFixture>
    {
        private readonly HttpClient _client;

        public PrintsApiE2ETests(AuthenticatedClientFixture authFixture)
        {
            _client = authFixture.Client;
        }

        [Fact]
        public async Task GetPrintList_ShouldReturnPrintsList()
        {
            var response = await _client.GetAsync("/api/v1/prints/GetPrintList");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<Server.Models.CommonResponse<PrintListResponse>>();
            Assert.NotNull(content);
            Assert.NotNull(content.Data);
            Assert.True(content.Data.prints.Count > 0);
        }

        [Fact]
        public async Task GetPrintDetail_ShouldReturnPrint()
        {
            var response = await _client.GetAsync("/api/v1/prints/GetPrintDetail?printId=1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<CommonResponse<PrintDetailObject>>();

            Assert.NotNull(content);
            Assert.NotNull(content.Data);
            Assert.True(content.Data.PrintId > 0);
        }

        [Fact]
        public async Task UpdatePrinter_ShouldReturnSuccess()
        {
            var detailResponse = await _client.GetAsync("/api/v1/prints/GetPrintDetail?printId=1");

            Assert.Equal(HttpStatusCode.OK, detailResponse.StatusCode);

            var detail = await detailResponse.Content.ReadFromJsonAsync<CommonResponse<PrintDetailObject>>();

            Assert.NotNull(detail);
            Assert.NotNull(detail.Data);

            var request = new PrintDetailRequest
            {
                GroupId = 1,
                PrintId = 1,
                PrintName = "Updated print",
                PrintDescription = "Updated description"
            };

            var updateResponse = await _client.PutAsJsonAsync("/api/v1/prints/UpdatePrint", request);

            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

            var updateContent = await updateResponse.Content.ReadFromJsonAsync<CommonResponse<bool>>();

            Assert.NotNull(updateContent);
            Assert.True(updateContent.Data);
        }

        [Fact]
        public async Task DeletePrint_ShouldReturnSuccess()
        {

            var printId = 1;

            var response = await _client.DeleteAsync($"/api/v1/prints/DeletePrint?printId={printId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<CommonResponse<bool>>();

            Assert.NotNull(content);
            Assert.True(content.Data);
            Assert.Null(content.Error);
        }

        [Fact]
        public async Task DeletePrint_ShouldReturnServerError_WhenPrintDoesNotExist()
        {
            var invalidPrintId = -1;
            var response = await _client.DeleteAsync($"/api/v1/prints/DeletePrint?printId={invalidPrintId}");

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<CommonResponse<bool>>();

            Assert.NotNull(content);
            Assert.False(content.Data);
            Assert.NotNull(content.Error);
            Assert.Equal(500, content.Error.Code);
        }

    }
}
