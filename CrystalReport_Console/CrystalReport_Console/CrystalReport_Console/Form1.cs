using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

using System.Collections;

using System.Data.SqlClient;

namespace CrystalReport_Console
{
    public partial class Form1 : Form
    {
        public Form1(string[] param)
        {
            InitializeComponent();

            string _connectionString = "Data Source=10.1.1.191;Initial Catalog="+param[0]+";User ID=sa;Password=fa920711";
            string queryString = "select c.*, b.ChequeDate from Cheque c inner join Batch b on c.BatchID=b.BatchID";
            queryString = queryString + " where c.ChequeID=" + param[1];


            param[3] = param[3].Replace("貳", "贰");
            param[3] = param[3].Replace("陸", "陆");

            string ChequeDate = "";
            string Payee = "";
            string Curr_Code = "";
            string Amount = "";
            string Total = "";
            string ChequeLayoutID = "";
            string ChequeUsage = "";

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
                            if (datareader["Total"] != System.DBNull.Value)
                            {
                                Amount = datareader["Total"].ToString();
                            }
                            if (datareader["Total"] != System.DBNull.Value)
                            {
                                Total = datareader["Total"].ToString();
                            }
                            if (datareader["Cheque_Usage"] != System.DBNull.Value)
                            {
                                ChequeUsage = datareader["Cheque_Usage"].ToString();
                            }
                        }
                    }
                    datareader.Close();
                }
                connection.Close();
            }

            //ChequeDate = "3/3/2008 0:00:00";

            string[] date_items;
            string year = "";
            string month = "";
            string day = "";
            if (!string.IsNullOrEmpty(ChequeDate))
            {
                date_items = ChequeDate.Replace(" 0:00:00", "").Split('/');

                year = getchinesenum(date_items[2][0]) + getchinesenum(date_items[2][1]) + getchinesenum(date_items[2][2]) + getchinesenum(date_items[2][3]);
                month = getnum_formonth(date_items[1]);
                day = getnum_formonth(date_items[0]);
            }

            Amount = Math.Round(Convert.ToDouble(Amount), 2).ToString("#.00");
            //Console.WriteLine(Amount);

            char one;
            char two;
            char three = '0';
            char four = '0';
            char five = '0';
            char six = '0';
            char seven = '0';
            char eight = '0';
            char nine = '0';
            char ten = '0';
            char eleven = '0';
            char twelve = '0';

            one = Amount[Amount.Length - 1];
            two = Amount[Amount.Length - 2];

            // 3 is a point
            if( Amount.Length >= 4)
                three = Amount[Amount.Length - 4];
            if (Amount.Length >= 5)
                four = Amount[Amount.Length - 5];
            if (Amount.Length >= 6)
                five = Amount[Amount.Length - 6];
            if (Amount.Length >= 7)
                six = Amount[Amount.Length - 7];
            if (Amount.Length >= 8)
                seven = Amount[Amount.Length - 8];
            if (Amount.Length >= 9)
                eight = Amount[Amount.Length - 9];
            if (Amount.Length >= 10)
                nine = Amount[Amount.Length - 10];
            if (Amount.Length >= 11)
                ten = Amount[Amount.Length - 11];
            if (Amount.Length >= 12)
                eleven = Amount[Amount.Length - 12];
            if (Amount.Length >= 13)
                twelve = Amount[Amount.Length - 13];

            //Console.WriteLine(twelve.ToString() + " " + eleven.ToString() + " " + ten.ToString() + " " + nine.ToString() + " " + eight.ToString() + " " + seven.ToString() + " " + six.ToString() + " " + five.ToString() + " " + four.ToString() + " " + three.ToString() + " . " + two.ToString() + " " + one.ToString());
            //Console.ReadLine();

            string report_filename = "";
            string Layout_Printer = "";
            string Layout_Size = "";
            queryString = "Select * from ChequeLayout where ChequeLayoutID = " + param[2];
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
                            if (datareader["RptFileName"] != System.DBNull.Value)
                            {
                                report_filename = datareader["RptFileName"].ToString();
                                Layout_Printer = datareader["Layout_Printer"].ToString();
                                Layout_Size = datareader["Layout_Size"].ToString();
                            }
                        }
                    }
                    datareader.Close();
                }
                connection.Close();
            }
            ReportDocument objRpt = new ReportDocument();

            TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
            TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();

            Tables CrTables;

            try
            {
                string reportpath = "";
                queryString = "Select RptLoc from System";
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
                                if (datareader["RptLoc"] != System.DBNull.Value)
                                {
                                    reportpath = datareader["RptLoc"].ToString();
                                }
                            }
                        }
                        datareader.Close();
                    }
                    connection.Close();
                }

                //string reportpath = @"D:\Data\My Documents\Visual Studio 2008\Projects\CrystalReport_Console\CrystalReport_Console\Reports\" + report_filename;
                reportpath = reportpath + report_filename;

                objRpt.Load(reportpath);

                if (param[5] == "PREVIEW" || param[4] == "PREVIEW")
                {
                    ConnectionInfo connectionInfo = new ConnectionInfo();
                    connectionInfo.DatabaseName = param[0];
                    connectionInfo.UserID = "sa";
                    connectionInfo.Password = "fa920711";

                    //SetDBLogonForReport(connectionInfo);

                    //crystalReportViewer1.ReportSource = reportpath;
                    crystalReportViewer1.ReportSource = objRpt;
                    CrTables = objRpt.Database.Tables;
                    foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                    {
                        crtableLogoninfo = CrTable.LogOnInfo;

                        //MessageBox.Show("1");

                        connectionInfo.ServerName = crtableLogoninfo.ConnectionInfo.ServerName;
                        crtableLogoninfo.ConnectionInfo = connectionInfo;

                        //MessageBox.Show(crtableLogoninfo.ConnectionInfo.UserID);
                        //MessageBox.Show(crtableLogoninfo.ConnectionInfo.Password);
                        //MessageBox.Show(crtableLogoninfo.ConnectionInfo.DatabaseName);
                        //MessageBox.Show(crtableLogoninfo.ConnectionInfo.ServerName);

                        CrTable.ApplyLogOnInfo(crtableLogoninfo);

                        CrTable.LogOnInfo.ConnectionInfo = crtableLogoninfo.ConnectionInfo;

                        //MessageBox.Show("2");
                        //crystalReportViewer1.LogOnInfo.Add(CrTable.LogOnInfo);
                    }

                    //crystalReportViewer1.ReportSource = dgruralchinaTemplate1_HP1100_print1;

                    //MessageBox.Show("3");
                    crystalReportViewer1.Controls[2].Visible = false;

                    //MessageBox.Show("4");

                    if (param[5] == "PREVIEW" || param[4] == "PREVIEW")
                        crystalReportViewer1.ShowPrintButton = false;
                    else
                        crystalReportViewer1.ShowPrintButton = true;

                    //MessageBox.Show("5");

                    crystalReportViewer1.Refresh();

                    //MessageBox.Show("6");

                    ParameterFields parameterFields = crystalReportViewer1.ParameterFieldInfo;

                    
                    ArrayList arrayList0a = new ArrayList();
                    arrayList0a.Add(year);
                    SetCurrentValuesForParameterField(parameterFields, arrayList0a, "Year");

                    ArrayList arrayList0b = new ArrayList();
                    arrayList0b.Add(month);
                    SetCurrentValuesForParameterField(parameterFields, arrayList0b, "Month");

                    ArrayList arrayList0c = new ArrayList();
                    arrayList0c.Add(day);
                    SetCurrentValuesForParameterField(parameterFields, arrayList0c, "Day");

                    ArrayList arrayList0d = new ArrayList();
                    arrayList0d.Add(Convert.ToDouble(Amount));
                    // Updated By Ken, 20160822, begin
                    // SetCurrentValuesForParameterField_Number(parameterFields, arrayList0d, "Amount");
                     SetCurrentValuesForParameterField_Double(parameterFields, arrayList0d, "Amount");
                    // Updated By Ken, 20160822, end
                    ArrayList arrayList1 = new ArrayList();
                    arrayList1.Add(Payee);
                    SetCurrentValuesForParameterField(parameterFields, arrayList1, "Payee");


                    ArrayList arrayList2a = new ArrayList();
                    if (Amount.Length - 2 == -1)
                        arrayList2a.Add("¥");
                    else
                        arrayList2a.Add(one);
                    SetCurrentValuesForParameterField(parameterFields, arrayList2a, "one");

                    ArrayList arrayList2b = new ArrayList();
                    if (Amount.Length - 3 == -1)
                        arrayList2b.Add("¥");
                    else
                        arrayList2b.Add(two);
                    SetCurrentValuesForParameterField(parameterFields, arrayList2b, "two");

                    ArrayList arrayList2c = new ArrayList();
                    if (Amount.Length >= 4)
                        arrayList2c.Add(three);
                    else if (Amount.Length - 4 == -1)
                        arrayList2c.Add("¥");
                    else
                        arrayList2c.Add("");
                    SetCurrentValuesForParameterField(parameterFields, arrayList2c, "three");

                    ArrayList arrayList2d = new ArrayList();
                    if (Amount.Length >= 5)
                        arrayList2d.Add(four);
                    else if (Amount.Length - 5 == -1)
                        arrayList2d.Add("¥");
                    else
                        arrayList2d.Add("");
                    SetCurrentValuesForParameterField(parameterFields, arrayList2d, "four");

                    ArrayList arrayList2e = new ArrayList();
                    if (Amount.Length >= 6)
                        arrayList2e.Add(five);
                    else if (Amount.Length - 6 == -1)
                        arrayList2e.Add("¥");
                    else
                        arrayList2e.Add("");
                    SetCurrentValuesForParameterField(parameterFields, arrayList2e, "five");

                    ArrayList arrayList2f = new ArrayList();
                    if (Amount.Length >= 7)
                        arrayList2f.Add(six);
                    else if (Amount.Length - 7 == -1)
                        arrayList2f.Add("¥");
                    else
                        arrayList2f.Add("");
                    SetCurrentValuesForParameterField(parameterFields, arrayList2f, "six");

                    ArrayList arrayList2g = new ArrayList();
                    if (Amount.Length >= 8)
                        arrayList2g.Add(seven);
                    else if (Amount.Length - 8 == -1)
                        arrayList2g.Add("¥");
                    else
                        arrayList2g.Add("");
                    SetCurrentValuesForParameterField(parameterFields, arrayList2g, "seven");

                    ArrayList arrayList2h = new ArrayList();
                    if (Amount.Length >= 9)
                        arrayList2h.Add(eight);
                    else if (Amount.Length - 9 == -1)
                        arrayList2h.Add("¥");
                    else
                        arrayList2h.Add("");
                    SetCurrentValuesForParameterField(parameterFields, arrayList2h, "eight");

                    ArrayList arrayList2i = new ArrayList();
                    if (Amount.Length >= 10)
                        arrayList2i.Add(nine);
                    else if (Amount.Length - 10 == -1)
                        arrayList2i.Add("¥");
                    else
                        arrayList2i.Add("");
                    SetCurrentValuesForParameterField(parameterFields, arrayList2i, "nine");

                    ArrayList arrayList2j = new ArrayList();
                    if (Amount.Length >= 11)
                        arrayList2j.Add(ten);
                    else if (Amount.Length - 11 == -1)
                        arrayList2j.Add("¥");
                    else
                        arrayList2j.Add("");
                    SetCurrentValuesForParameterField(parameterFields, arrayList2j, "ten");

                    ArrayList arrayList2k = new ArrayList();
                    if (Amount.Length >= 12)
                        arrayList2k.Add(eleven);
                    else if (Amount.Length - 12 == -1)
                        arrayList2k.Add("¥");
                    else
                        arrayList2k.Add("");
                    SetCurrentValuesForParameterField(parameterFields, arrayList2k, "eleven");

                    ArrayList arrayList2l = new ArrayList();
                    if (Amount.Length - 13 == -1)
                        arrayList2l.Add("¥");
                    else
                        arrayList2l.Add("");
                    SetCurrentValuesForParameterField(parameterFields, arrayList2l, "twelve");

                    ArrayList arrayList3 = new ArrayList();
                    arrayList3.Add(param[3]);
                    SetCurrentValuesForParameterField(parameterFields, arrayList3, "EngAmt1");

                    ArrayList arrayList4 = new ArrayList();
                    arrayList4.Add(ChequeUsage);
                    //arrayList4.Add("買賣原料");
                    SetCurrentValuesForParameterField(parameterFields, arrayList4, "EngAmt2");

                    ArrayList arrayList5 = new ArrayList();
                    arrayList5.Add(param[2]);
                    SetCurrentValuesForParameterField_Number(parameterFields, arrayList5, "ChequeLayoutID");
                }

                
                //crystalReportViewer1.PrintReport();


                //if (objRpt != null)
                //{
                //    objRpt.Close();
                //    objRpt.Dispose();
                //}

                if (param[4] == "PRINT")
                {
                    this.Close();
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                if (objRpt != null)
                {
                    objRpt.Close();
                    objRpt.Dispose();
                }
            }
            
        }
        public string getnum_formonth(string num)
        {
            if (num.Length > 1)
            {
                return getchinesenum(num[0]) + "拾" + getchinesenum(num[1]);
            }
            else
                return getchinesenum_formonth(num[0]);
        }

        public string getnum(string num)
        {
            if (num.Length > 1)
            {
                return getchinesenum(num[0]) + "拾" + getchinesenum(num[1]);
            }
            else
                return getchinesenum(num[0]);
        }
        public string getchinesenum_formonth(char num)
        {
            switch (num)
            {
                case '0':
                    return "零";
                case '1':
                    return "零壹";
                case '2':
                    return "零貳";
                case '3':
                    return "零叁";
                case '4':
                    return "零肆";
                case '5':
                    return "零伍";
                case '6':
                    return "零陸";
                case '7':
                    return "零柒";
                case '8':
                    return "零捌";
                case '9':
                    return "零玖";
            }
            return "零";
        }
        public string getchinesenum(char num)
        {
            switch (num)
            {
                case '0':
                    return "零";
                case '1':
                    return "壹";
                case '2':
                    return "貳";
                case '3':
                    return "叁";
                case '4':
                    return "肆";
                case '5':
                    return "伍";
                case '6':
                    return "陸";
                case '7':
                    return "柒";
                case '8':
                    return "捌";
                case '9':
                    return "玖";
            }
            return "零";
        }
        //private const string PARAMETER_FIELD_NAME = "param1";
        private void SetCurrentValuesForParameterField(ParameterFields parameterFields, ArrayList arrayList, string PARAMETER_FIELD_NAME)
        {
            ParameterValues currentParameterValues = new ParameterValues();

            foreach (object submittedValue in arrayList)
            {
                ParameterDiscreteValue parameterDiscreteValue = new ParameterDiscreteValue();
                parameterDiscreteValue.Value = submittedValue.ToString();

                currentParameterValues.Add(parameterDiscreteValue);
            }

            ParameterField parameterField = parameterFields[PARAMETER_FIELD_NAME];
            parameterField.CurrentValues = currentParameterValues;
        }

        private void SetCurrentValuesForParameterField_Number(ParameterFields parameterFields, ArrayList arrayList, string PARAMETER_FIELD_NAME)
        {
            ParameterValues currentParameterValues = new ParameterValues();

            foreach (object submittedValue in arrayList)
            {
                ParameterDiscreteValue parameterDiscreteValue = new ParameterDiscreteValue();
                parameterDiscreteValue.Value = Convert.ToInt32(submittedValue);

                currentParameterValues.Add(parameterDiscreteValue);
            }

            ParameterField parameterField = parameterFields[PARAMETER_FIELD_NAME];
            parameterField.CurrentValues = currentParameterValues;
        }

        // Updated By Ken, 20160822, begin
        private void SetCurrentValuesForParameterField_Double(ParameterFields parameterFields, ArrayList arrayList, string PARAMETER_FIELD_NAME)
        {
            ParameterValues currentParameterValues = new ParameterValues();

            foreach (object submittedValue in arrayList)
            {
                ParameterDiscreteValue parameterDiscreteValue = new ParameterDiscreteValue();
                parameterDiscreteValue.Value = Convert.ToDouble(submittedValue);

                currentParameterValues.Add(parameterDiscreteValue);
            }

            ParameterField parameterField = parameterFields[PARAMETER_FIELD_NAME];
            parameterField.CurrentValues = currentParameterValues;
        }
        // Updated By Ken, 20160822, end

        private void SetDBLogonForReport(ConnectionInfo connectionInfo)
        {
            TableLogOnInfos tableLogOnInfos = crystalReportViewer1.LogOnInfo;

            foreach (TableLogOnInfo tableLogOnInfo in tableLogOnInfos)
            {
                tableLogOnInfo.ConnectionInfo = connectionInfo;
            }
        }
    }
}
