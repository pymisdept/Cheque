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
using System.Configuration;

namespace PrintReportConsole
{
    public partial class Form1 : Form
    {
        public Form1(string[] param)
        {
            InitializeComponent();
            
            string report_filename = param[1];

            //Console.WriteLine("hello super");
            //string _connectionString = ConfigurationManager.ConnectionStrings["ChequeConnectionString"].ConnectionString;
            string _connectionString = "Data Source=10.1.1.191;Initial Catalog=" + param[0] + ";User ID=sa;Password=fa920711;Connect Timeout=60;MultipleActiveResultSets=True";

            //Console.WriteLine(_connectionString);

            ReportDocument objRpt = new ReportDocument();
            try
            {
                string reportpath = "";
                string queryString = "Select RptLoc from System";
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
                //Console.WriteLine("hello0");

                //string reportpath = @"D:\Data\My Documents\Visual Studio 2008\Projects\CrystalReport_Console\CrystalReport_Console\Reports\" + report_filename;
                reportpath = reportpath + report_filename;

                //Console.WriteLine("hello1");
                    crystalReportViewer1.ReportSource = reportpath;
                    crystalReportViewer1.ShowPrintButton = true;
                    //Console.WriteLine("hello2");

                    ConnectionInfo connectionInfo = new ConnectionInfo();
                    connectionInfo.DatabaseName = param[0];
                    connectionInfo.UserID = "sa";
                    connectionInfo.Password = "fa920711";

                    SetDBLogonForReport(connectionInfo);

                    //Console.WriteLine("hello3");

                    ParameterFields parameterFields = crystalReportViewer1.ParameterFieldInfo;

                    if (param.Count() == 6)
                    {
                        ArrayList arrayList0 = new ArrayList();
                        arrayList0.Add(param[2]);
                        SetCurrentValuesForParameterField(parameterFields, arrayList0, "UserID");

                        ArrayList arrayList1 = new ArrayList();
                        arrayList1.Add(param[3]);
                        SetCurrentValuesForParameterField(parameterFields, arrayList1, "fm_CoCode");

                        ArrayList arrayList2 = new ArrayList();
                        arrayList2.Add(param[3]);
                        SetCurrentValuesForParameterField(parameterFields, arrayList2, "to_CoCode");

                        ArrayList arrayList3 = new ArrayList();
                        arrayList3.Add(param[4]);
                        SetCurrentValuesForParameterField(parameterFields, arrayList3, "fm_BankACNo");

                        ArrayList arrayList4 = new ArrayList();
                        arrayList4.Add(param[4]);
                        SetCurrentValuesForParameterField(parameterFields, arrayList4, "to_BankACNo");

                        ArrayList arrayList5 = new ArrayList();
                        arrayList5.Add(param[5]);
                        SetCurrentValuesForParameterField(parameterFields, arrayList5, "fm_BatchNo");
                        //SetCurrentValuesForParameterField_Number(parameterFields, arrayList5, "to_BatchNo");

                        ArrayList arrayList6 = new ArrayList();
                        arrayList6.Add(param[5]);
                        SetCurrentValuesForParameterField(parameterFields, arrayList6, "to_BatchNo");
                        //SetCurrentValuesForParameterField_Number(parameterFields, arrayList6, "to_BatchNo");
                    }
                    else
                    {
                        ArrayList arrayList0 = new ArrayList();
                        arrayList0.Add(param[2]);
                        SetCurrentValuesForParameterField(parameterFields, arrayList0, "UserID");

                        ArrayList arrayList1 = new ArrayList();
                        arrayList1.Add(param[3]);
                        SetCurrentValuesForParameterField(parameterFields, arrayList1, "fm_CoCode");

                        ArrayList arrayList2 = new ArrayList();
                        arrayList2.Add(param[4]);
                        SetCurrentValuesForParameterField(parameterFields, arrayList2, "to_CoCode");

                        ArrayList arrayList3 = new ArrayList();
                        arrayList3.Add(param[5]);
                        SetCurrentValuesForParameterField(parameterFields, arrayList3, "fm_BankACNo");

                        ArrayList arrayList4 = new ArrayList();
                        arrayList4.Add(param[6]);
                        SetCurrentValuesForParameterField(parameterFields, arrayList4, "to_BankACNo");

                        ArrayList arrayList5 = new ArrayList();
                        arrayList5.Add(param[7]);
                        SetCurrentValuesForParameterField(parameterFields, arrayList5, "fm_BatchNo");
                        //SetCurrentValuesForParameterField_Number(parameterFields, arrayList5, "to_BatchNo");

                        ArrayList arrayList6 = new ArrayList();
                        arrayList6.Add(param[8]);
                        SetCurrentValuesForParameterField(parameterFields, arrayList6, "to_BatchNo");
                    }
                if (objRpt != null)
                {
                    objRpt.Close();
                    objRpt.Dispose();
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
