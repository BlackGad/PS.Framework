using System;
using System.Threading;
using System.Threading.Tasks;
using PS.Patterns.Dispose;

namespace PS.Threading
{
    public class MutexHelper
    {
        #region Constants

        private const int MaxDegreeOfParallelism = 10;

        #endregion

        #region Static members

        /// <summary>
        ///     Global mutex implementation.
        /// </summary>
        /// <param name="mutexId">Mutex name.</param>
        /// <param name="timeout">Time to acquire.</param>
        /// <returns>Disposable lock object. Dispose will release global lock.</returns>
        public static RelayDispose Acquire(string mutexId, TimeSpan? timeout = null)
        {
            Mutex mutex = null;
            try
            {
                mutex = new Mutex(false, mutexId);
                bool hasHandle;

                try
                {
                    hasHandle = mutex.WaitOne(timeout ?? TimeSpan.Zero, false);
                    if (hasHandle == false)
                    {
                        mutex.Dispose();
                        return null;
                    }
                }
                catch (AbandonedMutexException)
                {
                    hasHandle = true;
                }

                var closureMutex = mutex;
                return new RelayDispose(() =>
                {
                    if (closureMutex == null) return;
                    if (hasHandle) closureMutex.ReleaseMutex();
                    closureMutex.Dispose();
                    closureMutex = null;
                });
            }
            catch
            {
                mutex?.Dispose();
                return null;
            }
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
            var mutex = Acquire(mutexId, timeout);
            mutex?.Dispose();
            return mutex == null;
        }

        #endregion
    }
}