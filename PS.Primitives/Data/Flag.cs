using System;
using System.Threading;
using PS.Patterns.Dispose;

namespace PS.Data
{
    public class Flag
    {
        public static implicit operator bool(Flag offset)
        {
            return offset?._event.WaitOne(0) == true;
        }

        private readonly ManualResetEvent _event;
        private readonly Action<Flag> _flagChangedAction;
        private int _counter;

        public Flag(Action<Flag> flagChangedAction = null)
        {
            _flagChangedAction = flagChangedAction;
            _event = new ManualResetEvent(false);
        }

        public IDisposable Scope()
        {
            if (Interlocked.Increment(ref _counter) == 1)
            {
                _event.Set();
                _flagChangedAction?.Invoke(this);
            }

            return new RelayDispose(() =>
            {
                if (Interlocked.Decrement(ref _counter) == 0)
                {
                    _event.Reset();
                    _flagChangedAction?.Invoke(this);
                }
            });
        }
    }
}
