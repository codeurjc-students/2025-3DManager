using _3DMANAGER_APP.TestingSupport.Database;
using Microsoft.Extensions.Configuration;


namespace _3DMANAGER_APP.TEST.Fixture
{
    public class DatabaseFixture : IAsyncLifetime
    {
        public string ConnectionString { get; }
        private readonly DatabaseSeeder _seeder;
        public DatabaseFixture()
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .AddUserSecrets<DatabaseFixture>()
            .Build();

            var cs = config.GetConnectionString("TestConnection")
                ?? throw new InvalidOperationException("ConnectionString para test no definida");
            if (!cs.Contains("3dmanagerCI"))
            {
                throw new InvalidOperationException(
                    "DatabaseFixture solo puede usarse contra la BBDD de CI/Test"
                );
            }
            _seeder = new DatabaseSeeder(cs);
            ConnectionString = cs;
        }

        public async Task InitializeAsync()
        {
            await _seeder.SeedAsync();
        }

        public async Task DisposeAsync()
        {
            await _seeder.CleanupAsync();
        }
    }
}
