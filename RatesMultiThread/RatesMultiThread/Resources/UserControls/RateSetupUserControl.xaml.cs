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
using RatesMultiThread.Resources.AppCode;

namespace RatesMultiThread.Resources.UserControls
{
    /// <summary>
    /// Interaction logic for RateSetupUserControl.xaml
    /// </summary>
    public partial class RateSetupUserControl : UserControl
    {
        public string UniqueID;
        public RateSetupUserControl()
        {
            InitializeComponent();
        }

        private void cboWSType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtCurrencyFrom.Text = "";
            txtCurrencyTo.Text = "";
            txtSymbol.Text = "";

            if ((APIThread.WSRateType)cboWSType.SelectedItem == APIThread.WSRateType.SingleExchange)
            {
                grpExchange.Visibility = Visibility.Visible;
                grpStock.Visibility = Visibility.Hidden;
            }
            else if ((APIThread.WSRateType)cboWSType.SelectedItem == APIThread.WSRateType.SingleStock)
            {
                grpExchange.Visibility = Visibility.Hidden;
                grpStock.Visibility = Visibility.Visible;
            }
            else
            {
                grpExchange.Visibility = Visibility.Hidden;
                grpStock.Visibility = Visibility.Hidden;
            }
        }

        public void Remove()
        {
            try
            {
                RateUserControl self = MainWindow.WebServices.Where(x => x.Key.ViewModel.SetupWSRate == this).FirstOrDefault().Key;
                if (self != null)
                {
                    if (MainWindow.WebServices[self] != null)
                    {
                        MainWindow.WSStack.Children.Remove(self);
                        MainWindow.WebServices.Remove(self);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Could Not Remove Control : " + e.Message);
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            Remove();
        }
    }
}
