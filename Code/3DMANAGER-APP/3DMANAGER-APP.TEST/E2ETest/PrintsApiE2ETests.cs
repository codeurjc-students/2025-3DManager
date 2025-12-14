using _3DMANAGER_APP.BLL.Models.Print;
using _3DMANAGER_APP.Server.Models;
using System.Net;
using System.Net.Http.Json;

namespace _3DMANAGER_APP.TEST.E2ETest
{
    public class PrintsApiE2ETests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PrintsApiE2ETests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetPrintList_ShouldReturnPrintsList()
        {
            var response = await _client.GetAsync("/api/Print/GetPrintList?groupId=4");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<CommonResponse<List<PrintListResponse>>>();
            Assert.NotNull(content);
            Assert.True(content.Data.Count > 0);
        }
    }
}
