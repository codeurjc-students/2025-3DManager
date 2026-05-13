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
            var response = await _client.GetAsync("/api/v1/users");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<Server.Models.CommonResponse<List<UserListResponse>>>();
            Assert.NotNull(content);
            Assert.True(content.Data.Count > 0);
        }

        [Fact]
        public async Task CreateUser_ShouldReturnSuccess()
        {
            var form = new MultipartFormDataContent
            {
                { new StringContent("newuser@test.com"), "UserEmail" },
                { new StringContent("New User"), "UserName" },
                { new StringContent("123456"), "UserPassword" }
            };

            var response = await _client.PostAsync("/api/v1/users", form);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<CommonResponse<int>>();

            Assert.NotNull(content);
            Assert.True(content.Data > 0);
        }


        [Fact]
        public async Task UpdateUser_ShouldReturnSuccess()
        {
            var detailResponse = await _client.GetAsync("/api/v1/users/1");

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

            var updateResponse = await _client.PutAsJsonAsync("/api/v1/users/1", request);

            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

            var updateContent = await updateResponse.Content.ReadFromJsonAsync<CommonResponse<bool>>();

            Assert.NotNull(updateContent);
            Assert.True(updateContent.Data);
        }

        [Fact]
        public async Task GetUserDetail_ShouldReturnUser()
        {
            var response = await _client.GetAsync("/api/v1/users/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<
                CommonResponse<UserDetailObject>
            >();

            Assert.NotNull(content);
            Assert.NotNull(content.Data);
            Assert.True(content.Data.userId > 0);
        }

        [Fact]
        public async Task GetUserDetail_ShouldReturnServerError_WhenUserDoesNotExist()
        {
            var response = await _client.GetAsync("/api/v1/users/-1");

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task UpdateUserImage_ShouldFail()
        {
            var image = new ByteArrayContent(new byte[] { 1, 2, 3 });
            image.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");

            var form = new MultipartFormDataContent
            {
                { image, "imageFile", "test.png" }
            };

            var response = await _client.PostAsync("/api/v1/users/1/image", form);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task DeleteUserImage_ShouldReturnSuccess()
        {
            var response = await _client.DeleteAsync("/api/v1/users/1/image");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<CommonResponse<bool>>();

            Assert.NotNull(content);
            Assert.True(content.Data);
        }
    }
}
