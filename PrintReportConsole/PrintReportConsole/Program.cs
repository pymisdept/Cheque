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
using System.Security.Permissions;
using Microsoft.Win32;

namespace PrintReportConsole
{
    class Program
    {
        
        internal static class NativeMethods
        {
            [DllImport("kernel32.dll")]
            internal static extern Boolean AllocConsole();
        }
      

        static void Main(string[] args)
        {
            
            //NativeMethods.AllocConsole();
            //Console.WriteLine("Number of command line parameters = {0}", args.Length);
            for (int i = 0; i < args.Length; i++)
            {
                //Console.WriteLine("Arg[{0}] = [{1}]", i, args[i]);
            }

            string[] param = new string[args.Length];

            //Console.WriteLine("length " + args.Length);
            /*
            param[0] = "Cheque_Test";
            param[1] = "ChequeIssueDet_ChinaCheque.rpt";
            param[2] = "MASTER";
            param[3] = "HP1";
            param[4] = "Dong Guan Rural Commercial Bank (Dong Guan Branch) #090010190010026617";
            param[5] = "00049";
            */
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Contains("!") == true)
                    param[i] = args[i].Replace("!", " ");
                else if (args[i].Contains("NULL"))
                {
                    param[i] = "";
                }
                else
                    param[i] = args[i];
                //Console.WriteLine("param {0} :{1}", i, param[i]);
            }
            
            if (args.Length >= 1)
            {
                Form1 view = new Form1(param);
                view.TopMost = true;
                view.StartPosition = FormStartPosition.CenterScreen;
                view.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                view.Height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                Form dummy = new Form();
                view.ShowDialog(dummy);
                
            }
           
        }
    }
}
