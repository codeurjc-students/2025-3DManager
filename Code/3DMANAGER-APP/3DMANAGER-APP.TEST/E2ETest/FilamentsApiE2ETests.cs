using _3DMANAGER_APP.BLL.Models.Filament;
using _3DMANAGER_APP.Server.Models;
using _3DMANAGER_APP.TEST.Fixture;
using System.Net;
using System.Net.Http.Json;

namespace _3DMANAGER_APP.TEST.E2ETest
{
    [Collection("Database")]
    public class FilamentsApiE2ETests : IClassFixture<AuthenticatedClientFixture>
    {
        private readonly HttpClient _client;

        public FilamentsApiE2ETests(AuthenticatedClientFixture authFixture)
        {
            _client = authFixture.Client;
        }

        [Fact]
        public async Task GetFilamentList_ShouldReturnFilamentsList()
        {
            var response = await _client.GetAsync("/api/v1/filaments/GetFilamentList");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<Server.Models.CommonResponse<List<FilamentListResponse>>>();
            Assert.NotNull(content);
            Assert.NotNull(content.Data);
            Assert.True(content.Data.Count > 0);
        }

        [Fact]
        public async Task GetFilamentDetail_ShouldReturnFilament()
        {
            var response = await _client.GetAsync("/api/v1/filaments/GetFilamentDetail?filamentId=1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<CommonResponse<FilamentDetailObject>>();

            Assert.NotNull(content);
            Assert.NotNull(content.Data);
            Assert.True(content.Data.FilamentId > 0);
        }

        [Fact]
        public async Task UpdateFilament_ShouldReturnSuccess()
        {
            var detailResponse = await _client.GetAsync("/api/v1/filaments/GetFilamentDetail?filamentId=1");

            Assert.Equal(HttpStatusCode.OK, detailResponse.StatusCode);

            var detail = await detailResponse.Content.ReadFromJsonAsync<
                CommonResponse<FilamentDetailObject>
            >();

            Assert.NotNull(detail);
            Assert.NotNull(detail.Data);

            var request = new FilamentUpdateRequest
            {
                GroupId = 1,
                FilamentId = detail.Data.FilamentId,
                FilamentName = "E2E Filament Updated",
                FilamentColor = "#FFFFFF",
                FilamentCost = 25,
                FilamentDescription = "E2E Filament description updated",
                FilamentLenght = 100,
                FilamentTemperature = 220,
                ImageFile = null,

            };

            var updateResponse = await _client.PutAsJsonAsync("/api/v1/filaments/UpdateFilament", request);

            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

            var updateContent = await updateResponse.Content.ReadFromJsonAsync<CommonResponse<bool>>();

            Assert.NotNull(updateContent);
            Assert.True(updateContent.Data);
        }

        [Fact]
        public async Task DeleteFilament_ShouldReturnSuccess()
        {

            var filamentId = 1;

            var response = await _client.DeleteAsync($"/api/v1/filaments/DeleteFilament?filamentId={filamentId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<CommonResponse<bool>>();

            Assert.NotNull(content);
            Assert.True(content.Data);
            Assert.Null(content.Error);
        }

        [Fact]
        public async Task DeleteFilament_ShouldReturnServerError_WhenFilamentDoesNotExist()
        {
            var invalidfilamentId = -1;
            var response = await _client.DeleteAsync($"/api/v1/filaments/DeleteFilament?filamentId={invalidfilamentId}");

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<CommonResponse<bool>>();

            Assert.NotNull(content);
            Assert.False(content.Data);
            Assert.NotNull(content.Error);
            Assert.Equal(500, content.Error.Code);
        }


    }
}
