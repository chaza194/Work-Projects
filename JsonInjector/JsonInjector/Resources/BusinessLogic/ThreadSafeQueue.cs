using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace JsonInjector.Resources.BusinessLogic
{
    public class ThreadSafeQueue<T> : IQueueable<T>
    {
        private readonly object _Lock = 1;
        private readonly Queue<T> _Queue;

        public long Count { get { return _Queue.Count; } }

        public ThreadSafeQueue()
        {
            _Queue = new Queue<T>();
        }

        public void Enqueue(T obj)
        {
            _Queue.Enqueue(obj);
            lock (_Queue)
            {
                Monitor.PulseAll(_Queue);
            }
        }

        public T Dequeue()
        {
            while (true)
            {
                if (_Queue.Count > 0)
                {
                    return _Queue.Dequeue();
                }

                lock (_Queue)
                {
                    Monitor.Wait(_Queue);
                }
            }
        }
    }
}
