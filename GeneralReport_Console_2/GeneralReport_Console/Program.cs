using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

using System.Drawing.Printing;

namespace GeneralReport_Console
{
    /*
   internal static class NativeMethods {     
       [DllImport("kernel32.dll")]     
       internal static extern Boolean AllocConsole(); 
   }*/
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
            if (args.Length >= 10)
            {
                //MessageBox.Show(args[args.Count() - 1].ToString());

                //if (args[args.Count() - 1] == "SHOWPRINTERDIALOG")
                //{
                //MessageBox.Show("true");
                //ShowPrinterDialog = true;
                //}
                
                string report_name = args[0];
                string stored_procedure_name = args[1];
                string showTree = args[2]; // args[0]: show group tree, args[1]: show print button
                string choice = args[3];
                string parameter_name = args[4];
                string parameter_value = args[5];

                string parameter_database_ip = args[6];
                string parameter_database_name = args[7];
                string parameter_database_user = args[8];
                string parameter_database_password = args[9];
                string parameter_cheque_id = args[10];
                int zoomLvl = 100;

                if (args.Length > 11)
                    zoomLvl = Int16.Parse(args[11]);

                parameter_name = parameter_name.Replace("+", " ");
                parameter_value = parameter_value.Replace("+", " ");

                string[] parameter_name_array = parameter_name.Split('|');
                string[] parameter_value_array = parameter_value.Split('|');

                for (int i = 0; i < parameter_value_array.Count(); i++)
                {
                    if (parameter_value_array[i].ToUpper() == "NULL")
                        parameter_value_array[i] = "";
                }

                showTree = showTree.Replace("+", " ");
                string[] parameter_report_array = showTree.Split('|');
                for (int i = 0; i < parameter_report_array.Count(); i++)
                {
                    if (parameter_report_array[i].ToUpper() == "NULL")
                        parameter_report_array[i] = "";
                }

                //Console.ReadLine();
                //Console.WriteLine(report_name);
                //Console.WriteLine(stored_procedure_name);
                //Console.WriteLine(choice);
                //for (int i = 0; i < parameter_name_array.Count(); i++)
                //{
                //Console.WriteLine(parameter_name_array[i]+" : " + parameter_value_array[i]);
                //}
                //Console.ReadLine();

                //bool showGroupTree = false;
                //if (showTree.ToUpper() == "TRUE")
                //{
                //    showGroupTree = true;
                //}

                if (choice == "PREVIEW")
                {
                    Form1 view;
                    if( parameter_report_array.Count() > 1)
                        view = new Form1(report_name, parameter_name_array, parameter_value_array, Convert.ToBoolean(parameter_report_array[0]), Convert.ToBoolean(parameter_report_array[1]), parameter_database_name, parameter_database_user, parameter_database_password, zoomLvl);
                    else
                        view = new Form1(report_name, parameter_name_array, parameter_value_array, Convert.ToBoolean(parameter_report_array[0]), true, parameter_database_name, parameter_database_user, parameter_database_password,zoomLvl);
 
                    
                    view.StartPosition = FormStartPosition.CenterScreen;

                    view.ShowDialog();
                  //  Application.Run(view);
                  //  view.LoadReport();
                }

                if (choice == "PRINT")
                {
                    string Layout_Printer = "";
                    string Layout_Size = "";
                    string database_name = "";
                    string database_ip = "";
                    string _connectionString = "Data Source=" + parameter_database_ip + ";Initial Catalog=" + parameter_database_name + ";User ID=" + parameter_database_user + ";Password=" + parameter_database_password;
                    bool Chk_Print_Cheque_Program = false;

                    ReportDocument rDoc = new ReportDocument();

                    TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
                    TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                    ConnectionInfo crConnectionInfo = new ConnectionInfo();
                    Tables CrTables;

                    System.Drawing.Printing.PrintDocument pDoc = new System.Drawing.Printing.PrintDocument();
                    rDoc.Load(report_name);

                    crConnectionInfo.DatabaseName = parameter_database_name;
                    crConnectionInfo.UserID = parameter_database_user;
                    crConnectionInfo.Password = parameter_database_password;

                    //rDoc.SetDatabaseLogon(parameter_database_user, parameter_database_password);

                    CrTables = rDoc.Database.Tables;
                    foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                    {
                        crtableLogoninfo = CrTable.LogOnInfo;
                        crConnectionInfo.ServerName = crtableLogoninfo.ConnectionInfo.ServerName;
                        crtableLogoninfo.ConnectionInfo = crConnectionInfo;

                        CrTable.ApplyLogOnInfo(crtableLogoninfo);
                        CrTable.LogOnInfo.ConnectionInfo = crtableLogoninfo.ConnectionInfo;
                    }

                    for (int i=0; i<parameter_name_array.Count(); i++)
                    {
                        rDoc.SetParameterValue(parameter_name_array[i], parameter_value_array[i]);
                        if (parameter_name_array[i] == "ChequeLayoutID")
                        {
                            Chk_Print_Cheque_Program = true;
                            string queryString = "Select * from ChequeLayout where ChequeLayoutID = " + parameter_value_array[i];

                            try
                            {
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
                                                    Layout_Printer = datareader["Layout_Printer"].ToString();
                                                    Layout_Size = datareader["Layout_Size"].ToString();
                                                }
                                            }
                                        }
                                        datareader.Close();
                                    }
                                    connection.Close();
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }

                    // add begin to replace below coding
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
                        //Console.WriteLine("RawKind " + rawKind);
                        rDoc.PrintOptions.PaperSize = (CrystalDecisions.Shared.PaperSize)rawKind;
                    }
                    rDoc.PrintToPrinter(1, false, 1, 1);// Print 1
                    if ((!String.IsNullOrEmpty(parameter_cheque_id)) || (Chk_Print_Cheque_Program == true))
                    {
                        updatedatabase(parameter_cheque_id, _connectionString);
                    }
                    // add end to replace below coding

                    //bool default_printer_exist = true;
                    //if (DefaultPrinterName() != Layout_Printer)
                    //{
                    //    default_printer_exist = false;
                    //}
                    //if (default_printer_exist == true)
                    //{
                    //    pDoc.PrinterSettings.PrinterName = Layout_Printer;
                    //    int rawKind = 0;
                    //    for (int i = 0; i <= pDoc.PrinterSettings.PaperSizes.Count - 1; i++)
                    //    {
                    //        if (pDoc.PrinterSettings.PaperSizes[i].PaperName == Layout_Size)
                    //        {
                    //            rawKind = pDoc.PrinterSettings.PaperSizes[i].RawKind; //設定中斷點檢查，的確有進入 
                    //        }
                    //    }
                    //    rDoc.PrintOptions.PrinterName = Layout_Printer;//加上此行即可
                    //    if (rawKind != 0)
                    //    {
                    //        rDoc.PrintOptions.PaperSize = (CrystalDecisions.Shared.PaperSize)rawKind;
                    //    }
                    //}
                    //if (ShowPrinterDialog == true || default_printer_exist == false)
                    //{
                    //    PrintDialog pd = new PrintDialog();
                    //    bool printer_exist = false;
                    //    if (default_printer_exist == false)
                    //    {
                    //        foreach (string strPrinter in PrinterSettings.InstalledPrinters)
                    //        {   // Check existing printer but not default
                    //            if (strPrinter == Layout_Printer)
                    //            {
                    //                Console.WriteLine("printer exist");
                    //                printer_exist = true;
                    //                pd.PrinterSettings.PrinterName = Layout_Printer;
                    //            }
                    //        }
                    //    }
                    //    if (printer_exist == true)
                    //    {
                    //        pDoc.PrinterSettings.PrinterName = Layout_Printer;
                    //        int rawKind = 0;
                    //        for (int i = 0; i <= pDoc.PrinterSettings.PaperSizes.Count - 1; i++)
                    //        {
                    //            if (pDoc.PrinterSettings.PaperSizes[i].PaperName == Layout_Size)
                    //            {
                    //                rawKind = pDoc.PrinterSettings.PaperSizes[i].RawKind; //設定中斷點檢查，的確有進入 
                    //            }
                    //        }
                    //        rDoc.PrintOptions.PrinterName = Layout_Printer;//加上此行即可
                    //        if (rawKind != 0)
                    //        {
                    //            rDoc.PrintOptions.PaperSize = (CrystalDecisions.Shared.PaperSize)rawKind;
                    //        }
                    //        rDoc.PrintToPrinter(1, false, 1, 1);// Print 1
                    //        if ((!String.IsNullOrEmpty(parameter_cheque_id)) || (Chk_Print_Cheque_Program == true))
                    //        {
                    //            updatedatabase(parameter_cheque_id, _connectionString);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (default_printer_exist == true)
                    //        {
                    //            for (int i = 0; i <= pDoc.PrinterSettings.PaperSizes.Count - 1; i++)
                    //            {
                    //                if (pDoc.PrinterSettings.PaperSizes[i].PaperName == Layout_Size)
                    //                {
                    //                    pd.PrinterSettings.DefaultPageSettings.PaperSize = pDoc.PrinterSettings.PaperSizes[i]; //設定中斷點檢查，的確有進入 
                    //                }
                    //            }
                    //        }
                    //        int rawKind = 0;
                    //        for (int i = 0; i <= pDoc.PrinterSettings.PaperSizes.Count - 1; i++)
                    //        {
                    //            if (pDoc.PrinterSettings.PaperSizes[i].PaperName == Layout_Size)
                    //            {
                    //                rawKind = pDoc.PrinterSettings.PaperSizes[i].RawKind; //設定中斷點檢查，的確有進入 
                    //            }
                    //        }
                    //        //MessageBox.Show(pd.PrinterSettings.PrinterName);
                    //        pd.Document = pDoc;
                    //        DialogResult print = pd.ShowDialog();
                    //        if (print != DialogResult.Cancel)
                    //        {
                    //            rDoc.PrintOptions.PrinterName = pd.PrinterSettings.PrinterName;
                    //            if (rawKind != 0)
                    //            {
                    //                rDoc.PrintOptions.PaperSize = (CrystalDecisions.Shared.PaperSize)rawKind;
                    //            }
                    //            rDoc.PrintToPrinter(1, false, 1, 1); // Print 2
                    //            if ((!String.IsNullOrEmpty(parameter_cheque_id)) || (Chk_Print_Cheque_Program == true))
                    //            {
                    //                updatedatabase(parameter_cheque_id, _connectionString);
                    //            }
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    rDoc.PrintToPrinter(1, false, 1, 1); // Print 3
                    //    if ((!String.IsNullOrEmpty(parameter_cheque_id)) || (Chk_Print_Cheque_Program == true))
                    //    {
                    //        updatedatabase(parameter_cheque_id, _connectionString);
                    //    }
                    //}

                    rDoc.Close();
                    rDoc.Dispose();
                    pDoc.Dispose();

                    return;
                }
                //if (choice == "PRINT_TT")
                //{
                    //string Layout_Printer = "";
                    //string Layout_Size = "";
                    //string database_name = "";
                    //string database_ip = "";
                    //string _connectionString = "Data Source=" + parameter_database_ip + ";Initial Catalog=" + parameter_database_name + ";User ID=" + parameter_database_user + ";Password=" + parameter_database_password;
                    //bool Chk_Print_Cheque_Program = false;

                    //ReportDocument rDoc = new ReportDocument();
                    //System.Drawing.Printing.PrintDocument pDoc = new System.Drawing.Printing.PrintDocument();
                    //rDoc.Load(report_name);
                    //rDoc.SetDatabaseLogon(parameter_database_user, parameter_database_password);

                    //for (int i = 0; i < parameter_name_array.Count(); i++)
                    //{
                    //    rDoc.SetParameterValue(parameter_name_array[i], parameter_value_array[i]);
                    //}

                    //// add begin to replace below coding
                    //pDoc.PrinterSettings.PrinterName = "HP LaserJet P3005 UPD PCL 6";
                    //int rawKind = 0;
                    //for (int i = 0; i <= pDoc.PrinterSettings.PaperSizes.Count - 1; i++)
                    //{
                    //    if (pDoc.PrinterSettings.PaperSizes[i].PaperName == "A4")
                    //    {
                    //        rawKind = pDoc.PrinterSettings.PaperSizes[i].RawKind; //設定中斷點檢查，的確有進入 
                    //    }
                    //}

                    //rDoc.PrintOptions.PrinterName = "HP LaserJet P3005 UPD PCL 6";//加上此行即可

                    //if (rawKind != 0)
                    //{
                    //    //Console.WriteLine("RawKind " + rawKind);
                    //    rDoc.PrintOptions.PaperSize = (CrystalDecisions.Shared.PaperSize)rawKind;
                    //}
                    //rDoc.PrintToPrinter(1, false, 1, 1);// Print 1

                    //rDoc.Close();
                    //rDoc.Dispose();
                    //pDoc.Dispose();

                    //return;
                //}
            }
        }

        public static void updatedatabase(string _ChequeID, string _connectionString)
        {
            string updateString = "";

            updateString = "update Cheque set IsPrinted=1 where ChequeID=" + _ChequeID;
            updateString += " and Cheque.Status<>'Void' and Cheque.Status<>'Supersede'";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlTransaction trans = null;
                SqlCommand command = new SqlCommand();
                try
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = updateString;
                    connection.Open();
                    trans = connection.BeginTransaction();
                    command.Transaction = trans;
                    command.Connection = connection;
                    command.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    trans.Rollback();
                }
                finally
                {
                    connection.Close();
                    trans.Dispose();
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        //public static string DefaultPrinterName()
        //{
        //    string functionReturnValue = null;
        //    System.Drawing.Printing.PrinterSettings oPS = new System.Drawing.Printing.PrinterSettings();

        //    try
        //    {
        //        functionReturnValue = oPS.PrinterName;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        functionReturnValue = "";
        //    }
        //    finally
        //    {
        //        oPS = null;
        //    }
        //    return functionReturnValue;
        //}
    }
}
