using System.Data;
using System.Data.OleDb;

namespace SuperFilm.Enerji.OleDbReader
{
    public class Client
    {
        OleDbConnection _oleDbConnection;
        string _connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;";
        string _rootFile = @"\\10.11.3.78\Actwin EMS\Data\Trend\";
        List<string> _fileCodes = new List<string>() { "6", "11", "12", "19", "20", "22", "29" };
        public Client()
        {
            
        }
        public void ReadFile(string fileName)
        {
            string toReadFile = _rootFile + fileName;
            if (File.Exists(toReadFile))
            {
                _connectionString = _connectionString + "Data Source=" + toReadFile + ";";
                try
                {
                    using (_oleDbConnection = new OleDbConnection(_connectionString))
                    {
                        _oleDbConnection.Open();
                        string sqlCommand = "SELECT TimeRead,CID,WHT1 FROM NPM50";
                        OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlCommand, _oleDbConnection);
                        DataSet ds = new DataSet();
                        dataAdapter.Fill(ds, "SAYAC_VERI");
                        _oleDbConnection.Close();
                    }

                }
                catch (Exception ex)
                {
                }
            }
            else
            {

            }
        }
        public void ReadFile(string fileCode,DateTime date)
        {
            //20c20250210
            ReadFile(fileCode + "c" + date.ToString("yyyyMMdd")+".mdb");
        }
    }
}
