namespace SuperFilm.Enerji.WebUI.Models
{
    public class IsletmeEnerjiModel
    {
    }
    public class SayacTuketimModel
    {
        public string Kod { get; set; }
        public string Aciklama { get; set; }
        public DateTime FirstReadDate { get; set; }
        public DateTime LastReadDate { get; set; }
        public decimal FirstReadValue { get; set; }
        public decimal LastReadValue { get; set; }
        public decimal Value => LastReadValue - FirstReadValue;

    }
}
