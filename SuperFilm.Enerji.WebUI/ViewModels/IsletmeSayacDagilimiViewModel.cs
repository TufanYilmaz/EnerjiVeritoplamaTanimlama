using SuperFilm.Enerji.Entites;

namespace SuperFilm.Enerji.WebUI.ViewModels
{
    public class IsletmeSayacDagilimiViewModel
    {
        public required IsletmeSayacDagilimi IsletmeSayacDagilimi { get; set; }
        public required List<Isletme> Isletme { get; set; }
        public required List<SayacTanimlari> SayacTanimlari { get; set; }
    }
}
