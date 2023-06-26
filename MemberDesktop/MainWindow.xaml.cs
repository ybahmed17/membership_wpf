using MemberDesktop.View;
using MemberDesktop.ViewModel;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MemberDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MemberViewModel memberViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            App.Current.Properties["DB"] = "PROD";
            HEADER.Background = new SolidColorBrush(Colors.Red);
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            memberViewModel = new MemberViewModel();
            XFrame.Navigate(new HomePage(XFrame, memberViewModel));


        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            XFrame.Navigate(new Search(XFrame, memberViewModel));

        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {

            XFrame.Navigate(new Add(XFrame, memberViewModel));
        }


        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void OnTestDBChecked(object sender, RoutedEventArgs e)
        {
            App.Current.Properties["DB"] = "TEST";
            MenuProdDB.IsChecked = false;
            HEADER.Background = new SolidColorBrush(Colors.YellowGreen);
        }

        private void OnTestDBUnchecked(object sender, RoutedEventArgs e)
        {
            App.Current.Properties["DB"] = "PROD";

        }

        private void OnProdDBChecked(object sender, RoutedEventArgs e)
        {
            App.Current.Properties["DB"] = "TEST";
            MenuTestDB.IsChecked=false;
            if (HEADER != null)
            {
                HEADER.Background = new SolidColorBrush(Colors.Red);
            }
        }

        private void OnProdDBUnchecked(object sender, RoutedEventArgs e)
        {
            App.Current.Properties["DB"] = "TEST";
        }

        async private void Backup_Click(object sender, RoutedEventArgs e)
        {
            BackupWaiting win = new BackupWaiting();
            win.ShowWaiting("Creating Backup......");
            this.IsEnabled = false;
            win.Show();
            string backupFile = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HHmm");
            await Task.Run(() =>
            {

                string batchFileWithParameter = @"C:\ICNE\DBBackup\backup.bat " + backupFile;
                string processParameters = $"/k \"{batchFileWithParameter}\"";
                Process compiler = new Process();
                compiler.StartInfo.FileName = "cmd";
                compiler.StartInfo.Arguments = processParameters;
                compiler.StartInfo.CreateNoWindow = true;
                compiler.StartInfo.RedirectStandardError = false;
                compiler.StartInfo.RedirectStandardError = false;
                compiler.Start();
                compiler.WaitForExit(5000);
                compiler.Close();
                Thread.Sleep(2000);
            });
            

            this.IsEnabled = true;
            win.Close();
            string output = ValidateBackup(backupFile);
          
            switch (output)
            {
                case "NOTCREATED":
                   
                    MessageBox.Show("Failed to create backup", "Backup",MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case "INVALIDSIZE":

                    MessageBox.Show("Backup size not as expected","Backup",MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case "SUCCESS":

                    MessageBox.Show("Backup created", "Backup", MessageBoxButton.OK, MessageBoxImage.Information);
                   
                    break;
            }

         
            

            // ProcessStartInfo psi = new ProcessStartInfo("cmd");
            // psi.Arguments = processParameters;
            // psi.UseShellExecute = false;
            // psi.RedirectStandardInput = true;
            // psi.CreateNoWindow = true;
            // //psi.WorkingDirectory = "c:\\test";

            // psi.RedirectStandardOutput = true;

            // psi.RedirectStandardError = true;


            // Process p = Process.Start(psi);
            //// p.StandardOutput.ReadToEnd();
            // //StreamReader read = p.StandardOutput;

            // //while (read.Peek() >= 0)
            // //    Console.WriteLine(read.ReadLine());

            // Console.WriteLine("Complete");
            // p.WaitForExit();
            // p.Close();
            // MessageBox.Show("DONE");










        }


        private string  ValidateBackup(string backupFile)
        {
            string output;
            string outputFilePath = @"C:\ICNE\DBBackup\" + backupFile + ".sql";
            if (!File.Exists(outputFilePath))
            {
               return "NOTCREATED";
            }
            FileInfo fileInfo = new FileInfo(outputFilePath);
            //Validate Length     
            if (fileInfo.Length / 1000 < 900)
            {
                return "INVALIDSIZE";
                
            }
            return "SUCCESS";
        }
    }
}
