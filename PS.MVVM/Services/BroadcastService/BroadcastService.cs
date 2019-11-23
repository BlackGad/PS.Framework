using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PS.Extensions;
using PS.Threading;

namespace PS.MVVM.Services
{
    public class BroadcastService : IBroadcastService,
                                    IDisposable
    {
        private readonly ThreadSynchronizationContext _enqueueSynchronizationContext;
        private readonly Dictionary<Type, List<Delegate>> _subscriptions;

        #region Constructors

        public BroadcastService()
        {
            _subscriptions = new Dictionary<Type, List<Delegate>>();
            _enqueueSynchronizationContext = new ThreadSynchronizationContext(ApartmentState.MTA);
        }

        #endregion

        #region IBroadcastService Members

        public Task Broadcast(Type eventType, object args)
        {
            var subscriptions = new List<Delegate>();
            lock (_subscriptions)
            {
                var type = eventType;
                if (_subscriptions.ContainsKey(type)) subscriptions.AddRange(_subscriptions[type]);
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

            lock (_subscriptions)
            {
                var type = eventType;
                if (!_subscriptions.ContainsKey(type)) _subscriptions.Add(type, new List<Delegate>());
                if (_subscriptions[type].Contains(subscribeAction)) return false;
                _subscriptions[type].Add(subscribeAction);
            }

            return true;
        }

        public bool Unsubscribe(Type eventType, Delegate unsubscribeAction)
        {
            lock (_subscriptions)
            {
                var type = eventType;
                if (!_subscriptions.ContainsKey(type)) return false;
                return _subscriptions[type].Remove(unsubscribeAction);
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            lock (_subscriptions)
            {
                _subscriptions.Clear();
            }

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
            //TODO: Implement delegate signature check. It must return void and handle single argument that can handle argument type
        }

        #endregion
    }
}