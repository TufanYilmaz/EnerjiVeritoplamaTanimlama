using Hangfire.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperFilm.Enerji.Entites;
using SuperFilm.Enerji.WebUI.ViewModels;
using TanvirArjel.EFCore.GenericRepository;

namespace SuperFilm.Enerji.WebUI.Controllers
{
    [Authorize]
    public class IsletmeEnerjiTuketimi(ILogger<HomeController> _logger,
            IRepository _repository,
            IQueryRepository<EnerjiDbContext> _queryRepository,
            IRepository<EnerjiDbContext> _enerjiRepository) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var isletmeler = await _queryRepository.GetListAsync<Isletme>(includes:r=>r.Include(x=>x.Isyeri));
            IsletmeEnerjiTuketimViewModel model = new IsletmeEnerjiTuketimViewModel() 
            { 
                Isletmeler = isletmeler
            };
            return View(model);
        }
        public async Task<IActionResult> IsletmeSayacVerileri(IsletmeEnerjiTuketimRequestModel RequestModel)
        {
            var isletme = await _queryRepository.GetAsync<Isletme>(r => r.Id == RequestModel.IsletmeId, r => r.Include(x => x.Isyeri));
            if(isletme != null && isletme.Isyeri!.Kodu == "AS02")
            {
                var OpcNodesIsletme = await _queryRepository.GetListAsync<OpcNodesIsletmeDagilimi>(r => r.IsletmeId == RequestModel.IsletmeId);
                var OpcNodes = await _queryRepository.GetListAsync<OpcNodes>(r => OpcNodesIsletme.Select(x => x.OpcNodesId).ToList().Contains(r.Id));
                var sayacverileri = await _queryRepository.GetListAsync<SayacVeri>(r =>
                r.NormalizeDate > RequestModel.StartDate.AddMinutes(-2)
                && r.OpcNodesId != null
                && OpcNodesIsletme.Select(x => x.Id).ToList().Contains(r.OpcNodesId.Value)
                );
                var groupdSayacVeri = _repository.GetQueryable<SayacVeri>().Where(r =>
                     r.NormalizeDate > RequestModel.StartDate.AddMinutes(-2)
                    && r.NormalizeDate < RequestModel.EndDate.AddMinutes(2)
                    && r.OpcNodesId != null
                    && OpcNodesIsletme.Select(x => x.OpcNodesId).ToList().Contains(r.OpcNodesId.Value)
                ).GroupBy(g => g.OpcNodesId)
                .Select(r =>
                    new IsletmeEnerjiTuketimVeriModel()
                    {
                        Id=r.Key ?? 0,
                        EndDate=r.Max(x=>x.NormalizeDate).ToString("dd/MM/yyyy HH:mm:ss"),
                        StartDate=r.Min(x=>x.NormalizeDate).ToString("dd/MM/yyyy HH:mm:ss"),
                        StartValue = r.Min(x=>x.Deger),
                        EndValue = r.Max(x=>x.Deger),
                    }
                ).ToList();
                foreach(var deger in groupdSayacVeri)
                {
                    deger.Aciklama = OpcNodes.Where(r => r.Id == deger.Id).Select(r => r.Code +" - "+ r.Description).FirstOrDefault();
                    deger.TotalValue = groupdSayacVeri.Sum(r => r.Value);
                }
                return new JsonResult(groupdSayacVeri);

            }
            else
            {
                return new JsonResult(null);
            }


        }

    }

}
