using System;
using System.Collections.Concurrent;
using System.Threading;

namespace CancelKeyPressBug
{
    class SingleThreadExecutor
    {
        private BlockingCollection<Func<object>> _tasks;
        private BlockingCollection<object> _results;
        private readonly Thread _thread;

        public SingleThreadExecutor()
        {
            _tasks = new BlockingCollection<Func<object>>();
            _results = new BlockingCollection<object>();
            _thread = new Thread(ExecutionLoop);
            _thread.IsBackground = true;
            _thread.Start();
        }

        private void ExecutionLoop(object obj)
        {
            while (true)
            {
                var task = _tasks.Take();
                var result = task();
                _results.Add(result);
            }
        }

        public T Execute<T>(Func<T> action)
        {
            _tasks.Add(() =>
            {
                return action();
            });
            return (T)_results.Take();
        }

        public void Execute(Action action)
        {
            _tasks.Add(() =>
            {
                action();
                return null;
            });
            _results.Take();
        }
    }

}
