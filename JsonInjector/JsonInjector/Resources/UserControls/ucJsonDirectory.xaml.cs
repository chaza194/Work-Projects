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

namespace JsonInjector.Resources.UserControls
{
    /// <summary>
    /// Interaction logic for ucJsonDirectory.xaml
    /// </summary>
    public partial class ucJsonDirectory : UserControl
    {
        public JsonDirectoryViewModel ViewModel;
        public ucJsonDirectory()
        {
            InitializeComponent();
            ViewModel = new JsonDirectoryViewModel();
            this.DataContext = ViewModel;
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RemoveDirectory(this);
        }
    }
}
