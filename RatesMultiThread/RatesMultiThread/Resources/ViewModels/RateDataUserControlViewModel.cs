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
using RatesMultiThread.Resources.UserControls;


namespace RatesMultiThread.Resources.ViewModels
{

    public class RateUserDataControlViewModel : INotifyPropertyChanged
    {
        // Colors of text when data increases or decreases
        private Brush _PositiveColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#77dd77"));
        private Brush _NegativeColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ff6961"));

        #region View Model Props
        public string CurrentRate { get { return _CurrentRate; } set { OnRateChange(value); _CurrentRate = value; NotifyPropertyChanged("CurrentRate"); } }
        public APIThread.WSRateType RateType { get { return _RateType; } set { _RateType = value; NotifyPropertyChanged("RateType"); } }
        public int ProgressCount { get { return _ProgressCount; } set { _ProgressCount = value; NotifyPropertyChanged("ProgressCount"); } }
        public ObservableCollection<KeyValuePair<string, int>> RefreshTimes { get; set; }
        public KeyValuePair<string, int> SelectedRefreshTime { get { return _SelectedRefreshTime; } set { _SelectedRefreshTime = value; } }
        public string CurrencyFrom { get { return _CurrencyFrom; } set { _CurrencyFrom = value; NotifyPropertyChanged("CurrencyFrom"); OnContextChange(); } }
        public string CurrencyTo { get { return _CurrenyTo; } set { _CurrenyTo = value; NotifyPropertyChanged("CurrencyTo"); OnContextChange(); } }
        public string Symbol { get { return _Symbol; } set { _Symbol = value; NotifyPropertyChanged("Symbol"); OnContextChange(); } }
        public string ContextText { get { return _ContextText; } set { _ContextText = value; NotifyPropertyChanged("ContextText"); } }
        public Brush ContextColor { get { return _ContextColor; } set { _ContextColor = value; NotifyPropertyChanged("ContextColor"); } }
        public IEnumerable<APIThread.WSRateType> WSTypes { get { return _WSTypes; } set { _WSTypes = value; NotifyPropertyChanged("WSTypes"); } }
        public float ProgressMinimum { get { return _ProgressMinimum; } set { _ProgressMinimum = value; NotifyPropertyChanged("ProgressMinimum"); } }
        public float ProgressMaximum { get { return _ProgressMaximum; } set { _ProgressMaximum = value; NotifyPropertyChanged("ProgressMaximum"); } }


        public event PropertyChangedEventHandler PropertyChanged;

        private RateDataUserControl _MainControl;
        private string _CurrentRate;
        private APIThread.WSRateType _RateType;
        private int _ProgressCount;
        private KeyValuePair<string, int> _SelectedRefreshTime;
        private string _CurrencyFrom;
        private string _CurrenyTo;
        private string _Symbol;
        private string _ContextText;
        private Brush _ContextColor;
        private APIThread _APIThread;
        private IEnumerable<APIThread.WSRateType> _WSTypes;
        private float _ProgressMinimum;
        private float _ProgressMaximum;
        #endregion

        public RateUserDataControlViewModel(RateDataUserControl mainControl)
        {
            Initialize();
            _MainControl = mainControl;
            InitalizeDropDowns();
        }

        private void Initialize()
        {
            // Create Main Thread for webservice calls
            _APIThread = new APIThread();

            // Needed as we need to make sure threads are stopped on close.
            Window window = Application.Current.MainWindow;
            window.Closing += window_Closing;

            ContextColor = Brushes.Green;
        }

        // Make sure our threads stop when windows closes
        void window_Closing(object sender, global::System.ComponentModel.CancelEventArgs e)
        {
            if (_APIThread != null)
            {
                if ((_APIThread.SpawnedThread != null) && (_APIThread.SpawnedThread.IsAlive))
                {
                    _APIThread.Stop();
                }
            }
        }

        //Notify when proptery has changed 
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        // Populate Drop Downs
        private void InitalizeDropDowns()
        {
            WSTypes = Enum.GetValues(typeof(APIThread.WSRateType)).Cast<APIThread.WSRateType>();

            RefreshTimes = new ObservableCollection<KeyValuePair<string, int>>()
            {
               new KeyValuePair<string, int>("1 Sec" ,1000),
               new KeyValuePair<string, int>("30 Sec" ,30000),
               new KeyValuePair<string, int>("1 Min" ,60000),
               new KeyValuePair<string, int>("2 Min" ,120000),
               new KeyValuePair<string, int>("5 Min" ,300000),
               new KeyValuePair<string, int>("10 Min" ,600000),
            };
            _SelectedRefreshTime = RefreshTimes[0];
        }

        // When we change Contexts make sure the labels change
        private void OnContextChange()
        {
            if (RateType == APIThread.WSRateType.SingleExchange)
            {
                ContextText = _CurrencyFrom + " → " + _CurrenyTo;
            }
            else if (RateType == APIThread.WSRateType.SingleStock)
            {
                ContextText = _Symbol + "(Vol)";
            }
        }

        // Change color when rate changes. Red = decrease; Green = Increase
        private void OnRateChange(string newValue)
        {
            if ((RateType == APIThread.WSRateType.SingleExchange) || (RateType == APIThread.WSRateType.SingleStock))
            {
                if (_CurrentRate != null)
                {
                    if (float.Parse(newValue) > float.Parse(_CurrentRate))
                    {
                        ContextColor = _PositiveColor;
                    }
                    else if (float.Parse(newValue) < float.Parse(_CurrentRate))
                    {
                        ContextColor = _NegativeColor;
                    }
                }
                else
                {
                    ContextColor = _PositiveColor;
                }
            }
        }

        // Refresh API Thread
        public void ForceRefreshRatingThread()
        {
            InitalizeAPIThread();
            _APIThread.ForceRefresh();
        }

        // Start API Thread *We initalize to make sure it has the latest model data.
        public void StartRatingThread()
        {
            InitalizeAPIThread();
            _APIThread.Start();
        }

        // Stop API Thread
        public void StopRatingThread()
        {
            _APIThread.Stop();
        }

        private void InitalizeAPIThread()
        {
            if (ValidateFields())
            {
                _APIThread.RefreshTime = SelectedRefreshTime.Value;
                _APIThread.Parent = this;
                _APIThread.WSType = RateType;
                _APIThread.CurrencyFrom = CurrencyFrom;
                _APIThread.CurrencyTo = CurrencyTo;
                _APIThread.Symbol = Symbol;
                ProgressMinimum = 0;
                ProgressMaximum = _APIThread.RefreshTime;
            }
        }

        private bool ValidateFields()
        {
            return true;
        }
    }
}
