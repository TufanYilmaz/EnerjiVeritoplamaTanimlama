using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperFilm.Enerji.Entites
{
    public class IsYeriIsletmeViewModel
    {
        public required IsYeri IsYeri { get; set; }
        public required List<IsletmeTanimlari> IsletmeTanimlari { get; set; }
    }
}
