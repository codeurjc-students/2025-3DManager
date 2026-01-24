using _3DMANAGER_APP.TEST.E2ETest;
using System.Net.Http.Headers;

namespace _3DMANAGER_APP.TEST.Fixture
{
    public class AuthenticatedClientFixture : IAsyncLifetime
    {
        public HttpClient Client { get; private set; } = default!;
        public string Token { get; private set; } = string.Empty;

        private CustomWebApplicationFactory<Program> _factory = default!;

        public async Task InitializeAsync()
        {
            _factory = new CustomWebApplicationFactory<Program>();
            Client = _factory.CreateClient();

            Token = await E2EAuthHelper.LoginAndGetTokenAsync(Client);

            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", Token);
        }

        public Task DisposeAsync()
        {
            Client.Dispose();
            _factory.Dispose();
            return Task.CompletedTask;
        }
    }
}
