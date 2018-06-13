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
using RatesMultiThread.Resources.UserControls;
using RatesMultiThread.Resources.AppCode;
using RatesMultiThread.Resources.ViewModels;

namespace RatesMultiThread.Resources.UserControls
{
    public partial class RateUserControl : UserControl
    {
        public RateUserControlViewModel ViewModel;

        public RateUserControl()
        {
            InitializeComponent();
            ViewModel = new RateUserControlViewModel(Rater, RaterSetup, this);
            DataContext = ViewModel;
            ViewModel.ChangeView(false);

            // Make sure the Setup Datacontext is the same as rate for easy Data Flow;
            RaterSetup.DataContext = Rater.DataContext;
        }
    }
}
