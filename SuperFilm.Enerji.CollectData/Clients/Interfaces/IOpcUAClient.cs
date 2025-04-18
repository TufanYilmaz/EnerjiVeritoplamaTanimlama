﻿using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperFilm.Enerji.CollectData.Clients.Interfaces
{
    public interface IOpcUAClient
    {
        Task<DataValueCollection> ReadNodes(CancellationToken cancellationToken);
        Task LoadNodes(CancellationToken cancellationToken);

    }
}
