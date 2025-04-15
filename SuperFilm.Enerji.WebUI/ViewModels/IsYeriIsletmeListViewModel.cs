using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperFilm.Enerji.Entites
{
    public class IsYeriIsletmeListViewModel
    {
        public required List<Isletme> Isletmeler { get; set; }
        public required List<IsYeri> IsYerleri { get; set; }
    }
}
