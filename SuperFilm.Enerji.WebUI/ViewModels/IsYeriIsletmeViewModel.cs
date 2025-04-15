using SuperFilm.Enerji.Entites;

namespace SuperFilm.Enerji.WebUI.ViewModels
{
    public class IsYeriIsletmeViewModel
    {
        public required Isletme Isletme { get; set; }
        public required List<IsYeri> IsYerleri { get; set; }
    }
}
