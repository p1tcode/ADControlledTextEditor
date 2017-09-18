using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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



namespace TextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ActiveDirectoryData adInfo;
        FilesData filesData;

        List<string> filesList = new List<string>();
        
        public MainWindow()
        {
            InitializeComponent();

            LogFile.Initialize("ADCT_Log.txt");

            adInfo = new ActiveDirectoryData();
            LogFile.WriteLine($"UserName: { adInfo.UserName }");
            LogFile.Seperator();

            filesData = new FilesData();

            filesData.PopulateFilesList(adInfo.GetNotesFromCurrentUserGroups(adInfo.GetCurrentUserGroups()));
            foreach (KeyValuePair<string, string> data in filesData.FilesList)
            {
                LogFile.WriteLine($"Name: { data.Key } - Path: { data.Value }");
            }

            this.Close();
        }


        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Console.WriteLine($"Open");
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Console.WriteLine($"Save");
        }

        private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
