using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace TextEditor
{
    /// <summary>
    /// Interaction logic for OpenDialog.xaml
    /// </summary>
    public partial class OpenDialog : Window
    {
        List<FileInfo> files = new List<FileInfo>();
        MainWindow mainWindow = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;

        public OpenDialog()
        {
            InitializeComponent();
        }

        public void UpdateFilesList(List<FileInfo> filesList)
        {
            this.files = filesList;
            lvFiles.ItemsSource = files;
        }

        private void OpenFile()
        {
            FileInfo fileInfo = lvFiles.SelectedItem as FileInfo;
            if (File.Exists(fileInfo.Path))
            {
                try
                {
                    mainWindow.txtEditor.IsEnabled = true;
                    mainWindow.OpenedFileContent = File.ReadAllText(fileInfo.Path);
                    mainWindow.OpenedFile = fileInfo;
                    this.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error during opening of file. Please check the logfile for more detailts.", "Error:", MessageBoxButton.OK, MessageBoxImage.Error);
                    LogFile.Seperator();
                    LogFile.WriteLine($"ERROR: Could not open file { fileInfo.Name}!");
                    LogFile.WriteLine($"ERROR: Error message - { e.Message }");
                    LogFile.Seperator();
                }
                
            }
            else
            {
                MessageBox.Show($"The file { fileInfo.Name } was not found. { Environment.NewLine }Please contact your administrator.", "File not found.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }

        private void LvFiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvFiles.SelectedItem != null)
            {
                OpenFile();
            }
            
        }
    }
}
