using System.Collections.Concurrent;

// Generic FixedQueue class as a custom collection to limit number of values in collection
namespace Data_Logger
{
    public class FixedQueue<T>
    {
        public ConcurrentQueue<T> q = new ConcurrentQueue<T>();
        private object lockObject = new object();

        public int Limit { get; set; }

        public void Enqueue(T obj)
        {
            q.Enqueue(obj);
            lock (lockObject)
            {
                T overflow;
                while (q.Count > Limit && q.TryDequeue(out overflow)) ;
            }
        }
    }
}
