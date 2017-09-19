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
using System.Windows.Shapes;

namespace TextEditor
{
    /// <summary>
    /// Interaction logic for OpenDialog.xaml
    /// </summary>
    public partial class OpenDialog : Window
    {
        List<FileInfo> files = new List<FileInfo>();

        public OpenDialog()
        {
            InitializeComponent();
        }

        public void UpdateFilesList(List<FileInfo> filesList)
        {
            this.files = filesList;
            lvFiles.ItemsSource = files;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
            mainWindow.txtEditor.Text = lvFiles.SelectedValue.ToString();
            this.Close();
        }

    }
}
