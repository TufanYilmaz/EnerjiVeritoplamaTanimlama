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
        IQueryRepository<EnerjiDbContext> queryRepository
        )
    {

    }
}
