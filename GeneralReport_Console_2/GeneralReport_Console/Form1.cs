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
//using System.Data;

using System.Drawing.Printing;

namespace GeneralReport_Console
{
    public partial class Form1 : Form
    {
        public Form1(string reportpath, string[] param_name_array, string[] param_value_array, bool showGroupTree, bool showPrint, string parameter_database_name, string parameter_database_user, string parameter_database_password, int zoomLvl)
        {

            InitializeComponent();

            try
            {
                ////ReportDocument objRpt = new ReportDocument();
                ReportDocument doc = new ReportDocument();

                TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
                TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                ConnectionInfo crConnectionInfo = new ConnectionInfo();
                Tables CrTables;

                doc.Load(reportpath);

                //crystalReportViewer1.ReportSource = reportpath;
                ////crystalReportViewer1.ReportSource = objRpt;
                ////crystalReportViewer1.ShowPrintButton = true;

                crConnectionInfo.DatabaseName = parameter_database_name;
                crConnectionInfo.UserID = parameter_database_user;
                crConnectionInfo.Password = parameter_database_password;
                //SetDBLogonForReport(crConnectionInfo);

                crystalReportViewer1.ReportSource = doc;
                CrTables = doc.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;

                    crConnectionInfo.ServerName = crtableLogoninfo.ConnectionInfo.ServerName;

                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                    CrTable.LogOnInfo.ConnectionInfo = crtableLogoninfo.ConnectionInfo;
                }
                crystalReportViewer1.Controls[2].Visible = showGroupTree;
                crystalReportViewer1.ShowPrintButton = showPrint;
                crystalReportViewer1.Zoom(zoomLvl);

                crystalReportViewer1.Refresh();

                ParameterFields parameterFields = crystalReportViewer1.ParameterFieldInfo;

                for (int i = 0; i < param_name_array.Count(); i++)
                {
                    {
                        ArrayList arrayList0a = new ArrayList();
                        if (!string.IsNullOrEmpty(param_value_array[i]))
                        {
                            arrayList0a.Add(param_value_array[i]);
                        }
                        else
                        {
                            arrayList0a.Add("");
                        }

                        SetCurrentValuesForParameterField(parameterFields, arrayList0a, param_name_array[i]);
                    }
                }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SetCurrentValuesForParameterField(ParameterFields parameterFields, ArrayList arrayList, string PARAMETER_FIELD_NAME)
        {
            ParameterValues currentParameterValues = new ParameterValues();

            foreach (object submittedValue in arrayList)
            {
                ParameterDiscreteValue parameterDiscreteValue = new ParameterDiscreteValue();

                if( !string.IsNullOrEmpty(submittedValue.ToString()))
                    parameterDiscreteValue.Value = submittedValue.ToString();
                else
                    parameterDiscreteValue.Value = "";

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

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.label1.Visible = true;
            crystalReportViewer1.RefreshReport();
            this.label1.Visible = false;
        }

    }
}
