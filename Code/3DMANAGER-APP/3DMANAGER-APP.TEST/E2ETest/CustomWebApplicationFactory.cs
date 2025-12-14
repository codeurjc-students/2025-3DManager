using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.TEST.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace _3DMANAGER_APP.TEST
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {

                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                bool isCI = string.Equals(environment, "CI", StringComparison.OrdinalIgnoreCase);

                if (isCI)
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(IPrinterDbManager));

                    if (descriptor != null)
                        services.Remove(descriptor);


                    services.AddSingleton<IPrinterDbManager, FakePrinterDbManager>();
                }
            });
        }
    }
}
