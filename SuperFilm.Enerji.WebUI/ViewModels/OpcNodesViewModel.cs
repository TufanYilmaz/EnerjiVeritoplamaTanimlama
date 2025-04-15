using SuperFilm.Enerji.Entites;

namespace SuperFilm.Enerji.WebUI.ViewModels
{
    public class OpcNodesViewModel
    {
        public required OpcNodes OpcNodes { get; set; }
        public required List<Isletme> Isletmeler { get; set; }
    }
}
