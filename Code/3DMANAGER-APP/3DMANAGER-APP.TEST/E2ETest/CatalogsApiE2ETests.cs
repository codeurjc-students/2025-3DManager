using _3DMANAGER_APP.BLL.Models.Catalog;
using _3DMANAGER_APP.Server.Models;
using System.Net;
using System.Net.Http.Json;

namespace _3DMANAGER_APP.TEST.E2ETest
{
    public class CatalogsApiE2ETests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public CatalogsApiE2ETests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetFilamentsTypes_ShouldReturnCatalogFilamentTypesUsersList()
        {
            var response = await _client.GetAsync("/api/v1/catalogs/GetFilamentType");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<CommonResponse<List<CatalogResponse>>>();
            Assert.NotNull(content);
            Assert.True(content.Data.Count > 0);
        }
        [Fact]
        public async Task GetPrintStates_ShouldReturnCatalogPrintStatesList()
        {
            var response = await _client.GetAsync("/api/v1/catalogs/GetPrintState");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<CommonResponse<List<CatalogResponse>>>();
            Assert.NotNull(content);
            Assert.True(content.Data.Count > 0);
        }
        [Fact]
        public async Task GetFilamentsCatalog_ShouldReturnCatalogFilamentsList()
        {
            var response = await _client.GetAsync("/api/v1/catalogs/GetFilamentCatalog?groupId=4");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<CommonResponse<List<CatalogResponse>>>();
            Assert.NotNull(content);
            Assert.True(content.Data.Count > 0);
        }
        [Fact]
        public async Task GetPrintersCatalog_ShouldReturnCatalogPrintersList()
        {
            var response = await _client.GetAsync("/api/v1/catalogs/GetPrinterCatalog?groupId=4");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<CommonResponse<List<CatalogResponse>>>();
            Assert.NotNull(content);
            Assert.True(content.Data.Count > 0);
        }
    }
}
