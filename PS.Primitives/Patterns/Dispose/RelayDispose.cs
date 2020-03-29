using System;

namespace PS.Patterns.Dispose
{
    public class RelayDispose : IDisposable
    {
        private readonly Action _disposeAction;

        #region Constructors

        public RelayDispose(Action disposeAction)
        {
            _disposeAction = disposeAction ?? (() => { });
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _disposeAction();
        }

        #endregion
    }
}