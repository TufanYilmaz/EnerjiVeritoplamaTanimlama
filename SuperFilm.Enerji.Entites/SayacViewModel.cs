using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperFilm.Enerji.Entites
{
    public class SayacViewModel
    {
        public required SayacTanimlari SayacTanimlari { get; set; }
        public required List<IsletmeTanimlari> IsletmeTanimlari { get; set; }
        public required List<IsYeri> IsYeri { get; set; }
    }
}
