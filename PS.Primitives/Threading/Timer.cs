using System;
using System.Threading;

namespace PS.Threading
{
    public class Timer : IDisposable
    {
        private readonly Action<CancellationToken> _action;
        private readonly CancellationToken? _externalToken;
        private readonly TimeSpan _period;
        private readonly System.Threading.Timer _timer;
        private CancellationTokenSource _tokenSource;

        public Timer(Action<CancellationToken> action, TimeSpan? period = null, CancellationToken? token = null)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
            _period = period ?? TimeSpan.FromMilliseconds(-1);
            _externalToken = token;
            _timer = new System.Threading.Timer(Callback);
        }

        public bool IsStarted { get; private set; }

        public event EventHandler<RecoverableExceptionEventArgs> Failed;

        public event EventHandler Started;

        public event EventHandler Stopped;

        public void Dispose()
        {
            Stop();
        }

        public bool Start(TimeSpan? delay = null)
        {
            Stop();

            lock (this)
            {
                _externalToken?.ThrowIfCancellationRequested();

                _tokenSource = CancellationTokenSource.CreateLinkedTokenSource(_externalToken ?? CancellationToken.None);
                _tokenSource.Token.Register(() => Stop());
            }

            var change = _timer.Change(delay ?? TimeSpan.Zero, TimeSpan.FromMilliseconds(-1));
            IsStarted = true;
            Started?.Invoke(this, EventArgs.Empty);
            return change;
        }

        public bool Stop()
        {
            var change = _timer.Change(Timeout.Infinite, Timeout.Infinite);
            if (!change) return false;

            lock (this)
            {
                if (_tokenSource != null)
                {
                    _tokenSource.Cancel();
                }
            }

            IsStarted = false;
            Stopped?.Invoke(this, EventArgs.Empty);

            return true;
        }

        private void Callback(object state)
        {
            var nextIteration = false;
            try
            {
                lock (this)
                {
                    if (_tokenSource.IsCancellationRequested) return;
                }

                _action(_tokenSource.Token);
                nextIteration = true;
            }
            catch (Exception e)
            {
                var handler = Failed;
                if (handler != null)
                {
                    var args = new RecoverableExceptionEventArgs(e, true);
                    handler(this, args);
                    nextIteration = args.Handled;
                }
            }

            if (nextIteration)
            {
                _timer.Change(_period, TimeSpan.FromMilliseconds(-1));
            }
            else
            {
                IsStarted = false;
                Stopped?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
