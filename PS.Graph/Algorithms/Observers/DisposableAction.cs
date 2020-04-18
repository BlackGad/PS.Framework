using System;

namespace PS.Graph.Algorithms.Observers
{
    internal struct DisposableAction : IDisposable
    {
        private Action _action;

        #region Constructors

        public DisposableAction(Action action)
        {
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