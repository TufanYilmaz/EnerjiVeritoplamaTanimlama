using SuperFilm.Enerji.Entites;

namespace SuperFilm.Enerji.WebUI.ViewModels
{
    public class RequestViewModel
    {
        public required List<SayacTanimlari> SayacTanimlari { get; set; }
        public required List<OpcNodes> OpcNodes { get; set; }
        public required List<SayacVeri> SayacVeri { get; set; }
        public required SayacVeriRequest SayacVeriRequest { get; set; }
    }
}
