using System;
using System.Diagnostics.Contracts;

namespace PS.Graph.Algorithms.Observers
{
    internal struct DisposableAction : IDisposable
    {
        private Action _action;

        #region Constructors

        public DisposableAction(Action action)
        {
            Contract.Requires(action != null);
            _action = action;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            var a = _action;
            _action = null;
            a?.Invoke();
        }

        #endregion

        #region Nested type: Action

        public delegate void Action();

        #endregion
    }
}