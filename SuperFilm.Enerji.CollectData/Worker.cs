using SuperFilm.Enerji.CollectData.Readers;
using System.Configuration;

namespace SuperFilm.Enerji.CollectData
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configration;

        public Worker(ILogger<Worker> logger,IConfiguration configuration)
        {
            _logger = logger;
            _configration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            OpcUAReader reader = new OpcUAReader(_configration);
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
