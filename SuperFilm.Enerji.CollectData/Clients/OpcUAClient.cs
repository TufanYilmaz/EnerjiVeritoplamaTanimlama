using Opc.Ua;
using SuperFilm.Enerji.CollectData.Clients.Interfaces;
using SuperFilm.Enerji.CollectData.Readers;
using SuperFilm.Enerji.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;

namespace SuperFilm.Enerji.CollectData.Clients
{
    internal sealed class OpcUAClient(
        IRepository<EnerjiDbContext> enerjiRepository,
        IQueryRepository<EnerjiDbContext> queryRepository,
        IOpcUAReader opcUAReader
        ) : IOpcUAClient
    {
        List<OpcNodes> nodes = new List<OpcNodes>();
        public async Task LoadNodes(CancellationToken cancellationToken)
        {
            nodes = await enerjiRepository.GetListAsync<OpcNodes>(false, cancellationToken);
        }
        public async Task<DataValueCollection> ReadNodes(CancellationToken cancellationToken)
        {
            return null;
        }

   
    }
}
