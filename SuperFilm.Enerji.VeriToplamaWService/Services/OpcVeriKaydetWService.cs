using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Opc.Ua;
using SuperFilm.Enerji.Entites;
using TanvirArjel.EFCore.GenericRepository;
using System.Reflection.Emit;

namespace SuperFilm.Enerji.VeriToplamaWService.Services
{
    public class OpcVeriKaydetWService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _configuration;

        public OpcVeriKaydetWService(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
        }

        public async Task Kaydet(List<SayacVeri> data, List<OpCodes> opcNodes)
        {
            List<SayacVeri> toAdd = new List<SayacVeri>();
            DateTime now = DateTime.Now;

            toAdd.Add(new SayacVeri()); // örnek veri

            using var scope = _serviceScopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IRepository<EnerjiDbContext>>();

            await repository.AddAsync(toAdd);
            await repository.SaveChangesAsync();
        }

        public async Task Kaydet()
        {
            List<SayacVeri> add = new List<SayacVeri>
            {
                new SayacVeri
                {
                    Kod = "DefaultKod",
                    OpcNodesId = 0,
                    Deger = 0,
                    CreateDate = DateTime.Now,
                    NormalizeDate = DateTime.Now,
                    Ay = DateTime.Now.ToString("MM"),
                    Gun = DateTime.Now.ToString("dd"),
                    Yil = DateTime.Now.ToString("yyyy"),
                    Zaman = DateTime.Now.ToString("HHmmss")
                }
            };

            using var scope = _serviceScopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IRepository<EnerjiDbContext>>();

            foreach (var item in add)
            {
                await repository.AddAsync<SayacVeri>(item);
            }
            await repository.SaveChangesAsync();

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Kaydet();
                await Task.Delay(2000);
            }
        }
    }
}
