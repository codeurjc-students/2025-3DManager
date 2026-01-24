using _3DMANAGER_APP.BLL.Models.User;
using _3DMANAGER_APP.TEST.Fixture;
using System.Net;
using System.Net.Http.Json;

namespace _3DMANAGER_APP.TEST.E2ETest
{
    [Collection("Database")]
    public class UsersApiE2ETests : IClassFixture<AuthenticatedClientFixture>
    {
        private readonly HttpClient _client;

        public UsersApiE2ETests(AuthenticatedClientFixture authFixture)
        {
            _client = authFixture.Client;
        }

        [Fact]
        public async Task GetUserList_ShouldReturnUsersList()
        {
            var response = await _client.GetAsync("/api/v1/users/GetUserList");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<Server.Models.CommonResponse<List<UserListResponse>>>();
            Assert.NotNull(content);
            Assert.True(content.Data.Count > 0);
        }
    }
}
