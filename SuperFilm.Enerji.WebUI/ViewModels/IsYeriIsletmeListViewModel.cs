﻿using SuperFilm.Enerji.Entites;

namespace SuperFilm.Enerji.WebUI.ViewModels
{
    public class IsYeriIsletmeListViewModel
    {
        public required List<Isletme> Isletmeler { get; set; }
        public required List<IsYeri> IsYerleri { get; set; }
    }
}
