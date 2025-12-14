using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.TEST.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
                    services.RemoveAll(typeof(IPrinterDbManager));
                    services.RemoveAll(typeof(IFilamentDbManager));
                    services.RemoveAll(typeof(IPrintDbManager));
                    services.RemoveAll(typeof(IUserDbManager));
                    services.RemoveAll(typeof(ICatalogDbManager));

                    services.AddSingleton<IPrinterDbManager, FakePrinterDbManager>();
                    services.AddSingleton<IFilamentDbManager, FakeFilamentDbManager>();
                    services.AddSingleton<IPrintDbManager, FakePrintDbManager>();
                    services.AddSingleton<IUserDbManager, FakeUserDbManager>();
                    services.AddSingleton<ICatalogDbManager, FakeCatalogDbManager>();
                }
            });
        }
    }
}
