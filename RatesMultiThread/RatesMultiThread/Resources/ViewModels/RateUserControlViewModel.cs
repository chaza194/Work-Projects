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

namespace RatesMultiThread.Resources.ViewModels
{
    public class RateUserControlViewModel : INotifyPropertyChanged
    {
        public RateDataUserControl RateDataControl;
        public RateSetupUserControl SetupWSRate;
        public RateUserControl MainControl;

        public Visibility ShowSettings { get { return _ShowSettings; } set { _ShowSettings = value; NotifyPropertyChanged("ShowSettings"); } }
        public Visibility ShowRating { get { return _ShowRating; } set { _ShowRating = value; NotifyPropertyChanged("ShowRating"); } }

        public event PropertyChangedEventHandler PropertyChanged;

        private Visibility _ShowSettings;
        private Visibility _ShowRating;


        public RateUserControlViewModel(RateDataUserControl rateDataControl, RateSetupUserControl setupWSRate, RateUserControl mainControl)
        {
            // Reference Children
            RateDataControl = rateDataControl;
            SetupWSRate = setupWSRate;

            //Refrence Main Control
            MainControl = mainControl;

            // Assign on click events of child controls
            RateDataControl.btnEditSettings.Click += new RoutedEventHandler(OnWSRateEditSettings);
            SetupWSRate.btnSave.Click += new RoutedEventHandler(OnWSRateSettingsSave);
            SetupWSRate.btnCancel.Click += new RoutedEventHandler(OnWSRateSettingsCancel);
        }

        //Set visibility of Setup and Data Controls
        public void ChangeView(bool showSettings)
        {
            ShowSettings = !showSettings ? Visibility.Hidden : Visibility.Visible;
            ShowRating = showSettings ? Visibility.Hidden : Visibility.Visible;

            SetupWSRate.Visibility = ShowSettings;
            RateDataControl.Visibility = ShowRating;
        }

        // Edit Click on the top right of the control. Changes view and Stops thread of the data rater
        public void OnWSRateEditSettings(object sender, RoutedEventArgs e)
        {
            if (RateDataControl != null)
            {
                RateDataControl.ViewModel.StopRatingThread();
            }

            ChangeView(true);
        }

        // Saves view Model data to XML and Starts Thread with new data input on setup
        public void OnWSRateSettingsSave(object sender, RoutedEventArgs e)
        {
            RateUserDataControlViewModel viewModel = RateDataControl.ViewModel;
            MainWindow.Save(MainControl, new XMLRateUserControlData()
            {
                Type = viewModel.RateType,
                CurrencyFrom = viewModel.CurrencyFrom,
                CurrencyTo = viewModel.CurrencyTo,
                Symbol = viewModel.Symbol,
                RefreshTime = viewModel.SelectedRefreshTime.Key
            });

            if (RateDataControl != null)
            {
                RateDataControl.ViewModel.StartRatingThread();
            }

            ChangeView(false);
        }

        // Cancels setup. Currently doesnt work / visible.
        public void OnWSRateSettingsCancel(object sender, RoutedEventArgs e)
        {
            if (RateDataControl != null)
            {
                RateDataControl.ViewModel.StartRatingThread();
            }

            ChangeView(false);
        }

        //Notify when proptery has changed to bindings
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
