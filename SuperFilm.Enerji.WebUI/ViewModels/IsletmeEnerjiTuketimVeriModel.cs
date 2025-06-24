namespace SuperFilm.Enerji.WebUI.ViewModels
{
    public class IsletmeEnerjiTuketimVeriModel
    {
        /*
          <th>Kod</th>
                <th>Aciklama</th>
                <th>Başlangıç Tarihi</th>
                <th>Bitiş Tarihi</th>
                <th>Değer</th>
         */
        public string Aciklama { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public decimal Value { get; set; }
    }
}
