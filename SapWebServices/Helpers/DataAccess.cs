using Dapper;
using SapWebServices.Model;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace SapWebServices.Helpers
{
	public class DataAccess : IDataAccess
	{
		private readonly IConfiguration _config;
		private readonly string _connString;
        public DataAccess(IConfiguration configuration)
        {
            _config = configuration;
            _connString = _config.GetConnectionString("Mes");

        }
        //IDbConnection GetConnection()
        //{
        //    using IDbConnection connection = new SQLiteConnection(_connString);
        //    return connection;
        //}
		SqlConnection GetMsSqlConnection()
		{
			return new SqlConnection(_connString);
		}
        public async Task<IEnumerable<EnerjiRequestAdvanceModelDb>> GetAsycn()
        {
			using SqlConnection connection =  GetMsSqlConnection();

			if (connection.State != ConnectionState.Open)
				connection.Open();
			string sql = "Select * from EnerjiRequestAdvanceModelDb";
            var res = await connection.QueryAsync<EnerjiRequestAdvanceModelDb>(sql);
			connection.Close();
            return res;

		}
		public async Task<EnerjiRequestAdvanceModelDb> GetAsycn(EnerjiRequestAdvanceModel model)
		{
			using SqlConnection connection = GetMsSqlConnection();
			try
			{

				if (connection.State != ConnectionState.Open)
					connection.Open();
				string sql = @"Select * from EnerjiRequestAdvanceModelDb where
                    ProductionLine=@ProductionLine and 
                    StartDate=@StartDate and 
                    StartTime=@StartTime and
                    EndDate=@EndDate and
                    EndTime=@EndTime";
				var res = await connection.QueryAsync<EnerjiRequestAdvanceModelDb>(sql, model);
				return res.FirstOrDefault();
			}
			finally
			{
				connection.Close();
			}
			return null;
		}
        public async Task<int> AddAsync(EnerjiRequestAdvanceModelDb model)
        {
			using SqlConnection connection = GetMsSqlConnection();
			try
			{
				if (connection.State != ConnectionState.Open)
					connection.Open();
				string sql = @"insert into EnerjiRequestAdvanceModelDb(ProductionLine,StartDate,StartTime,EndDate,EndTime,DDeger,EDeger)
values (@ProductionLine ,@StartDate,@StartTime ,@EndDate, @EndTime,@DDeger,@EDeger) RETURNING Id; ";
				int res = await connection.ExecuteScalarAsync<int>(sql, model);
			}
			finally
			{
				connection.Close();
			}

			return 0;
		}

		public int Add(EnerjiRequestAdvanceModelDb model)
		{
			using SqlConnection connection = GetMsSqlConnection();
			try
			{
				if(connection.State!= ConnectionState.Open)
				connection.Open();
				string sql = @"insert into EnerjiRequestAdvanceModelDb(ProductionLine,StartDate,StartTime,EndDate,EndTime,DDeger,EDeger)
values (@ProductionLine ,@StartDate,@StartTime ,@EndDate, @EndTime,@DDeger,@EDeger)  ";
				int res = connection.ExecuteScalar<int>(sql, model);
				return res;

			}
			finally
			{
				connection.Close();

			}
			return 0;
		}

		public EnerjiRequestAdvanceModelDb Get(EnerjiRequestAdvanceModel model)
		{
			using SqlConnection connection = GetMsSqlConnection();
			try
			{
				if (connection.State != ConnectionState.Open)
					connection.Open();
				string sql = @"Select * from EnerjiRequestAdvanceModelDb where
                    ProductionLine=@ProductionLine and 
                    StartDate=@StartDate and 
                    StartTime=@StartTime and
                    EndDate=@EndDate and
                    EndTime=@EndTime";
				var res = connection.Query<EnerjiRequestAdvanceModelDb>(sql, model);
				return res.FirstOrDefault();
			}
			finally
			{
				connection.Close();
			}
			return null;
		}
	}
}
