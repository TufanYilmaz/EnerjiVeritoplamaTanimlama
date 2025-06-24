using SuperFilm.Enerji.Entites;

namespace SuperFilm.Enerji.WebUI.ViewModels
{
    public class IsletmeEnerjiTuketimViewModel
    {
        public required List<Isletme> Isletmeler { get; set; }
        public IsletmeEnerjiTuketimRequestModel RequestModel { get; set; }
    }
}
