using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonInjector.Resources.BusinessLogic
{
    public interface IDataFeed<T>
    {
        Action<T> EnqueDataMessage { get; set; }
        void Start(string directory);
    }
}
