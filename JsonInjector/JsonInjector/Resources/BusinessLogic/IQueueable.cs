using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonInjector.Resources.BusinessLogic
{
    public interface IQueueable<T>
    {
        T Dequeue();
        void Enqueue(T obj);
    }
}
