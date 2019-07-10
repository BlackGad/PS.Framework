using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using PS.Extensions;

namespace PS.ComponentModel.Dynamic
{
    public class DynamicSubscription<TObject, THandler>
        where TObject : class
        where THandler : class
    {
        private readonly Action<TObject, THandler> _subscribeAction;
        private readonly ConditionalWeakTable<TObject, THandler> _table;
        private readonly Dictionary<int, WeakReference> _targetsRegistry;
        private readonly Action<TObject, THandler> _unsubscribeAction;

        #region Constructors

        public DynamicSubscription(Action<TObject, THandler> subscribeAction, Action<TObject, THandler> unsubscribeAction)
        {
            _subscribeAction = subscribeAction ?? throw new ArgumentNullException("subscribeAction");
            _unsubscribeAction = unsubscribeAction ?? throw new ArgumentNullException("unsubscribeAction");
            _table = new ConditionalWeakTable<TObject, THandler>();
            _targetsRegistry = new Dictionary<int, WeakReference>();
        }

        #endregion

        #region Members

        public bool Subscribe(object target, THandler handler)
        {
            var targetRef = target as TObject;
            if (targetRef == null) return false;
            Unsubscribe(target);

            _subscribeAction(targetRef, handler);
            _table.Add(targetRef, handler);
            _targetsRegistry.Add(targetRef.GetHash(), new WeakReference(targetRef));
            return true;
        }

        public bool Unsubscribe(object target)
        {
            if (target is TObject targetRef)
            {
                if (_table.TryGetValue(targetRef, out var existingHandler))
                {
                    _unsubscribeAction(targetRef, existingHandler);
                }

                _table.Remove(targetRef);
                _targetsRegistry.Remove(targetRef.GetHash());
                return true;
            }

            return false;
        }

        public void UnsubscribeAll()
        {
            var targets = _targetsRegistry.Select(r => r.Value.Target).Where(t => t != null).ToList();
            foreach (var target in targets)
            {
                Unsubscribe(target);
            }

            _targetsRegistry.Clear();
        }

        #endregion
    }
}