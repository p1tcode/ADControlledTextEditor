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
        ActiveDirectoryInfo adInfo = new ActiveDirectoryInfo();
        FilesData filesData = new FilesData();

        List<string> filesList = new List<string>();

        public MainWindow()
        {
            InitializeComponent();

            filesData.PopulateFilesList(adInfo.GetNotesFromCurrentUserGroups());
            foreach (KeyValuePair<string, string> data in filesData.FilesList)
            {
                Console.WriteLine($"Name: { data.Key } - Path: { data.Value }");
            }
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
