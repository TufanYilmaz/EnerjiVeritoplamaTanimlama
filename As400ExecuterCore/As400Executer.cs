using System;
using System.Data;
using System.Data.OleDb;

namespace As400ExecuterCore
{
    public struct InsertResult
    {
        public int InsertCount { get; set; }

        public long InsertId { get; set; }
    }
    public class As400Executer
    {
        private OleDbConnection baglanti;

        private OleDbTransaction transaction;

        private bool usingTransaction;

      
        private string as400ConnectionString = "";

        public void BeginTransaction()
        {
            BaglantiAc();
            transaction = baglanti.BeginTransaction();
            usingTransaction = true;
        }

        public void RollbackTransaction()
        {
            transaction.Rollback();
            usingTransaction = false;
            baglanti.Close();
        }

        public void CommitTransaction()
        {
            transaction.Commit();
            usingTransaction = false;
            BaglantiKapat();
        }

        public As400Executer(string connectionString)
        {
            as400ConnectionString = connectionString;
            baglanti = new OleDbConnection(as400ConnectionString);
        }

        public bool BaglantiAc()
        {
            if (baglanti.State != ConnectionState.Open)
            {
                baglanti = new OleDbConnection(as400ConnectionString);
                baglanti.Open();
                return true;
            }

            return false;
        }

        public bool BaglantiKapat()
        {
            try
            {
                if (baglanti.State != 0)
                {
                    baglanti.Close();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public object ExecuteScalar(string query = "", bool useTransaction = false, OleDbCommand command = null, bool closeConnection = true)
        {
            BaglantiAc();
            if (useTransaction)
            {
                BeginTransaction();
            }

            if (command == null)
            {
                command = new OleDbCommand(query, baglanti);
            }

            if (usingTransaction)
            {
                command.Transaction = transaction;
            }

            command.Connection = baglanti;
            object result = command.ExecuteScalar();
            if (!usingTransaction && closeConnection)
            {
                BaglantiKapat();
            }

            return result;
        }

        public int ExecuteNonQuery(string query = "", bool useTransaction = false, OleDbCommand command = null, bool closeConnection = true, bool batchInsert = false)
        {
            BaglantiAc();
            if (useTransaction)
            {
                BeginTransaction();
            }

            if (command == null)
            {
                command = new OleDbCommand(query, baglanti);
            }

            if (usingTransaction)
            {
                command.Transaction = transaction;
            }

            command.Connection = baglanti;
            int num = command.ExecuteNonQuery();
            int result = -1;
            if (num > 0 && !batchInsert && command.CommandText.ToLower().Contains("insert"))
            {
                command.CommandText = "SELECT IDENTITY_VAL_LOCAL() FROM SYSIBM.SYSDUMMY1";
                object obj = command.ExecuteScalar();
                if (obj != null)
                {
                    int.TryParse(obj.ToString(), out result);
                }
            }

            if (!usingTransaction && closeConnection)
            {
                BaglantiKapat();
            }

            if (result <= 0)
            {
                return num;
            }

            return result;
        }

        public InsertResult InsertQuery(string query = "", bool useTransaction = false, OleDbCommand command = null, bool closeConnection = true, bool batchInsert = false)
        {
            BaglantiAc();
            if (useTransaction)
            {
                BeginTransaction();
            }

            if (command == null)
            {
                command = new OleDbCommand(query, baglanti);
            }

            if (usingTransaction)
            {
                command.Transaction = transaction;
            }

            command.Connection = baglanti;
            long result = -1L;
            int ınsertCount = command.ExecuteNonQuery();
            if (!batchInsert)
            {
                command.CommandText = "SELECT IDENTITY_VAL_LOCAL() FROM SYSIBM.SYSDUMMY1";
                object obj = command.ExecuteScalar();
                if (obj != null)
                {
                    long.TryParse(obj.ToString(), out result);
                }
            }

            if (!usingTransaction && closeConnection)
            {
                BaglantiKapat();
            }

            InsertResult result2 = default(InsertResult);
            result2.InsertCount = ınsertCount;
            result2.InsertId = result;
            return result2;
        }

        public DataTable ExecuteQuery(string query = "", bool useTransaction = false, OleDbCommand command = null, bool closeConnection = true)
        {
            BaglantiAc();
            DataTable dataTable = new DataTable();

            if (useTransaction)
            {
                BeginTransaction();
            }

            if (command == null)
            {
                command = new OleDbCommand(query, baglanti);
            }

            if (usingTransaction)
            {
                command.Transaction = transaction;
            }

            command.Connection = baglanti;
            OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(command);
            oleDbDataAdapter.Fill(dataTable);
            if (!usingTransaction && closeConnection)
            {
                BaglantiKapat();
            }

            return dataTable;
        }
    }
}