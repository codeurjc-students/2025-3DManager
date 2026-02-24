using _3DMANAGER_APP.BLL.Models.User;
using _3DMANAGER_APP.Server.Models;
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

        [Fact]
        public async Task UpdateUser_ShouldReturnSuccess()
        {
            var detailResponse = await _client.GetAsync("/api/v1/users/GetUserDetail?userId=1");

            Assert.Equal(HttpStatusCode.OK, detailResponse.StatusCode);

            var detail = await detailResponse.Content.ReadFromJsonAsync<CommonResponse<UserDetailObject>>();

            Assert.NotNull(detail);
            Assert.NotNull(detail.Data);

            var request = new UserUpdateRequest
            {
                GroupId = 1,
                UserId = detail.Data.userId,
                UserName = "E2E User Updated",
                UserEmail = "e2e@test.com",
            };

            var updateResponse = await _client.PutAsJsonAsync("/api/v1/users/UpdateUser", request);

            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

            var updateContent = await updateResponse.Content.ReadFromJsonAsync<CommonResponse<bool>>();

            Assert.NotNull(updateContent);
            Assert.True(updateContent.Data);
        }

        [Fact]
        public async Task GetUserDetail_ShouldReturnUser()
        {
            var response = await _client.GetAsync("/api/v1/users/GetUserDetail?userId=1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<
                CommonResponse<UserDetailObject>
            >();

            Assert.NotNull(content);
            Assert.NotNull(content.Data);
            Assert.True(content.Data.userId > 0);
        }


    }
}
