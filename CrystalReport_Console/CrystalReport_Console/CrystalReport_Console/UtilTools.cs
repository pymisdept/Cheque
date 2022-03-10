using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
namespace CrystalReport_Console
{
    class UtilTools
    {
        static public void ExecuteDatabase(string TempStoreStr, string _connectionString)
        {
            SqlTransaction trans = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    command.CommandType = CommandType.Text;

                    command.CommandText = TempStoreStr;

                    connection.Open();
                    trans = connection.BeginTransaction();
                    command.Transaction = trans;
                    command.Connection = connection;
                    command.ExecuteNonQuery();
                    trans.Commit();

                    trans.Dispose();
                    connection.Close();
                    command.Dispose();
                    connection.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    trans.Rollback();
                    trans.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
    }
}
