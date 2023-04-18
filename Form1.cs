using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.IO;


namespace logWritterApp
{
    public partial class Form1 : Form
    {
        private static Form1 instance = null;

        private string baseDirectory = null;
        private bool isLogging = false;
        private int logInterval = 30; // in seconds
        public Form1()
        {
            InitializeComponent();
            baseDirectory = ConfigurationManager.AppSettings["LogBaseDirectory"];
        }
        public static Form1 GetInstance()
        {
            if (instance == null)
            {
                instance = new Form1();
            }

            return instance;
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {

            if (!isLogging)
            {
                isLogging = true;
                btnStart.Enabled = false;
                btnStop.Enabled = true;

                while (isLogging)
                {
                    if (WindowState == FormWindowState.Normal)
                    {
                        WindowState = FormWindowState.Minimized;
                    }
                    await Task.Delay(logInterval * 1000); // delay in milliseconds

                    // Get the current year, month, and date
                    DateTime currentDate = DateTime.Now;
                    int year = currentDate.Year;
                    int month = currentDate.Month;
                    int day = currentDate.Day;
                   
                    string path = baseDirectory + @"\Log";
                    if (!System.IO.Directory.Exists(path))
                        System.IO.Directory.CreateDirectory(path);

                    string subFolderPath = Path.Combine(path, year.ToString());
                    if (!System.IO.Directory.Exists(subFolderPath))
                        System.IO.Directory.CreateDirectory(subFolderPath);

                    subFolderPath = Path.Combine(subFolderPath, month.ToString());
                    if (!System.IO.Directory.Exists(subFolderPath))
                        System.IO.Directory.CreateDirectory(subFolderPath);

                    subFolderPath = Path.Combine(subFolderPath, day.ToString());
                    if (!System.IO.Directory.Exists(subFolderPath))
                        System.IO.Directory.CreateDirectory(subFolderPath);


                    // Write the log message to a file in the sub-folder
                    string logFileName = string.Format("{0}", DateTime.Now.ToString("dd/MM/yyyy"));
                    string logFilePath = Path.Combine(subFolderPath, logFileName);
                    if (!System.IO.File.Exists(logFilePath))
                        System.IO.File.Create(logFilePath).Dispose();
                    using (StreamWriter writer = new StreamWriter(logFilePath, true))
                    {
                        string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                        message += "----------------------------------------------------------------------------------------------------------------------------------";
                        message += Environment.NewLine;
                        writer.WriteLine(message);
                    }

                    LogTextBox.AppendText("Log message written at " + currentDate.ToString() + Environment.NewLine);
                    if (WindowState == FormWindowState.Minimized)
                    {
                        WindowState = FormWindowState.Normal;
                    }

                    BringToFront();
                }
            }

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            isLogging = false;
            btnStart.Enabled = true;
            btnStop.Enabled = false;

        }
    }
}
