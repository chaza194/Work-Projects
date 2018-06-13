using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;

namespace RatesMultiThread.Resources.AppCode
{
    class APIHandler
    {
        public class ExchangeRate
        {
            public string Rate { get; set; }
        }

        public class StockRate
        {
            public string Open { get; set; }
            public string High { get; set; }
            public string Low { get; set; }
            public string Close { get; set; }
            public string Volume { get; set; }
        }

        private const string _CurrencyExchangeRateURL = "https://forex.1forge.com/1.0.3/quotes";
        private string _CurrencyExchangeRateParameters = "?pairs={0}{1}&api_key=zA6PyaebUrfMSEm9zVgxYHHfqDDvak9O";

        private const string _StockRateURL = "https://www.alphavantage.co/query";
        private string _StockRateParameters = "?function=TIME_SERIES_INTRADAY&symbol={0}&interval=1min&apikey=TEYDKG568Z0MHL7H";

        public ExchangeRate GetCurrencyExchangeRate(string CurrencyFrom, string CurrencyTo)
        {
            string APIParams = string.Format(_CurrencyExchangeRateParameters, CurrencyFrom, CurrencyTo);
            dynamic newObj = ResponseToObject(_CurrencyExchangeRateURL, APIParams);

            if (newObj != null)
            {
                try
                {
                    return DynamicToWSRateCurEXRate(newObj);
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public StockRate GetStockRate(string Symbol)
        {
            string APIParams = string.Format(_StockRateParameters, Symbol);
            dynamic newObj = ResponseToObject(_StockRateURL, APIParams);

            if (newObj != null)
            {
                try
                {
                    return DynamicToWSRateStockRate(newObj);
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public dynamic ResponseToObject(string URL, string APIParams)
        {
            string data = GetAPIResponse(URL, APIParams);
            dynamic newObj = null;
            if (data != "")
            {
                try
                {
                    newObj = JsonConvert.DeserializeObject<ExpandoObject>(data, new ExpandoObjectConverter());
                }
                catch
                {

                }
            }

            return newObj;
        }

        private StockRate DynamicToWSRateStockRate(dynamic dynamicObj)
        {
            //There must be a better way....
            List<KeyValuePair<string, object>> returnedValues = ((IDictionary<string, object>)((IDictionary<string, object>)((IDictionary<string, object>)dynamicObj)["Time Series (1min)"]).FirstOrDefault().Value).ToList();
            return new StockRate()
            {
                Open = returnedValues[0].Value.ToString(),
                High = returnedValues[1].Value.ToString(),
                Low = returnedValues[2].Value.ToString(),
                Close = returnedValues[3].Value.ToString(),
                Volume = returnedValues[4].Value.ToString(),
            };
        }

        private ExchangeRate DynamicToWSRateCurEXRate(dynamic dynamicObj)
        {
            try
            {
                return new ExchangeRate() { Rate = ((IDictionary<string, object>)dynamicObj)["price"].ToString() };
            }
            catch
            {
                return null;
            }
        }

        public string GetAPIResponse(string URL, string Params)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);

                HttpResponseMessage response = client.GetAsync(Params).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    string dataObjects = response.Content.ReadAsStringAsync().Result;
                    dataObjects = dataObjects.Replace("\n", String.Empty).Replace("\t", String.Empty).Replace("\r", String.Empty).Replace("[", String.Empty).Replace("]", String.Empty);
                    return dataObjects;
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }
    }
}
