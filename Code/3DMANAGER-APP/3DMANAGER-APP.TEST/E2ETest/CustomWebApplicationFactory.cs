using _3DMANAGER_APP.DAL.Base;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;

namespace _3DMANAGER_APP.TEST.E2ETest
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Testing.json", optional: true)
                .AddUserSecrets<Program>(optional: true) // <--- aquí se agregan los secretos
                .Build();

            var connectionString = config.GetConnectionString("TestConnection");

            builder.ConfigureServices(services =>
            {
                // Reemplazar la cadena de conexión de la DAL con la de testing
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IDataSource<MySqlConnection>));

                if (descriptor != null) services.Remove(descriptor);

                services.AddSingleton<IDataSource<MySqlConnection>>(new MySQLDataSource(connectionString, "3DMANAGER"));
            });

            builder.UseEnvironment("Testing"); // asegura que estamos en el entorno de test

            return base.CreateHost(builder);
        }
    }
}
