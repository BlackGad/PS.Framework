using System;
using System.Threading;
using System.Threading.Tasks;
using PS.Patterns.Dispose;
using PS.Threading.Queue;

namespace PS.Threading
{
    public class MutexHelper
    {
        #region Constants

        private const int MaxDegreeOfParallelism = 10;
        private static readonly IAsyncQueue<object, IDisposable> MutexProcessorQueue;

        #endregion

        #region Static members

        /// <summary>
        ///     Global mutex implementation.
        /// </summary>
        /// <param name="mutexId">Mutex name.</param>
        /// <param name="timeout">Time to acquire.</param>
        /// <returns>Disposable lock object. Dispose will release global lock.</returns>
        public static IDisposable Acquire(string mutexId, TimeSpan? timeout = null)
        {
            var task = MutexProcessorQueue.Enqueue(new RequestAcquire(mutexId, timeout));
            return task.Result;
        }

        public static Task<IDisposable> GlobalReadLockAsync(string id)
        {
            var releaseSemaphoreEvent = new ManualResetEvent(false);
            Task.Run(() =>
            {
                var semaphore = new Semaphore(MaxDegreeOfParallelism, MaxDegreeOfParallelism, "Semaphore" + id.GetHashCode());

                using (var mutex = Acquire("Mutex" + id.GetHashCode(), TimeSpan.FromSeconds(30)))
                {
                    if (mutex == null) return;
                    semaphore.WaitOne();
                }

                releaseSemaphoreEvent.WaitOne();
                semaphore.Release();
            });

            return Task.FromResult((IDisposable)new RelayDispose(() => releaseSemaphoreEvent.Set()));
        }

        public static Task<IDisposable> GlobalWriteLockAsync(string id)
        {
            var releaseSemaphoreEvent = new ManualResetEvent(false);
            Task.Run(() =>
            {
                var semaphore = new Semaphore(MaxDegreeOfParallelism, MaxDegreeOfParallelism, "Semaphore" + id.GetHashCode());

                using (var mutex = Acquire("Mutex" + id.GetHashCode(), TimeSpan.FromSeconds(30)))
                {
                    if (mutex == null) return;
                    for (var i = 0; i < MaxDegreeOfParallelism; i++)
                    {
                        semaphore.WaitOne();
                    }
                }

                releaseSemaphoreEvent.WaitOne();
                semaphore.Release(MaxDegreeOfParallelism);
            });

            return Task.FromResult((IDisposable)new RelayDispose(() => releaseSemaphoreEvent.Set()));
        }

        public static bool IsAcquired(string mutexId, TimeSpan? timeout = null)
        {
            var acquireRequest = new RequestAcquire(mutexId, timeout);
            var releaseRequest = acquireRequest.Execute();
            releaseRequest?.Execute();

            return releaseRequest == null;
        }

        private static IDisposable ProcessHandler(object request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            switch (request)
            {
                case RequestAcquire acquireRequest:
                {
                    var releaseRequest = acquireRequest.Execute();
                    if (releaseRequest != null)
                    {
                        return new RelayDispose(() =>
                        {
                            MutexProcessorQueue.Enqueue(releaseRequest, cancellationToken)
                                               .Wait(cancellationToken);
                        });
                    }

                    break;
                }
                case RequestRelease releaseRequest:
                    releaseRequest.Execute();
                    break;
            }

            return null;
        }

        #endregion

        #region Constructors

        static MutexHelper()
        {
            MutexProcessorQueue = AsyncQueue.Setup<object, IDisposable>(ProcessHandler)
                                            .Create()
                                            .Activate();
        }

        #endregion

        #region Nested type: RequestAcquire

        private class RequestAcquire
        {
            private readonly string _id;
            private readonly TimeSpan? _timeout;

            #region Constructors

            public RequestAcquire(string id, TimeSpan? timeout)
            {
                _id = id ?? throw new ArgumentNullException(nameof(id));
                _timeout = timeout;
            }

            #endregion

            #region Members

            public RequestRelease Execute()
            {
                Mutex mutex = null;
                try
                {
                    mutex = new Mutex(false, _id);
                    try
                    {
                        if (!mutex.WaitOne(_timeout ?? TimeSpan.Zero, false))
                        {
                            mutex.Dispose();
                            return null;
                        }
                    }
                    catch (AbandonedMutexException)
                    {
                        //Nothing
                    }

                    return new RequestRelease(mutex);
                }
                catch
                {
                    mutex?.Dispose();
                    return null;
                }
            }

            #endregion
        }

        #endregion

        #region Nested type: RequestRelease

        private class RequestRelease
        {
            private readonly Mutex _mutex;

            #region Constructors

            public RequestRelease(Mutex mutex)
            {
                _mutex = mutex ?? throw new ArgumentNullException(nameof(mutex));
            }

            #endregion

            #region Members

            public void Execute()
            {
                _mutex.ReleaseMutex();
                _mutex.Dispose();
            }

            #endregion
        }

        #endregion
    }
}