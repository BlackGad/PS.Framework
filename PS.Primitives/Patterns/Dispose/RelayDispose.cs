using System;

namespace PS.Patterns.Dispose
{
    public class RelayDispose : IDisposable
    {
        private readonly Action _disposeAction;

        public RelayDispose(Action disposeAction)
        {
            _disposeAction = disposeAction ?? (() => { });
        }

        public void Dispose()
        {
            _disposeAction();
        }
    }
}
