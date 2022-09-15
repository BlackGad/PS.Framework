using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PS.Threading
{
    public static class Async
    {
        public static void Run(Func<Task> func)
        {
            var prevCtx = SynchronizationContext.Current;

            try
            {
                var context = new SingleThreadSynchronizationContext();
                SynchronizationContext.SetSynchronizationContext(context);
                var task = func();
                task.ContinueWith(delegate { context.Complete(); }, TaskScheduler.Default);
                context.RunOnCurrentThread();
                task.GetAwaiter().GetResult();
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(prevCtx);
            }
        }

        public static T Run<T>(Func<Task<T>> func)
        {
            var prevCtx = SynchronizationContext.Current;

            try
            {
                var context = new SingleThreadSynchronizationContext();
                SynchronizationContext.SetSynchronizationContext(context);
                var task = func();
                task.ContinueWith(delegate { context.Complete(); }, TaskScheduler.Default);
                context.RunOnCurrentThread();
                return task.GetAwaiter().GetResult();
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(prevCtx);
            }
        }

        #region Nested type: SingleThreadSynchronizationContext

        private sealed class SingleThreadSynchronizationContext : SynchronizationContext

        {
            private readonly BlockingCollection<KeyValuePair<SendOrPostCallback, object>>
                _queue = new BlockingCollection<KeyValuePair<SendOrPostCallback, object>>();

            public override void Post(SendOrPostCallback d, object state)
            {
                _queue.Add(new KeyValuePair<SendOrPostCallback, object>(d, state));
            }

            public void Complete()
            {
                _queue.CompleteAdding();
            }

            public void RunOnCurrentThread()
            {
                foreach (var workItem in _queue.GetConsumingEnumerable())
                {
                    workItem.Key(workItem.Value);
                }
            }
        }

        #endregion
    }
}
