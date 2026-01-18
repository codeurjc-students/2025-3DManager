using _3DMANAGER_APP.BLL.Models.User;
using System.Net.Http.Json;

namespace _3DMANAGER_APP.TEST.E2ETest
{
    public static class E2EAuthHelper
    {
        public static async Task<string> LoginAndGetTokenAsync(HttpClient client)
        {
            LoginRequest loginRequest = new LoginRequest()
            {
                UserName = "user_test",
                UserPassword = "password123"
            };

            var response = await client.PostAsJsonAsync("/api/v1/users/login", loginRequest);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<Server.Models.CommonResponse<LoginResponse>>();

            return result!.Data!.Token;
        }
    }
}
