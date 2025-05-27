using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Opc.Ua;
using SuperFilm.Enerji.Entites;
using TanvirArjel.EFCore.GenericRepository;
using System.Reflection.Emit;
using SuperFilm.Enerji.OpcUAReader;
using System.Threading;

namespace SuperFilm.Enerji.VeriToplamaWService.Services
{
    public class OpcVeriKaydetWService : BackgroundService
    {
        private readonly ILogger<OpcVeriKaydetWService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _configuration;
        private readonly UAReaderClient _client;
        public OpcVeriKaydetWService(ILogger<OpcVeriKaydetWService> logger,
            IServiceScopeFactory serviceScopeFactory, 
            IConfiguration configuration)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
            _logger = logger;
            _client = new UAReaderClient(configuration.GetValue<string>("opcUrl") ?? "");
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


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                List<OpcNodes> opcNodes = await GetOpcNodesAsync();
                var data = await VeriCekAsync(opcNodes, stoppingToken);
                await Kaydet(data, opcNodes);
                await Task.Delay(120_000);
            }
        }
        public async Task<List<OpcNodes>> GetOpcNodesAsync()
        {
            List<OpcNodes> result = new List<OpcNodes>();
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var repository = scope.ServiceProvider.GetService<IQueryRepository<EnerjiDbContext>>()!;
                result = await repository.GetListAsync<OpcNodes>(asNoTracking: true);
            }
            return result;
        }
        public async Task<List<SayacVeri>> VeriCekAsync(List<OpcNodes> opcNodes, CancellationToken cancellationToken)
        {
            _logger.LogInformation("VeriCekAsync Start");
            DataValueCollection readResult = new DataValueCollection();
            List<SayacVeri> result = new List<SayacVeri>();
            int i = 0;
            while (true)
            {
                if (_client.m_session != null)
                {

                    _logger.LogInformation("{i} denemesinde girdi",i);
                    ReadValueIdCollection readValues = new ReadValueIdCollection();
                    foreach (var node in opcNodes)
                    {
                        readValues.Add(new ReadValueId() { AttributeId = (uint)node.AttributeId, NodeId = new NodeId(node.NodeId, (ushort)node.NodeNameSpace) });
                    }
                    readResult = await _client.ReadNodesAsync(readValues, cancellationToken);
                    foreach (var item in readResult)
                    {
                        result.Add(new SayacVeri()
                        {
                            Deger = Math.Round(Convert.ToDecimal(item.Value), 2),
                        });
                    }
                    _logger.LogInformation("{count} adet veri çekildi", result.Count);

                    break;
                }
                i++;
                await Task.Delay(1000);
            }
            _logger.LogInformation("VeriCekAsync End");
            return result;
        }
        public async Task Kaydet(List<SayacVeri> data, List<OpcNodes> opcNodes)
        {
            _logger.LogInformation("Kaydet start");

            List<SayacVeri> toAdd = new List<SayacVeri>();
            DateTime now = DateTime.Now;
            for (int i = 0; i < opcNodes.Count; i++)
            {
                toAdd.Add(new SayacVeri()
                {
                    Ay = now.ToString("MM"),
                    CreateDate = now,
                    Deger = data[i].Deger,
                    Gun = now.ToString("dd"),
                    Kod = opcNodes[i].Code,
                    NormalizeDate = now,
                    OpcNodesId = opcNodes[i].Id,
                    Yil = now.ToString("yyyy"),
                    Zaman = now.ToString("hhmmss"),
                });
            }
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var repository = scope.ServiceProvider.GetService<IRepository<EnerjiDbContext>>()!;
                await repository.AddAsync<SayacVeri>(toAdd);
                await repository.SaveChangesAsync();
            }
            _logger.LogInformation("Kaydet end");

        }
    }
}
