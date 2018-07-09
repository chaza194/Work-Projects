using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Threading;
using SimpleInjector;

namespace JsonInjector.Resources.BusinessLogic
{
    public class DataHandler
    {
        public static string TxtLog { get { return Log; } set { Log = value; OnLogChange?.Invoke(value); } }
        public static string Log;
        public static object lockObj = 1;

        public delegate void OnTextLogChange(string txt); 
        public static OnTextLogChange OnLogChange;

        private Container _Container;
        private DataProcessor _DataProcessor;
        private List<DataFeed> feeds;

        public DataHandler()
        {
            _Container = new Container();
            feeds = new List<DataFeed>();
        }

        public void ProcessData(List<string> directories)
        {
            TxtLog = "";

            _Container.Register(typeof(IQueueable<>), typeof(ThreadSafeQueue<>), Lifestyle.Singleton);
            _Container.Register(typeof(IDataFeed<>), typeof(StaticDataFeed<>));
            _Container.Verify();

            _DataProcessor = _Container.GetInstance<DataProcessor>();
            Task.Run((Action)_DataProcessor.StartListening);

            foreach (string dir in directories)
            {
                DataFeed newFeed = _Container.GetInstance<DataFeed>();
                feeds.Add(newFeed);
                Task.Run(()=> newFeed.Start(dir));
            }
        }

        public static void LogEvent(string str)
        {
            lock (lockObj)
            {
                string count = TxtLog == null ? "0" : TxtLog.Split('\n').Length.ToString();
                TxtLog = TxtLog + count + ": "+ (str + Environment.NewLine);
            }
        }
    }
}
