using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
#if !NETFRAMEWORK
using System.Runtime.InteropServices;
#endif

namespace PS.Threading.Queue
{
    public static class AsyncQueue
    {
        public static IAsyncQueueConfiguration<TPayload, TResult> Setup<TPayload, TResult>(Func<TPayload, CancellationToken, TResult> processHandler)
        {
            if (processHandler == null) throw new ArgumentNullException(nameof(processHandler));
            return new AsyncQueueConfiguration<TPayload, TResult>
            {
                ProcessHandler = processHandler
            };
        }

        public static IAsyncQueueConfiguration<TPayload, object> Setup<TPayload>(Action<TPayload, CancellationToken> processHandler)
        {
            if (processHandler == null) throw new ArgumentNullException(nameof(processHandler));
            return new AsyncQueueConfiguration<TPayload, object>
            {
                ProcessHandler = (payload, token) =>
                {
                    processHandler(payload, token);
                    return null;
                }
            };
        }
    }

    internal class AsyncQueue<TPayload, TResult> : IAsyncQueue<TPayload, TResult>
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly AsyncQueueConfiguration<TPayload, TResult> _configuration;
        private readonly Thread _consumeQueueThread;
        private readonly ManualResetEvent _consumeQueueThreadFinished;
        private readonly BlockingCollection<AsyncQueueMessage> _queue;

        public AsyncQueue(AsyncQueueConfiguration<TPayload, TResult> configuration)
        {
            _configuration = configuration.Clone() ?? throw new ArgumentNullException(nameof(configuration));

            _queue = new BlockingCollection<AsyncQueueMessage>();
            _cancellationTokenSource = _configuration.CancellationToken == null
                ? new CancellationTokenSource()
                : CancellationTokenSource.CreateLinkedTokenSource(_configuration.CancellationToken.Value);

            _cancellationTokenSource.Token.Register(() =>
            {
                _queue.CompleteAdding();
                while (_queue.TryTake(out var message))
                {
                    message.Cancelled();
                }
            });

            _consumeQueueThreadFinished = new ManualResetEvent(false);
            _consumeQueueThread = new Thread(() =>
            {
                ThreadSynchronizationContext threadSynchronizationContext = null;
                try
                {
                    if (_configuration.ApartmentState == ApartmentState.STA)
                    {
                        threadSynchronizationContext = new ThreadSynchronizationContext(ApartmentState.STA);
                        SynchronizationContext.SetSynchronizationContext(threadSynchronizationContext);
                    }

                    ConsumeQueueLoop();
                }
                catch (OperationCanceledException)
                {
                    ConsumeQueueTaskCanceled();
                }
                catch (Exception e)
                {
                    ConsumeQueueTaskFailure(e);
                }
                finally
                {
                    _consumeQueueThreadFinished.Set();
                    threadSynchronizationContext?.Dispose();
                }
            });
            #if !NETFRAMEWORK
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                #endif
            {
                _consumeQueueThread.SetApartmentState(configuration.ApartmentState);
            }

            _consumeQueueThread.IsBackground = true;
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();

            try
            {
                _consumeQueueThreadFinished.WaitOne();
            }
            catch
            {
                //Nothing
            }
        }

        Task IAsyncQueue<TPayload>.Enqueue(TPayload payload, CancellationToken? token)
        {
            return Enqueue(payload, token);
        }

        public IAsyncQueue<TPayload, TResult> Activate()
        {
            if (_consumeQueueThread.ThreadState.HasFlag(ThreadState.Unstarted))
            {
                _consumeQueueThread.Start();
            }

            return this;
        }

        public Task<TResult> Enqueue(TPayload payload, CancellationToken? token = null)
        {
            var queueMessage = CreateMessage(payload, token ?? CancellationToken.None);

            if (_queue.IsAddingCompleted || !_queue.TryAdd(queueMessage))
            {
                queueMessage.Cancelled();
            }

            return queueMessage.Awaiter;
        }

        IAsyncQueue<TPayload> IAsyncQueue<TPayload>.Activate()
        {
            Activate();
            return this;
        }

        private void ConsumeQueueLoop()
        {
            while (!_queue.IsCompleted && !_cancellationTokenSource.IsCancellationRequested)
            {
                var consumeTimeout = _configuration.ConsumePeriod ?? TimeSpan.FromMilliseconds(5000);

                var messageTaken = _queue.TryTake(out var message, (int)consumeTimeout.TotalMilliseconds, _cancellationTokenSource.Token);
                if (!messageTaken && _configuration.ConsumeTimedOutHandler != null)
                {
                    var timedOutPayload = _configuration.ConsumeTimedOutHandler(_cancellationTokenSource.Token);
                    message = CreateMessage(timedOutPayload, CancellationToken.None);
                }

                if (message == null) continue;
                if (message.CancellationToken.IsCancellationRequested) continue;

                try
                {
                    var result = default(TResult);
                    if (_configuration.ProcessHandler != null)
                    {
                        var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, message.CancellationToken);
                        result = _configuration.ProcessHandler(message.Payload, linkedTokenSource.Token);
                    }

                    if (message.CancellationToken.IsCancellationRequested)
                    {
                        message.Cancelled();
                    }
                    else
                    {
                        message.Finished(result);
                    }
                }
                catch (Exception e)
                {
                    message.Exception(e);
                    if (!_configuration.SuppressMessageExceptionForwarding) throw;
                }
            }
        }

        private void ConsumeQueueTaskCanceled()
        {
            _configuration.CancellationHandler?.Invoke();
        }

        private void ConsumeQueueTaskFailure(Exception exception)
        {
            _configuration.ErrorHandler?.Invoke(exception?.GetBaseException());
        }

        private AsyncQueueMessage CreateMessage(TPayload payload, CancellationToken directCancellationToken)
        {
            return new AsyncQueueMessage(payload, directCancellationToken);
        }

        #region Nested type: AsyncQueueMessage

        private class AsyncQueueMessage
        {
            private readonly TaskCompletionSource<TResult> _taskCompletionSource;

            public AsyncQueueMessage(TPayload payload, CancellationToken cancellationToken)
            {
                Payload = payload;
                CancellationToken = cancellationToken;
                _taskCompletionSource = new TaskCompletionSource<TResult>();
            }

            public Task<TResult> Awaiter
            {
                get { return _taskCompletionSource.Task; }
            }

            public CancellationToken CancellationToken { get; }

            public TPayload Payload { get; }

            public void Cancelled()
            {
                _taskCompletionSource.TrySetCanceled();
            }

            public void Exception(Exception e)
            {
                _taskCompletionSource.TrySetException(e);
            }

            public void Finished(TResult result)
            {
                _taskCompletionSource.TrySetResult(result);
            }
        }

        #endregion
    }
}
