using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.DAL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace _3DMANAGER_APP.BLL.Managers
{
    public class DailyTaskService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DailyTaskService> _logger;

        public DailyTaskService(IServiceProvider serviceProvider, ILogger<DailyTaskService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("DailyTaskService iniciado.");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.Now;
                    var nextRun = now.Date.AddHours(6);
                    //var nextRun = now.AddMinutes(2); //For test

                    if (now > nextRun)
                        nextRun = nextRun.AddDays(1);

                    var delay = nextRun - now;
                    string msg = $"Próxima ejecución automática programada a las {nextRun}.";
                    _logger.LogInformation(msg);

                    await Task.Delay(delay, stoppingToken);


                    using var scope = _serviceProvider.CreateScope();

                    var filamentService = scope.ServiceProvider.GetRequiredService<IFilamentManager>();
                    var dailyService = scope.ServiceProvider.GetRequiredService<IDailyTaskDbManager>();

                    _logger.LogInformation("Ejecutando tareas diarias...");

                    await filamentService.CheckFilamentLevelsAsync();
                    await dailyService.CleanOldDataAsync();

                    _logger.LogInformation("Tareas diarias completadas.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error durante la ejecución de las tareas diarias.");
                }
            }
        }
    }
}
