using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Threading;

namespace JsonInjector.Resources.BusinessLogic
{
    public class StaticDataFeed<T> : IDataFeed<T>
    {
        public Action<T> EnqueDataMessage { get; set; }

        public void Start(string directory)
        {
            List<string> data = File.ReadAllLines(directory).ToList();
            Task.Run(() =>
            {
                while (data.Count > 0)
                {
                    var msg = JsonConvert.DeserializeObject<T>(data[0]);
                    EnqueDataMessage?.Invoke(msg);
                    data.RemoveAt(0);
                    Thread.Sleep(500);
                }
                DataHandler.LogEvent("Complete");
            });
        }
    }
}
