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
        ADDataManager dataManager;

        

        public MainWindow()
        {
            InitializeComponent();

            LogFile.Initialize("ADCT_Log.txt");

            dataManager = new ADDataManager();
            dataManager.GetCurrentUserGroups();
            dataManager.GetNotesFromCurrentUserGroups();
            dataManager.ConvertNotesToFileData();

        }


        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenDialog openDialog = new OpenDialog();
            openDialog.UpdateFilesList(dataManager.Files);
            openDialog.Show();
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
