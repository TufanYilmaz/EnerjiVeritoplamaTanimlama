using SuperFilm.Enerji.Entites;

namespace SuperFilm.Enerji.WebUI.ViewModels
{
    public class OpcNodesIsletmeDagilimiViewModel
    {
        public required OpcNodesIsletmeDagilimi OpcNodesIsletmeDagilimi { get; set; }
        public required List<OpcNodes> OpcNodes { get; set; }
        public required List<Isletme> Isletme { get; set; }
    }
}
