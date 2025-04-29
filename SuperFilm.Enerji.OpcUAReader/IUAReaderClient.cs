using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperFilm.Enerji.OpcUAReader
{
    public interface IUAReaderClient
    {
        void CreateSession();
        Task<DataValueCollection> ReadNodesAsync(ReadValueIdCollection nodesToRead, CancellationToken cancellationToken);
    }
}