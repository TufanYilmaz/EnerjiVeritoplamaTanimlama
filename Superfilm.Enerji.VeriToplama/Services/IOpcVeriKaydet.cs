using Opc.Ua;
using SuperFilm.Enerji.Entites;
using System.Data;

namespace Superfilm.Enerji.VeriToplama.Services
{
    public interface IOpcVeriKaydet
    {
        Task Kaydet(List<SayacVeri> data, List<OpcNodes> opcNodes);
        Task<List<OpcNodes>> GetOpcNodesAsync();
        Task<List<SayacVeri>> VeriCekAsync(List<OpcNodes> opcNodes, CancellationToken cancellationToken);
        Task Start(CancellationToken cancellationToken);
    }
}
