using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PS.Data;
using PS.Extensions;

namespace PS.ComponentModel
{
    public class EventHandlerSubscriptionStorage
    {
        private readonly ObjectsStorage<Type, List<Delegate>> _subscriptions;

        #region Constructors

        public EventHandlerSubscriptionStorage()
        {
            PossibleHandlerTypes = new List<Type>
            {
                typeof(EventArgs)
            };

            _subscriptions = new ObjectsStorage<Type, List<Delegate>>();
        }

        #endregion

        #region Properties

        public IList<Type> PossibleHandlerTypes { get; }

        #endregion

        #region Event handlers

        public void Raise(object sender, EventArgs args)
        {
            foreach (var handler in _subscriptions[args.GetType()])
            {
                try
                {
                    handler?.DynamicInvoke(sender, args);
                }
                catch (Exception e)
                {
                    if (Debugger.IsAttached) Debug.WriteLine(e.GetMessage());
                }
            }
        }

        #endregion

        #region Members

        public void Subscribe(Delegate handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            var parameters = handler.Method.GetParameters();

            var signatureMessage = "Invalid handler delegate signature. Expected signature: void EventHandler(object sender, EventArgs args). " +
                                   $"Args could be of {string.Join(", ", PossibleHandlerTypes.Select(t => t.Name))} types or their parents";

            if (parameters.Length != 2) throw new ArgumentException(signatureMessage);
            if (parameters[0].ParameterType != typeof(object)) throw new ArgumentException(signatureMessage);

            var argsTypes = PossibleHandlerTypes.Where(t => parameters[1].ParameterType.IsAssignableFrom(t)).ToList();
            if (!argsTypes.Any()) throw new ArgumentException(signatureMessage);

            foreach (var type in argsTypes)
            {
                _subscriptions[type].Add(handler);
            }
        }

        #endregion
    }
}