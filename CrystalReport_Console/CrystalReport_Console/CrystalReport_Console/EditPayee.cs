using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;

namespace CrystalReport_Console
{
    public partial class EditPayee : Form
    {
        static string _connectionString;
        string Cheque_Usage;
        string ChequeID;
        string dbname;
        public EditPayee(string[] param)
        {
            InitializeComponent();

            _connectionString = "Data Source=10.1.1.191;Initial Catalog=" + param[0] + ";User ID=sa;Password=fa920711";
            string queryString = "select c.*, b.ChequeDate from Cheque c inner join Batch b on c.BatchID=b.BatchID";
            queryString = queryString + " where c.ChequeID=" + param[1];

            string dbname = param[0];
            string ChequeDate = "";
            string Payee = "";
            string Curr_Code = "";
            string Amount = "";
            string Total = "";
            string ChequeLayoutID = "";
            string Cheque_Usage = "";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                connection.Open();
                using (SqlDataReader datareader = command.ExecuteReader())
                {
                    if (datareader.HasRows == true)
                    {
                        while (datareader.Read())
                        {
                            if (datareader["ChequeDate"] != System.DBNull.Value)
                            {
                                ChequeDate = datareader["ChequeDate"].ToString();
                            }
                            if (datareader["PayeeName"] != System.DBNull.Value)
                            {
                                Payee = datareader["PayeeName"].ToString();
                            }
                            if (datareader["Curr_Code"] != System.DBNull.Value)
                            {
                                Curr_Code = datareader["Curr_Code"].ToString();
                            }
                            if (datareader["Amount"] != System.DBNull.Value)
                            {
                                Amount = datareader["Amount"].ToString();
                            }
                            if (datareader["Total"] != System.DBNull.Value)
                            {
                                Total = datareader["Total"].ToString();
                            }
                            if (datareader["Cheque_Usage"] != System.DBNull.Value)
                            {
                                Cheque_Usage = datareader["Cheque_Usage"].ToString();
                            }
                        }
                    }
                    datareader.Close();
                }
                connection.Close();
            }
            payee_textBox.Text = Payee;
            cheque_usage_textBox.Text = Cheque_Usage;

            if (param[2] == "18"
            || param[2] == "19"
            || param[2] == "19")
            {
                cheque_usage_textBox.Visible = true;
                label2.Visible = true;
            }
            else
            {
                cheque_usage_textBox.Visible = false;
                label2.Visible = false;
            }
            ChequeID = param[1];
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void save_button_Click(object sender, EventArgs e)
        {
            string queryString = "select c.*, b.ChequeDate from Cheque c inner join Batch b on c.BatchID=b.BatchID";
            queryString = queryString + " where c.ChequeID=" + ChequeID;

            string Payee = "";
            string Cheque_Usage = "";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                connection.Open();
                using (SqlDataReader datareader = command.ExecuteReader())
                {
                    if (datareader.HasRows == true)
                    {
                        while (datareader.Read())
                        {
                            if (datareader["PayeeName"] != System.DBNull.Value)
                            {
                                Payee = datareader["PayeeName"].ToString();
                            }
                            if (datareader["Cheque_Usage"] != System.DBNull.Value)
                            {
                                Cheque_Usage = datareader["Cheque_Usage"].ToString();
                            }
                        }
                    }
                    datareader.Close();
                }
                connection.Close();
            }
            getspid();
            string updateString = "";

            updateString = "Update Cheque set ";
            updateString = updateString + " PayeeName=N'" + payee_textBox.Text+"'";
            updateString = updateString + " ,Cheque_Usage=N'" + cheque_usage_textBox.Text+"'";
            updateString = updateString + " where ChequeID=" + ChequeID;
            UtilTools.ExecuteDatabase(updateString, _connectionString);

            //MessageBox.Show(ChequeID);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                connection.Open();
                using (SqlDataReader datareader = command.ExecuteReader())
                {
                    if (datareader.HasRows == true)
                    {
                        while (datareader.Read())
                        {
                            if (datareader["PayeeName"] != System.DBNull.Value)
                            {
                                Payee = datareader["PayeeName"].ToString();
                            }
                            else
                            {
                                Payee = "";
                            }
                            if (datareader["Cheque_Usage"] != System.DBNull.Value)
                            {
                                Cheque_Usage = datareader["Cheque_Usage"].ToString();
                            }
                            else
                            {
                                Cheque_Usage = "";
                            }
                        }
                    }
                    datareader.Close();
                }
                connection.Close();
            }
            payee_textBox.Text = Payee;
            cheque_usage_textBox.Text = Cheque_Usage;

            this.Close();
            this.Dispose();
        }
        static public void getspid()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = "select @@SPID as spid";
                    connection.Open();
                    using (SqlDataReader datareader = command.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            if (datareader["spid"] != System.DBNull.Value)
                            {
                                //error.Text = "spid" + datareader["spid"].ToString() + "$$$";
                            }
                        }
                        datareader.Close();
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    connection.Close();
                }
            }
        }
        
    }
}
