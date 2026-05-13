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
            var response = await _client.GetAsync("/api/v1/prints");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<Server.Models.CommonResponse<PrintListResponse>>();
            Assert.NotNull(content);
            Assert.NotNull(content.Data);
            Assert.True(content.Data.prints.Count > 0);
        }

        [Fact]
        public async Task GetPrintDetail_ShouldReturnPrint()
        {
            var response = await _client.GetAsync("/api/v1/prints/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<CommonResponse<PrintDetailObject>>();

            Assert.NotNull(content);
            Assert.NotNull(content.Data);
            Assert.True(content.Data.PrintId > 0);
        }

        [Fact]
        public async Task GetPrintDetail_ShouldReturnServerError_WhenPrintDoesNotExist()
        {
            var response = await _client.GetAsync("/api/v1/prints/-1");

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task CreatePrint_ShouldReturnSuccess()
        {
            var form = new MultipartFormDataContent
        {
            { new StringContent("Test Print"), "PrintName" },
            { new StringContent("Test Description"), "PrintDescription" },
            { new StringContent("1"), "PrinterId" },
            { new StringContent("1"), "FilamentId" }
        };

            var response = await _client.PostAsync("/api/v1/prints", form);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<CommonResponse<int>>();

            Assert.NotNull(content);
            Assert.True(content.Data > 0);
        }

        [Fact]
        public async Task UpdatePrinter_ShouldReturnSuccess()
        {
            var detailResponse = await _client.GetAsync("/api/v1/prints/1");

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

            var updateResponse = await _client.PutAsJsonAsync("/api/v1/prints/1", request);

            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

            var updateContent = await updateResponse.Content.ReadFromJsonAsync<CommonResponse<bool>>();

            Assert.NotNull(updateContent);
            Assert.True(updateContent.Data);
        }

        [Fact]
        public async Task PostPrintComment_ShouldReturnSuccess()
        {
            var request = new PrintCommentRequest
            {
                PrintId = 1,
                UserId = 1,
                Comment = "Test comment"
            };

            var response = await _client.PostAsJsonAsync("/api/v1/prints/1/comments", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<CommonResponse<int>>();

            Assert.NotNull(content);
            Assert.True(content.Data > 0);
        }
        [Fact]
        public async Task GetPrintComments_ShouldReturnSuccess()
        {
            var response = await _client.GetAsync("/api/v1/prints/1/comments");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<CommonResponse<List<PrintCommentObject>>>();

            Assert.NotNull(content);
            Assert.NotNull(content.Data);
        }

        [Fact]
        public async Task DeletePrint_ShouldReturnSuccess()
        {

            var printId = 1;

            var response = await _client.DeleteAsync($"/api/v1/prints/{printId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<CommonResponse<bool>>();

            Assert.NotNull(content);
            Assert.True(content.Data);
        }

        [Fact]
        public async Task DeletePrint_ShouldReturnServerError_WhenPrintDoesNotExist()
        {
            var invalidPrintId = -1;
            var response = await _client.DeleteAsync($"/api/v1/prints/{invalidPrintId}");

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<CommonResponse<bool>>();

            Assert.NotNull(content);
            Assert.False(content.Data);
            Assert.NotNull(content.Error);
            Assert.Equal(500, content.Error.Code);
        }

    }
}
