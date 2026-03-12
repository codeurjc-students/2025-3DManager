using _3DMANAGER_APP.BLL.Models.Group;
using _3DMANAGER_APP.Server.Models;
using _3DMANAGER_APP.TEST.Fixture;
using System.Net;
using System.Net.Http.Json;

namespace _3DMANAGER_APP.TEST.E2ETest
{
    [Collection("Database")]
    public class GroupApiE2ETests : IClassFixture<AuthenticatedClientFixture>
    {
        private readonly HttpClient _client;

        public GroupApiE2ETests(AuthenticatedClientFixture authFixture)
        {
            _client = authFixture.Client;
        }

        [Fact]
        public async Task GetGroupDashboardData_ShouldReturnSuccess()
        {

            var response = await _client.GetAsync("/api/v1/groups/GetGroupDashboardData");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<CommonResponse<GroupDashboardData>>();

            Assert.NotNull(content);
            Assert.NotNull(content.Data);
            Assert.Null(content.Error);
        }

    }
}
