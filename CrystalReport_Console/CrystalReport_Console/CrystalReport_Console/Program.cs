using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Data.SqlClient;

using System.Drawing.Printing;

namespace CrystalReport_Console
{
    internal static class NativeMethods {     
        [DllImport("kernel32.dll")]     
        internal static extern Boolean AllocConsole(); 
    }  
    static class Program 
    {
        [STAThread]
        static void Main(string[] args) 
        {
            /*
            NativeMethods.AllocConsole();
            Console.WriteLine("Number of command line parameters = {0}", args.Length);
            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine("Arg[{0}] = [{1}]", i, args[i]);
            }
            */
            bool ShowPrinterDialog = false;
            // run as windows app 
            if (args.Length >= 1)
            {
                string[] param = new string[7];
                
                //MessageBox.Show(args[args.Count() - 1].ToString());
                
                if (args[args.Count()-1] == "SHOWPRINTERDIALOG")
                {
                    //MessageBox.Show("true");
                    ShowPrinterDialog = true;
                }


                param[0] = args[0];
                param[1] = args[1];
                param[2] = args[2];
                param[3] = args[3];


                //change for pass system user, begin
                string parameter_user_id = "";
                string parameter_choice = args[4];
                string[] parameter_choice_array = parameter_choice.Split('|');

                if (parameter_choice_array.Count() >= 2)
                {
                    if (parameter_choice_array[0] == "PREVIEW" || parameter_choice_array[0] == "PRINT" || parameter_choice_array[0] == "EDITPAYEE")
                    {
                        param[4] = "";
                        parameter_choice = parameter_choice_array[0];
                    }
                    else
                    {
                        param[4] = parameter_choice_array[0];
                        parameter_choice = parameter_choice_array[0];
                    }
                    parameter_user_id = parameter_choice_array[1];
                }
                else
                {

                    if (args[4] == "PREVIEW" || args[4] == "PRINT" || args[4] == "EDITPAYEE")
                    {
                        param[4] = "";
                    }
                    else
                    {
                        param[4] = parameter_choice;
                    }
                    parameter_choice = args[4];
                }

                if (args.Length > 5)
                    param[5] = args[5];
                else
                    param[5] = parameter_choice;


                //if (args[4] == "PREVIEW" || args[4] == "PRINT" || args[4] == "EDITPAYEE")
                //    param[4] = "";
                //else
                //{
                //    param[4] = args[4];
                //}
                //if (args.Length > 5)
                //    param[5] = args[5];
                //else
                //    param[5] = args[4];
                //change for pass system user, end
                

                /*
                param[0] = "Cheque_Test";
                param[1] = "390";
                param[2] = "18";
                param[3] = "伍佰元正";
                param[4] = "PRINT";
                */
                //ShowPrinterDialog = true;
                
                for (int i = 0; i < param.Length; i++)
                {
                    Console.WriteLine("param[{0}] = [{1}]", i, param[i]);
                }

                //Console.ReadLine();
                //while (true) { }
                Application.EnableVisualStyles();

                if (param[4] == "EDITPAYEE" || param[5] == "EDITPAYEE")
                {
                    EditPayee ep = new EditPayee(param);
                    ep.TopMost = true;
                    ep.StartPosition = FormStartPosition.CenterScreen;
                    Form dummy = new Form();
                    //Application.Run(ep);
                    ep.ShowDialog(dummy);

                }
                else if (param[4] == "PREVIEW" || param[5] == "PREVIEW")
                {
                    Form1 view = new Form1(param);
                    view.TopMost = true;
                    view.StartPosition = FormStartPosition.CenterScreen;
                    //Form dummy = new Form();
                    ////Application.Run(view);
                    //view.ShowDialog(dummy);
                    view.ShowDialog();
                }

                //Console.ReadLine();
                if (param[5] == "PRINT" || param[4] == "PRINT")
                {
                    string _connectionString = "Data Source=10.1.1.191;Initial Catalog="+param[0]+";User ID=sa;Password=fa920711";
                    string queryString = "select c.*, b.ChequeDate, b.CurrentAccountID from Cheque c inner join Batch b on c.BatchID=b.BatchID";
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
                    string ChequeNo = "";
                    string bankacno = "";
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
                                    if (datareader["ChequeNo"] != System.DBNull.Value)
                                    {
                                        ChequeNo = datareader["ChequeNo"].ToString();
                                    }
                                    if (datareader["CurrentAccountID"] != System.DBNull.Value)
                                    {
                                        bankacno = datareader["CurrentAccountID"].ToString();
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
                    if (Amount.Length >= 4)
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

                    ReportDocument rDoc = new ReportDocument();


                    TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
                    TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                    ConnectionInfo crConnectionInfo = new ConnectionInfo();
                    Tables CrTables;

                    System.Drawing.Printing.PrintDocument pDoc = new System.Drawing.Printing.PrintDocument();
                    rDoc.Load(reportpath);

                    crConnectionInfo.DatabaseName = param[0];
                    crConnectionInfo.UserID = "sa";
                    crConnectionInfo.Password = "fa920711";

                    //rDoc.SetDatabaseLogon("sa", "fa920711");

                    CrTables = rDoc.Database.Tables;
                    foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                    {
                        crtableLogoninfo = CrTable.LogOnInfo;
                        crConnectionInfo.ServerName = crtableLogoninfo.ConnectionInfo.ServerName;
                        crtableLogoninfo.ConnectionInfo = crConnectionInfo;

                        CrTable.ApplyLogOnInfo(crtableLogoninfo);
                        CrTable.LogOnInfo.ConnectionInfo = crtableLogoninfo.ConnectionInfo;
                    }

                    Console.WriteLine("path=" + reportpath);
                    Console.ReadLine();
                    
                    rDoc.SetParameterValue("Year", year);
                    rDoc.SetParameterValue("Month", month);
                    rDoc.SetParameterValue("Day", day);

                    rDoc.SetParameterValue("Amount", Amount);
                    rDoc.SetParameterValue("Payee", Payee);

                    if (Amount.Length - 2 == -1)
                        rDoc.SetParameterValue("one", "¥");
                    else
                        rDoc.SetParameterValue("one", one.ToString());
                    if (Amount.Length - 3 == -1)
                        rDoc.SetParameterValue("two", "¥");
                    else
                        rDoc.SetParameterValue("two", two.ToString());

                    if (Amount.Length >= 4)
                        rDoc.SetParameterValue("three", three.ToString());
                    else if (Amount.Length - 4 == -1)
                        rDoc.SetParameterValue("three", "¥");
                    else
                        rDoc.SetParameterValue("three", "");

                    if (Amount.Length >= 5)
                        rDoc.SetParameterValue("four", four.ToString());
                    else if (Amount.Length - 5 == -1)
                        rDoc.SetParameterValue("four", "¥");
                    else
                        rDoc.SetParameterValue("four", "");

                    if (Amount.Length >= 6)
                        rDoc.SetParameterValue("five", five.ToString());
                    else if (Amount.Length - 6 == -1)
                        rDoc.SetParameterValue("five", "¥");
                    else
                        rDoc.SetParameterValue("five", "");

                    if (Amount.Length >= 7)
                        rDoc.SetParameterValue("six", six.ToString());
                    else if (Amount.Length - 7 == -1)
                        rDoc.SetParameterValue("six", "¥");
                    else
                        rDoc.SetParameterValue("six", "");

                    if (Amount.Length >= 8)
                        rDoc.SetParameterValue("seven", seven.ToString());
                    else if (Amount.Length - 8 == -1)
                        rDoc.SetParameterValue("seven", "¥");
                    else
                        rDoc.SetParameterValue("seven", "");

                    if (Amount.Length >= 9)
                        rDoc.SetParameterValue("eight", eight.ToString());
                    else if (Amount.Length - 9 == -1)
                        rDoc.SetParameterValue("eight", "¥");
                    else
                        rDoc.SetParameterValue("eight", "");

                    if (Amount.Length >= 10)
                        rDoc.SetParameterValue("nine", nine.ToString());
                    else if (Amount.Length - 10 == -1)
                        rDoc.SetParameterValue("nine", "¥");
                    else
                        rDoc.SetParameterValue("nine", "");

                    if (Amount.Length >= 11)
                        rDoc.SetParameterValue("ten", ten.ToString());
                    else if (Amount.Length - 11 == -1)
                        rDoc.SetParameterValue("ten", "¥");
                    else
                        rDoc.SetParameterValue("ten", "");

                    if (Amount.Length >= 12)
                        rDoc.SetParameterValue("eleven", eleven.ToString());
                    else if (Amount.Length - 12 == -1)
                        rDoc.SetParameterValue("eleven", "¥");
                    else
                        rDoc.SetParameterValue("eleven", "");

                    if (Amount.Length >= 13)
                        rDoc.SetParameterValue("twelve", twelve.ToString());
                    else if (Amount.Length - 13 == -1)
                        rDoc.SetParameterValue("twelve", "¥");
                    else
                        rDoc.SetParameterValue("twelve", "");
                    rDoc.SetParameterValue("EngAmt1", param[3]);
                    rDoc.SetParameterValue("EngAmt2", ChequeUsage);
                    rDoc.SetParameterValue("ChequeLayoutID", param[2]);

                    //rDoc.SetDataSource(ds.Tables[0]);
                    //Console.WriteLine(DefaultPrinterName() + "ChequeLayoutID " + param[2]);

                    bool default_printer_exist = true;
                    if( DefaultPrinterName() != Layout_Printer)
                    {
                        default_printer_exist = false;
                        //MessageBox.Show("No default printer HP1100");
                    }
                    if (default_printer_exist == true)
                    {
                        pDoc.PrinterSettings.PrinterName = Layout_Printer;
                        int rawKind = 0;
                        for (int i = 0; i <= pDoc.PrinterSettings.PaperSizes.Count - 1; i++)
                        {
                            if (pDoc.PrinterSettings.PaperSizes[i].PaperName == Layout_Size)
                            {
                                rawKind = pDoc.PrinterSettings.PaperSizes[i].RawKind; //設定中斷點檢查，的確有進入 
                            }
                        }
                        try
                        {
                        rDoc.PrintOptions.PrinterName = Layout_Printer;//加上此行即可
                        }
                        catch
                        {
                            PrintDialog dialog1 = new PrintDialog();
                            dialog1.AllowSomePages = true;
                            dialog1.AllowPrintToFile = false;

                            if (dialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                int copies = dialog1.PrinterSettings.Copies;
                                int fromPage = dialog1.PrinterSettings.FromPage;
                                int toPage = dialog1.PrinterSettings.ToPage;
                                bool collate = dialog1.PrinterSettings.Collate;

                                rDoc.PrintOptions.PrinterName = dialog1.PrinterSettings.PrinterName;
                            }
                            else
                            {
                                rDoc.PrintOptions.PrinterName = "";
                            }
                        }
                        if (rawKind != 0)
                        {
                            rDoc.PrintOptions.PaperSize = (CrystalDecisions.Shared.PaperSize)rawKind;
                        }
                    }
                    if (ShowPrinterDialog == true || default_printer_exist == false)
                    {
                        PrintDialog pd = new PrintDialog();

                        bool printer_exist = false;
                        if (default_printer_exist == false)
                        {
                            foreach (string strPrinter in PrinterSettings.InstalledPrinters)
                            {   // Check existing printer but not default
                                if (strPrinter == Layout_Printer)
                                {
                                    Console.WriteLine("printer exist");
                                    printer_exist = true;
                                    pd.PrinterSettings.PrinterName = Layout_Printer;
                                }
                            }
                        }
                        if (printer_exist == true)
                        {
                            pDoc.PrinterSettings.PrinterName = Layout_Printer;
                            int rawKind = 0;
                            for (int i = 0; i <= pDoc.PrinterSettings.PaperSizes.Count - 1; i++)
                            {
                                if (pDoc.PrinterSettings.PaperSizes[i].PaperName == Layout_Size)
                                {
                                    rawKind = pDoc.PrinterSettings.PaperSizes[i].RawKind; //設定中斷點檢查，的確有進入 
                                }
                            }
                            try
                            {
                            rDoc.PrintOptions.PrinterName = Layout_Printer;//加上此行即可
                            }
                            catch
                            {
                                PrintDialog dialog1 = new PrintDialog();
                                dialog1.AllowSomePages = true;
                                dialog1.AllowPrintToFile = false;

                                if (dialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                {
                                    int copies = dialog1.PrinterSettings.Copies;
                                    int fromPage = dialog1.PrinterSettings.FromPage;
                                    int toPage = dialog1.PrinterSettings.ToPage;
                                    bool collate = dialog1.PrinterSettings.Collate;

                                    rDoc.PrintOptions.PrinterName = dialog1.PrinterSettings.PrinterName;
                                }
                                else
                                {
                                    rDoc.PrintOptions.PrinterName = "";
                                }
                            }
                            if (rawKind != 0)
                            {
                                rDoc.PrintOptions.PaperSize = (CrystalDecisions.Shared.PaperSize)rawKind;
                            }
                            //Console.WriteLine("Layout_Printer="+Layout_Printer);
                            //Console.WriteLine("(CrystalDecisions.Shared.PaperSize)rawKind=" + (CrystalDecisions.Shared.PaperSize)rawKind);
                            //Console.ReadLine();

                            try
                            {
                                rDoc.PrintToPrinter(1, false, 1, 1);// Print 1
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            updatedatabase(ChequeNo, bankacno, _connectionString, parameter_user_id);
                        }
                        else
                        {
                            if (default_printer_exist == true)
                            {
                                for (int i = 0; i <= pDoc.PrinterSettings.PaperSizes.Count - 1; i++)
                                {
                                    if (pDoc.PrinterSettings.PaperSizes[i].PaperName == Layout_Size)
                                    {
                                        pd.PrinterSettings.DefaultPageSettings.PaperSize = pDoc.PrinterSettings.PaperSizes[i]; //設定中斷點檢查，的確有進入 
                                    }
                                }
                            }

                            int rawKind = 0;
                            for (int i = 0; i <= pDoc.PrinterSettings.PaperSizes.Count - 1; i++)
                            {
                                if (pDoc.PrinterSettings.PaperSizes[i].PaperName == Layout_Size)
                                {
                                    rawKind = pDoc.PrinterSettings.PaperSizes[i].RawKind; //設定中斷點檢查，的確有進入 
                                }
                            }
                            //MessageBox.Show(pd.PrinterSettings.PrinterName);
                            pd.Document = pDoc;
                            DialogResult print = pd.ShowDialog();

                            if (print != DialogResult.Cancel)
                            {
                                try
                                {
                                rDoc.PrintOptions.PrinterName = pd.PrinterSettings.PrinterName;
                                }
                                catch
                                {
                                    PrintDialog dialog1 = new PrintDialog();
                                    dialog1.AllowSomePages = true;
                                    dialog1.AllowPrintToFile = false;

                                    if (dialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                    {
                                        int copies = dialog1.PrinterSettings.Copies;
                                        int fromPage = dialog1.PrinterSettings.FromPage;
                                        int toPage = dialog1.PrinterSettings.ToPage;
                                        bool collate = dialog1.PrinterSettings.Collate;

                                        rDoc.PrintOptions.PrinterName = dialog1.PrinterSettings.PrinterName;
                                    }
                                    else
                                    {
                                        rDoc.PrintOptions.PrinterName = "";
                                    }
                                }
                                if (rawKind != 0)
                                {
                                    rDoc.PrintOptions.PaperSize = (CrystalDecisions.Shared.PaperSize)rawKind;
                                }
                                rDoc.PrintToPrinter(1, false, 1, 1); // Print 2
                                updatedatabase(ChequeNo, bankacno, _connectionString, parameter_user_id);
                            }
                        }
                    }
                    else
                    {
                        rDoc.PrintToPrinter(1, false, 1, 1); // Print 3
                        updatedatabase(ChequeNo, bankacno, _connectionString, parameter_user_id);
                    }

                    rDoc.Close();
                    rDoc.Dispose();
                    pDoc.Dispose();

                    return;
                }
            }
        }
        public static void updatedatabase(string ChequeNo, string bankacno, string _connectionString, string _UserID)
        {
            string updateString = "";

            //change for pass system user, begin
            updateString = "update Cheque set IsPrinted=1, Last_Updated_Date = getdate(), Last_Updated_User = '" + _UserID + "' from Cheque c inner join Batch b on c.BatchID=b.BatchID where c.ChequeNo='" + ChequeNo;
            //change for pass system user, end
            updateString += "' and b.CurrentAccountID=" + bankacno + " and c.Status<>'Void' and c.Status<>'Supersede'";
            //Forms("PrintCheque_CR_RDC").ChequeNo
            //& CStr(Forms("PrintCheque_CR_RDC").bankacno) &
            UtilTools.ExecuteDatabase(updateString, _connectionString);
        }
        public static string DefaultPrinterName() 
        { 
            string functionReturnValue = null;
            System.Drawing.Printing.PrinterSettings oPS = new System.Drawing.Printing.PrinterSettings();
            
            try { 
                functionReturnValue = oPS.PrinterName; 
            } 
            catch (System.Exception ex) { 
                functionReturnValue = ""; 
            } 
            finally { 
                oPS = null; 
            } 
            return functionReturnValue; 
        }

        public static string getnum_formonth(string num)
        {
            if (num.Length > 1)
            {
                return getchinesenum(num[0]) + "拾" + getchinesenum(num[1]);
            }
            else
                return getchinesenum_formonth(num[0]);
        }

        public static string getnum(string num)
        {
            if (num.Length > 1)
            {
                return getchinesenum(num[0]) + "拾" + getchinesenum(num[1]);
            }
            else
                return getchinesenum(num[0]);
        }
        public static string getchinesenum_formonth(char num)
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
        public static string getchinesenum(char num)
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
    } 

    /*
    internal static class NativeMethods 
    { 
        [DllImport("kernel32.dll")]     
        internal static extern Boolean AllocConsole(); 
    } 
    
    static class Program
    {
        static void Main(string[] args)
        {
            protected ReportDocument objRpt = null;

            //NativeMethods.AllocConsole();
            Console.WriteLine("Number of command line parameters = {0}", args.Length);
            for(int i = 0; i < args.Length; i++)
            {
                Console.WriteLine("Arg[{0}] = [{1}]", i, args[i]);
            }
            Application.EnableVisualStyles(); 
            Application.Run(new Form1()); // or whatever 
            
            crystalReportViewer1.HasToggleGroupTreeButton = false;
            crystalReportViewer1.HasToggleParameterPanelButton = false;
            crystalReportViewer1.HasPrintButton = true;
            crystalReportViewer1.HasDrilldownTabs = false;
            crystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            crystalReportViewer1.PrintMode = CrystalDecisions.Web.PrintMode.ActiveX;

            if (this.objRpt != null)
            {
                this.objRpt.Close();
                this.objRpt.Dispose();
            }
        }
    }*/
}
