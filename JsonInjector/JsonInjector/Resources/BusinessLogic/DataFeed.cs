using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonInjector.Resources.BusinessLogic
{
    public class DataFeed
    {
        private IQueueable<DataMessage> _Queue;
        private IDataFeed<DataMessage> _Feed;

        public DataFeed(IQueueable<DataMessage> dataQueue, IDataFeed<DataMessage> feed)
        {
            _Queue = dataQueue;
            _Feed = feed;

            _Feed.EnqueDataMessage = processDataMessage;
        }

        private void processDataMessage(DataMessage msg)
        {
            if (msg.GetType() == typeof(DataMessage))
            {
                _Queue.Enqueue(msg);
            }
        }

        public void Start(string directory)
        {
            _Feed.Start(directory);
        }
    }
}
