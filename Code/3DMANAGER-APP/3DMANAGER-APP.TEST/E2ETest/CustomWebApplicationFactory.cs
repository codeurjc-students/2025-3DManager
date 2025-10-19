using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models;
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
                // Detectar si estamos en CI (GitHub Actions u otro entorno sin BBDD)
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                bool isCI = string.Equals(environment, "CI", StringComparison.OrdinalIgnoreCase);

                if (isCI)
                {
                    // 🔹 Buscar y eliminar la implementación real del DAL
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(IPrinterDbManager));

                    if (descriptor != null)
                        services.Remove(descriptor);

                    // 🔹 Registrar una fake implementation que no use MySQL
                    services.AddSingleton<IPrinterDbManager, FakePrinterDbManager>();
                }
            });
        }
    }

    /// <summary>
    /// Fake del acceso a datos para CI: simula la respuesta de la BBDD
    /// </summary>
    public class FakePrinterDbManager : IPrinterDbManager
    {
        public List<PrinterDbObject> GetPrinterList(out ErrorDbObject error)
        {
            error = null;
            return new List<PrinterDbObject>
            {
                new PrinterDbObject { PrinterName = "HP" },
                new PrinterDbObject { PrinterName = "Epson" },
                new PrinterDbObject { PrinterName = "Canon" }
            };
        }
    }
}
