using _3DMANAGER_APP.BLL.Models.Filament;
using _3DMANAGER_APP.Server.Models;
using System.Net;
using System.Net.Http.Json;

namespace _3DMANAGER_APP.TEST.E2ETest
{
    public class FilamentsApiE2ETests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public FilamentsApiE2ETests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetFilamentList_ShouldReturnFilamentsList()
        {
            var response = await _client.GetAsync("/api/v1/Filament/GetFilamentList?groupId=4");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<CommonResponse<List<FilamentListResponse>>>();
            Assert.NotNull(content);
            Assert.True(content.Data.Count > 0);
        }
    }
}
