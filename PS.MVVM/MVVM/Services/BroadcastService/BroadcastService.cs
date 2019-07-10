using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PS.Data;
using PS.Extensions;
using PS.Threading;

namespace PS.MVVM.Services
{
    public class BroadcastService : IBroadcastService,
                                    IDisposable
    {
        private readonly ThreadSynchronizationContext _enqueueSynchronizationContext;

        // ReSharper disable once CollectionNeverUpdated.Local
        private readonly ObjectsStorage<Type, List<Delegate>> _subscriptions;

        #region Constructors

        public BroadcastService()
        {
            _subscriptions = new ObjectsStorage<Type, List<Delegate>>();
            _enqueueSynchronizationContext = new ThreadSynchronizationContext(ApartmentState.MTA);
        }

        #endregion

        #region IBroadcastService Members

        public Task Broadcast(Type eventType, object args)
        {
            var subscriptions = new List<Delegate>();
            var eventSubscriptions = _subscriptions[eventType];
            lock (eventSubscriptions)
            {
                subscriptions.AddRange(eventSubscriptions);
            }

            var resultTasks = new List<Task>();
            foreach (var subscription in subscriptions)
            {
                var task = new Task(() => CallDelegate(subscription, args));
                task.SuppressError();
                resultTasks.Add(task);
            }

            try
            {
                void SendOrPostCallback(object state)
                {
                    foreach (var resultTask in resultTasks)
                    {
                        resultTask.Start();
                    }

                    try
                    {
                        Task.WhenAll(resultTasks).Wait(TimeSpan.FromSeconds(5));
                    }
                    catch (Exception)
                    {
                        //Nothing
                    }
                }

                _enqueueSynchronizationContext.Post(SendOrPostCallback, null);
            }
            catch (Exception e)
            {
                return TaskHelper.FromException(e);
            }

            return Task.WhenAll(resultTasks);
        }

        public bool Subscribe(Type eventType, Delegate subscribeAction)
        {
            CheckDelegateSignature(eventType, subscribeAction);

            var eventSubscriptions = _subscriptions[eventType];
            lock (eventSubscriptions)
            {
                if (eventSubscriptions.Contains(subscribeAction)) return false;
                eventSubscriptions.Add(subscribeAction);
            }

            return true;
        }

        public bool Unsubscribe(Type eventType, Delegate unsubscribeAction)
        {
            var eventSubscriptions = _subscriptions[eventType];
            lock (eventSubscriptions)
            {
                return eventSubscriptions.Remove(unsubscribeAction);
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _subscriptions.Clear();
            _enqueueSynchronizationContext.Dispose();
        }

        #endregion

        #region Members

        protected virtual void CallDelegate<T>(Delegate @delegate, T args)
        {
            @delegate?.DynamicInvoke(args);
        }

        private void CheckDelegateSignature(Type argumentType, Delegate @delegate)
        {
            if (argumentType == null) throw new ArgumentNullException(nameof(argumentType));
            var parameters = @delegate.Method.GetParameters();
            if (parameters.Length != 1 || parameters[0].GetType().IsAssignableFrom(argumentType))
            {
                throw new ArgumentException("Delegate signature does not correspond to argument");
            }
        }

        #endregion
    }
}