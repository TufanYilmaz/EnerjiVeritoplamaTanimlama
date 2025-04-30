using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperFilm.Enerji.Entites;
using SuperFilm.Enerji.Repository;
using TanvirArjel.EFCore.GenericRepository;

namespace SuperFilm.Enerji.WebUI.Controllers
{
    public class SayacVeriGrafikController : Controller
    {
        private readonly EnerjiVeriRepository<EnerjiDbContext> _repository;
        private readonly IQueryRepository<EnerjiDbContext> _queryRepository;
        public SayacVeriGrafikController(
            EnerjiVeriRepository<EnerjiDbContext> repository,
            IQueryRepository<EnerjiDbContext> queryRepository
        )
        {
            _repository = repository;
            _queryRepository = queryRepository;
        }

        public async Task<IActionResult> Index()
        {
           
            var sayactanımları = await _queryRepository.GetQueryable<SayacTanimlari>().ToListAsync();
            return View(sayactanımları);
        }
        [HttpPost]
        public async Task<IActionResult> Index(int TimeTypeId,DateTime? Gun, DateTime? Ay, int SayacId)
        {
            List<LineChartData> chartData = new List<LineChartData>();
            if (TimeTypeId == 1)
            {
                var days = await _repository.GetDailyAsync(Convert.ToDateTime(Gun), SayacId);
                if(days != null)
                {
                    // Process daily data
                    chartData = DailyLine(days);

                }
            }
            else
            {
                var months = await _repository.GetMonthlyAsync(Convert.ToDateTime(Ay), SayacId);
                if (months != null)
                {
                    // Process monthly data
                    chartData = MonthlyLine(months);
                }
            }
            ViewData["ChartData"] = chartData;

            var sayactanımları = await _queryRepository.GetQueryable<SayacTanimlari>().ToListAsync();
            return View(sayactanımları);
        }

        public List<LineChartData> DailyLine(List<SayacVeri> model)
        {
            List<LineChartData> chartData = new List<LineChartData>();

            foreach (var item in model) 
            {
                LineChartData data = new LineChartData();
                data.Zaman = item.Zaman;
                data.Deger = item.Deger;
                chartData.Add(data);
            }

            return chartData;
        }
        public List<LineChartData> MonthlyLine(List<SayacVeri> model)
        {
            List<LineChartData> chartData = new List<LineChartData>();

            foreach (var item in model)
            {
                LineChartData data = new LineChartData();
                data.Zaman = item.Ay; ;
                data.Deger = item.Deger;
                chartData.Add(data);
            }

            return chartData;

        }
        public class LineChartData
        {
            public string Zaman;
            public decimal Deger;
        }
    }
}
