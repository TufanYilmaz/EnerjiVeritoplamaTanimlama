using SuperFilm.Enerji.CollectData.Readers;
using SuperFilm.Enerji.Entites;
using System.Configuration;
using TanvirArjel.EFCore.GenericRepository;

namespace SuperFilm.Enerji.CollectData
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configration;
        private readonly IRepository<EnerjiDbContext> _enerjiRepository;

        public Worker(ILogger<Worker> logger,
            IConfiguration configuration,
            IRepository<EnerjiDbContext> enerjiRepository)
        {
            _logger = logger;
            _configration = configuration;
            _enerjiRepository = enerjiRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            OpcUAReader reader = new OpcUAReader(_configration);
            var OpcNodes = await _enerjiRepository.GetListAsync<OpcNodes>(stoppingToken);
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
