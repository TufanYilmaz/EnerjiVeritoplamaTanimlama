using SuperFilm.Enerji.Entites;
using System.Data;

namespace Superfilm.Enerji.VeriToplama.Services
{
    public interface ISayacVeriKaydet
    {
        void Kaydet(DataSet data, List<SayacTanimlari> sayaclar);
        Task<DataSet> VeriCek(string fileCode);
        void Start();
    }
}
