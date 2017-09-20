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
using System.IO;



namespace TextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ADDataManager dataManager;

        public FileInfo OpenedFile { get; set; }

        private string openedFileContent;
        public string OpenedFileContent
        {
            get { return openedFileContent; }
            set
            {
                openedFileContent = value;
                txtEditor.Text = openedFileContent;
            }
        }

        public bool HasChanged
        {
            get
            {
                if (OpenedFileContent == txtEditor.Text)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }



        public MainWindow()
        {
            InitializeComponent();
            LogFile.Initialize("ADCT_Log.txt");

            dataManager = new ADDataManager();
            dataManager.GetCurrentUserGroups();
            dataManager.GetNotesFromCurrentUserGroups();
            dataManager.ConvertNotesToFileData();

            txtEditor.Text = $"Please use the File menu above to open a file for editing. {Environment.NewLine}" +
                             $"{Environment.NewLine}" +
                             $"If you don't have any files to open, please contact your System Administrator to gain access.";
        }


        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (HasChanged && txtEditor.IsEnabled)
            {
                var result = MessageBox.Show($"Save before closing file { OpenedFile.Name }?", "Closing file", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                {
                    Open();
                }
                else if (result == MessageBoxResult.Yes)
                {
                    Save();
                    Open();
                }
            }
            else
            {
                Open();
            }
            
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Save();
        }

        private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        public void Save()
        {
            Console.WriteLine(HasChanged);
            if (HasChanged)
            {
                Console.WriteLine("Saving!");
                try
                {
                    File.WriteAllText(OpenedFile.Path, txtEditor.Text);
                    openedFileContent = txtEditor.Text;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error during save. Please check the logfile for more detailts.", "Error:", MessageBoxButton.OK, MessageBoxImage.Error);
                    LogFile.Seperator();
                    LogFile.WriteLine($"ERROR: Could not save file { OpenedFile.Name}!");
                    LogFile.WriteLine($"ERROR: Error message - { e.Message }");
                    LogFile.Seperator();
                }
            }
        }

        public void Open()
        {
            OpenDialog openDialog = new OpenDialog()
            {
                Owner = this
            };
            openDialog.UpdateFilesList(dataManager.Files);
            openDialog.ShowDialog();
        }

        private void WpfMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (HasChanged && txtEditor.IsEnabled)
            {
                var result = MessageBox.Show($"Save before exiting?", "Exiting Application", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
                else if (result == MessageBoxResult.Yes)
                {
                    Save();
                }
            }
        }
    }
}
