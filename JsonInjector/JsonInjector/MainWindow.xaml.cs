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
using JsonInjector.Resources.ViewModels;
using JsonInjector.Resources.UserControls;
using System.ComponentModel;

namespace JsonInjector
{
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel;

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel();
            ViewModel.MainStack = stkJsonFiles;
            this.DataContext = ViewModel;
        }

        private void btnAddNewFile_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Multiselect = true;
            var result = fileDialog.ShowDialog();
            foreach (string dir in fileDialog.FileNames)
            {
                ViewModel.AddObjectToStackPanel(ViewModel.CreateDirectoryObject(dir, ViewModel.RemoveObjectFromStackPanel));
            }
        }

        private void btnProcessJson_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ProcessJson(txtLog);
        }
    }
}
