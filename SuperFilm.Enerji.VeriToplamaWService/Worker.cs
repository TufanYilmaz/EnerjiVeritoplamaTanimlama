using SuperFilm.Enerji.Entites;
using TanvirArjel.EFCore.GenericRepository;

namespace SuperFilm.Enerji.VeriToplamaWService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger,
                      IServiceScopeFactory serviceScopeFactory,
                      IConfiguration configuration)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var queryRepository = scope.ServiceProvider.GetRequiredService<IQueryRepository<EnerjiDbContext>>();
                    var enerjiRepository = scope.ServiceProvider.GetRequiredService<IRepository<EnerjiDbContext>>();

                    
                    var entities = await queryRepository.GetListAsync<OpcNodes>(cancellationToken: stoppingToken);

                    
                    _logger.LogInformation("{count} veri {time} zamanýnda getirildi", entities.Count, DateTimeOffset.Now);
                    _logger.LogInformation("Veriler baþarýyla kaydedildi at {time}", DateTimeOffset.Now);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
