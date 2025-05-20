using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperFilm.Enerji.Entites;

namespace SuperFilm.Enerji.Repository
{
    public interface IEnerjiVeriRepository<TDbContext> where TDbContext : DbContext
    {
        Task<List<SayacVeri>> GetDailyAsync(DateTime gun, int sayacId);
        Task<List<SayacVeri>> GetMonthlyAsync(DateTime ay, int sayacId);
        Task<List<SayacVeri>> GetDailyDiffAsync(DateTime gun, int sayacId);
        Task<List<SayacVeri>> GetMonthlyDiffAsync(DateTime ay, int sayacId);
        Task<List<SayacVeri>> GetOpcNodeDailyAsync(DateTime gun, int opcNodeId);
        Task<List<SayacVeri>> GetOpcNodeMonthlyEndOfDayAsync(DateTime ay, int opcNodeId);
        Task<(decimal minValue, decimal maxValue, decimal interval)> CalculateChartAxisValues(List<SayacVeri> data);
        List<SayacVeri> CompleteDailyData(List<SayacVeri> existingData);
        List<SayacVeri> CompleteMonthlyData(List<SayacVeri> existingData, DateTime selectedMonth);
        Task<List<SayacVeri>> OpcGetDailyDiffAsync(DateTime gun, int? OpcNodesId);
        Task<List<SayacVeri>> OpcGetMonthlyDiffAsync(DateTime ay, int? OpcNodesId);
        Task<List<string>> GetDistinctOpcNodeIds();
    }
}
