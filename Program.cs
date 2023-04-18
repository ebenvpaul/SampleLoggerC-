using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace logWritterApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
             bool createdNew;
             using (System.Threading.Mutex mutex = new System.Threading.Mutex(true, "LogWriterApp", out createdNew))
             {
                 if (createdNew)
                 {
                     Application.EnableVisualStyles();
                     Application.SetCompatibleTextRenderingDefault(false);
                     Application.Run(new Form1());
                 }
                 else
                 {
                     MessageBox.Show("Another instance of the application is already running.", "Log Writer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                 }
             }
        }
    }
}
