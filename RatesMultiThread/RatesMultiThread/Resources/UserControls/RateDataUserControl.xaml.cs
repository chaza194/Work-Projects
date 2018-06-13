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
using System.ComponentModel;
using System.Collections.ObjectModel;
using RatesMultiThread.Resources.AppCode;
using RatesMultiThread.Resources.ViewModels;

namespace RatesMultiThread.Resources.UserControls
{
    public partial class RateDataUserControl : UserControl
    {
        public RateUserDataControlViewModel ViewModel;

        public RateDataUserControl()
        {
            InitializeComponent();

            ViewModel = new RateUserDataControlViewModel(this);
            DataContext = ViewModel;
        }

        private void btnForceRefresh_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ForceRefreshRatingThread();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.StartRatingThread();
        }
    }
}
