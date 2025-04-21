using SuperFilm.Enerji.Entites;
using SuperFilm.Enerji.OleDbReader;
using System.Data;
using TanvirArjel.EFCore.GenericRepository;

namespace Superfilm.Enerji.VeriToplama.Services
{
    public class SayacVeriKaydet(
        IQueryRepository<EnerjiDbContext> queryRepository,
            IRepository<EnerjiDbContext> enerjiRepository,
        IConfiguration configuration) : ISayacVeriKaydet
    {
        Client client = new Client(configuration.GetValue<string>( "RootFilePath") ?? "");
        List<string> _fileCodes = new List<string>() { "6", "11", "12", "19", "20", "22", "29" };
        public async void Kaydet(DataSet data,List<SayacTanimlari> sayaclar)
        {
            if (data == null)
                return;
            if (data.Tables.Count == 0)
                return;
            foreach (DataRow row in data.Tables[0].Rows)
            {
                string sayacKodu = row["CID"].ToString() ?? "";
                var sayac = sayaclar.FirstOrDefault(r => r.SayacKodu == sayacKodu);
                if (sayac == null)
                {
                    continue;
                }
                SayacVeri sayacverimodel = new SayacVeri();
                DateTime time = Convert.ToDateTime(row["TimeRead"]);
                sayacverimodel.Kod = sayac.SayacKodu;
                sayacverimodel.SayacId = sayac.Id;
                sayacverimodel.Yil = time.ToString("yyyy");
                sayacverimodel.Ay = time.ToString("MM");
                sayacverimodel.Gun = time.ToString("dd");
                sayacverimodel.Zaman = time.ToString("hhmmss");
                sayacverimodel.Deger = Convert.ToDecimal(row["WHT1"]);
                //sayacverimodel.Zaman = time.ToString("hh:mm:ss").Replace(":","");

            }
        }
        public async void Start()
        {
            var sayaclar = await queryRepository.GetListAsync<SayacTanimlari>();
            foreach (var code in _fileCodes)
            {
                var data = await VeriCek(code);
                Kaydet(data, sayaclar);
            }
        }

        public async Task<DataSet> VeriCek(string fileCode)
        {
            return await client.ReadFile(fileCode,DateTime.Now.AddDays(-1));
        }
        
    }
}
