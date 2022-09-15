using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using PS.Extensions;
#if !NETFRAMEWORK
using System.Runtime.InteropServices;
#endif

namespace PS.Threading
{
    public class ThreadSynchronizationContext : SynchronizationContext,
                                                IDisposable
    {
        private readonly BlockingCollection<Task> _queue;
        private readonly object _sendLocker;
        private readonly CancellationTokenSource _threadCancellationTokenSource;

        private Task _sendTask;

        public ThreadSynchronizationContext(ApartmentState apartmentState)
        {
            if (apartmentState == ApartmentState.Unknown)
            {
                throw new NotSupportedException($"{apartmentState} apartment state not supported");
            }

            _threadCancellationTokenSource = new CancellationTokenSource();
            _queue = new BlockingCollection<Task>();
            _sendLocker = new object();

            Thread = new Thread(ThreadCallback)
            {
                Name = $"ThreadSynchronizationContext ApartmentState.{apartmentState}",
                IsBackground = false
            };
            #if !NETFRAMEWORK
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                #endif
            {
                Thread.SetApartmentState(apartmentState);
            }

            Thread.Start();
        }

        public Thread Thread { get; }

        public override void Post(SendOrPostCallback d, object state)
        {
            if (d == null) throw new ArgumentNullException(nameof(d));
            if (_threadCancellationTokenSource.IsCancellationRequested) throw new ObjectDisposedException("SynchronizationContext");
            var task = new Task(() => d(state));
            task.SuppressError();
            _queue.Add(task);
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            if (d == null) throw new ArgumentNullException(nameof(d));
            if (_threadCancellationTokenSource.IsCancellationRequested) throw new ObjectDisposedException("SynchronizationContext");

            if (Thread.ManagedThreadId != Thread.CurrentThread.ManagedThreadId)
            {
                lock (_sendLocker)
                {
                    var task = new Task(() => d(state));
                    Interlocked.Exchange(ref _sendTask, task);
                    try
                    {
                        task.Wait(_threadCancellationTokenSource.Token);
                    }
                    catch (AggregateException e)
                    {
                        throw e.InnerException ?? e;
                    }
                }
            }
            else
            {
                d(state);
            }
        }

        ~ThreadSynchronizationContext()
        {
            Dispose();
        }

        public void Dispose()
        {
            lock (_threadCancellationTokenSource)
            {
                if (_threadCancellationTokenSource.IsCancellationRequested) return;
                _threadCancellationTokenSource.Cancel();
            }

            GC.SuppressFinalize(this);
        }

        private void ThreadCallback(object param)
        {
            while (!_threadCancellationTokenSource.IsCancellationRequested)
            {
                var task = Interlocked.Exchange(ref _sendTask, null);
                if (task == null) _queue.TryTake(out task, 100);
                if (task == null) continue;

                try
                {
                    task.RunSynchronously();
                }
                catch
                {
                    //Nothing
                }
            }
        }
    }
}
