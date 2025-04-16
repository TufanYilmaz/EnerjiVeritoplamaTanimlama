using SuperFilm.Enerji.Entites;

namespace SuperFilm.Enerji.WebUI.ViewModels
{
    public class OpcNodesSayacDagilimiViewModel
    {
        public required OpcNodesSayacDagilimi OpcNodesSayacDagilimi { get; set; }
        public required List<OpcNodes> OpcNodes { get; set; }
        public required List<SayacTanimlari> SayacTanimlari { get; set; }
    }
}
