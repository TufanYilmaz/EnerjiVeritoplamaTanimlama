using Opc.Ua;
using SuperFilm.Enerji.Entites;
using SuperFilm.Enerji.OleDbReader;
using SuperFilm.Enerji.OpcUAReader;
using System.Collections.Generic;
using TanvirArjel.EFCore.GenericRepository;

namespace Superfilm.Enerji.VeriToplama.Services
{
    public class OpcVeriKaydet(IQueryRepository<EnerjiDbContext> queryRepository,
            IRepository<EnerjiDbContext> enerjiRepository,
        IConfiguration configuration,
        //IUAReaderClient client,
        IServiceScopeFactory _serviceScopeFactory) : IOpcVeriKaydet
    {
        UAReaderClient client = new UAReaderClient(configuration.GetValue<string>("opcUrl") ?? "");
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

        public async Task Kaydet(List<SayacVeri> data, List<OpcNodes> opcNodes)
        {
            List<SayacVeri> toAdd = new List<SayacVeri>();
            DateTime now = DateTime.Now;
            for (int i = 0; i < opcNodes.Count; i++)
            {
                toAdd.Add(new SayacVeri()
                {
                    Ay=now.ToString("MM"),
                    CreateDate=now,
                    Deger = data[i].Deger,
                    Gun=now.ToString("dd"),
                    Kod = opcNodes[i].Code,
                    NormalizeDate=now,
                    OpcNodesId = opcNodes[i].Id,
                    Yil=now.ToString("yyyy"),
                    Zaman=now.ToString("hhmmss"),
                });
            }
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var repository = scope.ServiceProvider.GetService<IRepository<EnerjiDbContext>>()!;
                await repository.AddAsync<SayacVeri>(toAdd);
                await repository.SaveChangesAsync();
                //if (client.m_session != null)
                //{
                //    await client.m_session.CloseAsync(); 
                //}
            }
        }

        public async Task Start(CancellationToken cancellationToken)
        {
            //client.CreateSession();
            List<OpcNodes> opcNodes = await GetOpcNodesAsync();
            var data = await VeriCekAsync(opcNodes, cancellationToken);
            await Kaydet(data, opcNodes);
        }

        public async Task<List<SayacVeri>> VeriCekAsync(List<OpcNodes> opcNodes,CancellationToken cancellationToken)
        {
            DataValueCollection readResult=new DataValueCollection();
            List<SayacVeri> result = new List<SayacVeri>();
            while (true)
            {
                if (client.m_session != null)
                {
                    ReadValueIdCollection readValues = new ReadValueIdCollection();
                    foreach (var node in opcNodes)
                    {
                        readValues.Add(new ReadValueId() { AttributeId = (uint)node.AttributeId, NodeId = new NodeId(node.NodeId, (ushort)node.NodeNameSpace) });
                    }
                    readResult = await client.ReadNodesAsync(readValues, cancellationToken);
                    foreach (var item in readResult)
                    {
                        result.Add(new SayacVeri()
                        {
                            Deger = Math.Round(Convert.ToDecimal(item.Value), 2),
                        });
                    }
                    await client.m_session.CloseAsync();
                    break;
                }
            } 
            return result;
        }
    }
}
