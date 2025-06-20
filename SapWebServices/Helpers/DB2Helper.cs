using As400ExecuterCore;
using SapWebServices.Model;
using System.Data.OleDb;
using System.Data;

namespace SapWebServices.Helpers
{
    public class DB2Helper: IDB2Helper
    {
        private readonly IConfiguration _config;
        private readonly string _connString;
        static As400Executer as400Executer;
        public DB2Helper(IConfiguration configuration)
        {
            _config = configuration;
            _connString = _config.GetConnectionString("AS400");
            as400Executer =  new As400Executer(_connString);

        }
        public static string MakeTwoDigits(int input)
        {
            return input < 10 ? "0" + input : input.ToString();
        }
        public EnerjiModel GetEnerji(string? UretimYeri,DateTime startDateTime,DateTime endDateTime,HelperPlants? helperPlants)
        {
            var result = new EnerjiModel
            {
                UretimYeri = UretimYeri
            };
            try
            {
                result.DDeger = GetTuketim(startDateTime, endDateTime, UretimYeri);
                
                string commandSqlEndirek = As400SQL.GetActivePlatnts
                    .Replace("pMKOD", "'" + UretimYeri + "'")
                    .Replace("pW01", "'" + helperPlants.MSW01 + "'")
                    .Replace("pW02", "'" + helperPlants.MSW02 + "'")
                    .Replace("pW03", "'" + helperPlants.MSW03 + "'")
                    .Replace("pC01", "'" + helperPlants.MSC01 + "'")
                    .Replace("pBW1", "'" + helperPlants.MSBW1 + "'")
                    .Replace("pBW2", "'" + helperPlants.MSBW2 + "'")
                    .Replace("pM01", "'" + helperPlants.MSM01 + "'")
                    .Replace("pM02", "'" + helperPlants.MSM02 + "'")
                    .Replace("pM03", "'" + helperPlants.MSM03 + "'")
                    .Replace("pM04", "'" + helperPlants.MSM04 + "'")
                    .Replace("pM05", "'" + helperPlants.MSM05 + "'")
                    .Replace("pM06", "'" + helperPlants.MSM06 + "'")
                    .Replace("pK01", "'" + helperPlants.MSK01 + "'")
                    .Replace("pTE1", "'" + helperPlants.MSTE1 + "'")
                    .Replace("pTE2", "'" + helperPlants.MSTE2 + "'")
                    .Replace("pYIS", "'" + helperPlants.MSYIS + "'");
                var emResult = as400Executer.ExecuteQuery(commandSqlEndirek);
                double endirectResult = 0;
                List<string> plants = new List<string>();
                foreach (DataRow row in emResult.Rows)
                {
                    double endirect = GetTuketim(startDateTime, endDateTime, row["MSISKD"].ToString());
                    endirectResult += (endirect * Convert.ToDouble(row["MSORAN"])) / 100;
                }
                result.EDeger = endirectResult;

            }
            catch (Exception ex)
            {
            }

            return result;

        }
        static double GetTuketim(DateTime startDateTime,DateTime endDateTime,string? plant)
        {
            double result = 0;
            try
            {
                string starttimeformat = startDateTime.ToString().Split(' ')[1].Replace(":", "");
                string starttimeToleransformat = startDateTime.AddMinutes(5).ToString().Split(' ')[1].Replace(":", "");
                string endtimeformat = endDateTime.ToString().Split(' ')[1].Replace(":", "");
                string endtimeToleransformat = endDateTime.AddMinutes(5).ToString().Split(' ')[1].Replace(":", "");
                string commandSqlDirect = As400SQL.EnerjiForUretimYeriBetweenDates
                    //.Replace("P1", "'" + string.Join(",",Plants )+ "'")
                    .Replace("P1", "'" + plant + "'")
                    .Replace("startyear", "'" + startDateTime.Year + "'")
                    .Replace("startmonth", "'" + MakeTwoDigits(startDateTime.Month) + "'")
                    .Replace("startday", "'" + MakeTwoDigits(startDateTime.Day) + "'")
                    .Replace("starttime", "'" + starttimeformat + "'")
                    .Replace("starttolerance", "'" + starttimeToleransformat + "'")
                    //.Replace("P2", "'" + string.Join(",", Plants) + "'")
                    .Replace("P2", "'" + plant + "'")
                    .Replace("endyear", "'" + endDateTime.Year + "'")
                    .Replace("endmonth", "'" + MakeTwoDigits(endDateTime.Month) + "'")
                    .Replace("endday", "'" + MakeTwoDigits(endDateTime.Day) + "'")
                    .Replace("endtime", "'" + endtimeformat + "'")
                    .Replace("endtolerance", "'" + endtimeToleransformat + "'");
                var dResult = as400Executer.ExecuteQuery(commandSqlDirect);
                foreach (DataRow row in dResult.Rows)
                {
                    if (row["DEGER"] != DBNull.Value && row["SONDEGER"] != DBNull.Value)
                    {
                        double yon = row["YON"].ToString() == "+" ? 1 : -1;
                        double basdeger = Convert.ToDouble(row["DEGER"]);
                        double sondeger = Convert.ToDouble(row["SONDEGER"]);
                        double carpan = Convert.ToDouble(row["CARPAN"]);
                        result += yon * (sondeger - basdeger) * carpan;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return result > 0 ? result : 0;
        }

        public EnerjiModel GetEnerji(EnerjiRequestModel enerjiRequest)
        {
            return GetEnerji(enerjiRequest.ProductionLine,enerjiRequest.StartDateTime,enerjiRequest.EndDateTime,enerjiRequest.HelperPlants);
        }
    }
}
