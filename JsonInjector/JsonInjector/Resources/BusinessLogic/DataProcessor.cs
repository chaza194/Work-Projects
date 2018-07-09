using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonInjector.Resources.BusinessLogic
{
    public class DataProcessor
    {
        private IQueueable<DataMessage> _DataFeed;

        public DataProcessor(IQueueable<DataMessage> dataFeed)
        {
            _DataFeed = dataFeed;
        }

        public void StartListening()
        {
            while (true)
            {               
                DataMessage msg = _DataFeed.Dequeue();
                //Use data
                DataHandler.LogEvent("Processed");
            }
        }
    }
}
