using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PS.Data;
using PS.Patterns.Aware;

namespace PS.ComponentModel
{
    public class EventHandlerSubscriptionStorage : ISubscriptionAware,
                                                   IRiseEventAware<object>,
                                                   IInvokeEventHandlerAware
    {
        private static Type CheckDelegateSignature(Delegate handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            var parameters = handler.Method.GetParameters();
            var signatureMessage = "Invalid handler delegate signature. Expected signature: void EventHandler(object sender, EventArgs args).";
            if (parameters.Length != 2) throw new ArgumentException(signatureMessage);
            if (parameters[0].ParameterType != typeof(object)) throw new ArgumentException(signatureMessage);
            return parameters[1].ParameterType;
        }

        private readonly Action<IInvokeEventHandlerAware, Delegate> _subscriptionAddedAction;
        private readonly Action<IInvokeEventHandlerAware, Delegate> _subscriptionRemovedAction;
        private readonly ObjectsStorage<Type, List<Delegate>> _subscriptions;

        public EventHandlerSubscriptionStorage(Action<IInvokeEventHandlerAware, Delegate> subscriptionAddedAction = null,
                                               Action<IInvokeEventHandlerAware, Delegate> subscriptionRemovedAction = null)
        {
            _subscriptionAddedAction = subscriptionAddedAction;
            _subscriptionRemovedAction = subscriptionRemovedAction;
            _subscriptions = new ObjectsStorage<Type, List<Delegate>>();
        }

        public void InvokeEventHandler(Delegate @delegate, object sender, object args)
        {
            if (@delegate == null) throw new ArgumentNullException(nameof(@delegate));
            if (sender == null) throw new ArgumentNullException(nameof(sender));
            if (args == null) throw new ArgumentNullException(nameof(args));

            var parameterType = CheckDelegateSignature(@delegate);
            if (!parameterType.IsInstanceOfType(args)) return;

            try
            {
                @delegate.DynamicInvoke(sender, args);
            }
            catch (Exception e)
            {
                if (Debugger.IsAttached) Debug.WriteLine(e.GetBaseException().Message);
            }
        }

        public void Raise(object sender, object args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            var argsType = args.GetType();

            var handlers = _subscriptions.Where(s => s.Key.IsAssignableFrom(argsType))
                                         .SelectMany(s => s.Value)
                                         .Distinct();

            foreach (var handler in handlers)
            {
                InvokeEventHandler(handler, sender, args);
            }
        }

        public void Subscribe<T>(EventHandler<T> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            var parameterType = CheckDelegateSignature(handler);
            _subscriptions[parameterType].Add(handler);

            _subscriptionAddedAction?.Invoke(this, handler);
        }

        public void Unsubscribe<T>(EventHandler<T> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            var parameterType = CheckDelegateSignature(handler);
            _subscriptions[parameterType].Remove(handler);
            _subscriptionRemovedAction?.Invoke(this, handler);
        }
    }
}
