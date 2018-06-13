using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using RatesMultiThread.Resources.AppCode;
using RatesMultiThread.Resources.UserControls;
using RatesMultiThread.Resources.ViewModels;

namespace RatesMultiThread.Resources.AppCode
{
    public class APIThread
    {
        public enum WSRateType {None, SingleExchange, SingleStock}
        public bool Initialized;

        //Required Vars
        public int RefreshTime;
        public WSRateType WSType;
        public RateUserDataControlViewModel Parent;

        // API VARS 
        public string CurrencyFrom;
        public string CurrencyTo;
        public string Symbol;

        public Thread SpawnedThread;

        private APIHandler _WSRateAPIHandler;
        private delegate APIHandler.ExchangeRate GetEXRates(string CurrencyFrom, string CurrencyTo);
        private GetEXRates _GetEXRates;
        private delegate APIHandler.StockRate GetSTKRates(string Symbol);
        private GetSTKRates _GetStockRates;

        private bool _RunAPICall;
        private bool _Running;
        private DateTime _LastRefresh;

        public void ForceRefresh()
        {
            Stop();
            Start();
        }

        public void Stop()
        {
            if (_Running)
            {
                try
                {
                    if ((SpawnedThread != null) && (SpawnedThread.IsAlive))
                    {
                        // Might be a bit harsh 
                        SpawnedThread.Abort();
                    }
                    //Reset Progress Bar
                    Parent.ProgressCount = 0;
                    _Running = false;
                }
                catch
                {
                    _Running = true;
                }
            }
        }

        public void Start()
        {
            if (!_Running)
            {
                try
                {
                    Initalize();
                    if ((SpawnedThread != null) && (!SpawnedThread.IsAlive))
                    {
                        SpawnedThread.Start();
                    }

                    _Running = true;
                }
                catch
                {
                    _Running = false;
                }
            }
        }

        public void RunAPIUpdates()
        {
            if (Initialized)
            {
                while (_Running)
                {
                    // Get the Time difference from last run
                    DateTime currentDate = DateTime.Now;
                    TimeSpan timeSpan = (currentDate - _LastRefresh);
                    int timeDifference = (int)timeSpan.TotalMilliseconds;

                    // Update Progress bar
                    Parent.ProgressCount = timeDifference;

                    // if its out first run (ticks will be 0) or our time difference is greater than our refresh time
                    if ((_LastRefresh.Ticks == 0) || (timeDifference > RefreshTime))
                    {
                        _RunAPICall = true;
                    }

                    // if we need to run the API again.
                    if (_RunAPICall)
                    {
                        if (WSType == WSRateType.SingleExchange)
                        {
                            if (_GetEXRates != null)
                            {
                                APIHandler.ExchangeRate newRates = _GetEXRates(CurrencyFrom, CurrencyTo);
                                if (newRates != null)
                                {
                                    Parent.CurrentRate = newRates.Rate;
                                }
                            }
                        }
                        else if (WSType == WSRateType.SingleStock)
                        {
                            if (_GetStockRates != null)
                            {
                                APIHandler.StockRate newRates = _GetStockRates(Symbol);
                                if (newRates != null)
                                {
                                    Parent.CurrentRate = newRates.Open;
                                }
                            }
                        }
                        _LastRefresh = DateTime.Now;
                        _RunAPICall = false;
                    }
                }
            }
            else
            {
                //if we are not initialized, Abort();
                SpawnedThread.Abort();
            }
        }

        private void Initalize()
        {
            try
            {
                _WSRateAPIHandler = new APIHandler();

                // Initalize Delegates
                if (WSType == WSRateType.SingleExchange)
                {
                    _GetEXRates = new GetEXRates(_WSRateAPIHandler.GetCurrencyExchangeRate);
                }
                else if (WSType == WSRateType.SingleStock)
                {
                    _GetStockRates = new GetSTKRates(_WSRateAPIHandler.GetStockRate);
                }
                SpawnedThread = new Thread(RunAPIUpdates);
                _LastRefresh = new DateTime();
                Initialized = true;
            }
            catch
            {
                Initialized = false;
            }
        }
    }
}

