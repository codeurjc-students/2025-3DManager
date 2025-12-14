using _3DMANAGER_APP.BLL.Models.User;
using _3DMANAGER_APP.Server.Models;
using System.Net;
using System.Net.Http.Json;

namespace _3DMANAGER_APP.TEST.E2ETest
{
    public class UsersApiE2ETests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public UsersApiE2ETests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetUserList_ShouldReturnUsersList()
        {
            var response = await _client.GetAsync("/api/v1/users/GetUserList?groupId=4");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<CommonResponse<List<UserListResponse>>>();
            Assert.NotNull(content);
            Assert.True(content.Data.Count > 0);
        }
    }
}
