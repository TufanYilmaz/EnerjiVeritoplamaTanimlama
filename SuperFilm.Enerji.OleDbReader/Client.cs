using Microsoft.AspNetCore.Http;
using SuperFilm.Enerji.Entites;
using System.Data;
using System.Data.OleDb;

namespace SuperFilm.Enerji.OleDbReader
{
    public class Client
    {
        OleDbConnection _oleDbConnection;
        //string _connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;";
        const string _connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;";
        readonly string _rootFile = string.Empty;
        List<string> _fileCodes = new List<string>() { "6", "11", "12", "19", "20", "22", "29" };
        public Client(string rootFile)
        {
            _rootFile = rootFile.Replace("\"","");
        }
        public async Task<DataSet> ReadFile(string fileName)
        {
            string toReadFile = Path.Combine(_rootFile , fileName);
            if (File.Exists(toReadFile))
            {
                string toReadconnectionString = _connectionString + "Data Source=" + toReadFile + ";";
                try
                {
                    using (_oleDbConnection = new OleDbConnection(toReadconnectionString))
                    {
                        _oleDbConnection.Open();
                        string sqlCommand = "SELECT TimeRead,CID,WHT1 FROM NPM50";
                        OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlCommand, _oleDbConnection);
                        DataSet ds = new DataSet();
                        dataAdapter.Fill(ds, "SAYAC_VERI");
                        _oleDbConnection.Close();
                        return ds;
                    }

                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public async Task<DataSet> ReadFile(string fileCode,DateTime date)
        {
            //20c20250210
            return await ReadFile(fileCode + "c" + date.ToString("yyyyMMdd")+".mdb");
        }
    }
}
