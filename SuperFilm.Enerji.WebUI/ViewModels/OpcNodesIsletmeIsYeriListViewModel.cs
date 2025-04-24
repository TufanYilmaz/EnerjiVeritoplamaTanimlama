using SuperFilm.Enerji.Entites;

namespace SuperFilm.Enerji.WebUI.ViewModels
{
    public class OpcNodesIsletmeIsYeriListViewModel
    {
        public required List<OpcNodesIsletmeDagilimi> OpcNodesIsletmeDagilimi { get; set; } = new();
        public required List<IsYeri> IsYeri { get; set; } = new();
    }
}
