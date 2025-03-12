using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperFilm.Enerji.CollectData.Readers
{
    public interface IOpcUAReader
    {
        void CreateSession();
        Task<DataValueCollection> ReadNodesAsync(ReadValueIdCollection nodesToRead, CancellationToken cancellationToken);
    }
}
