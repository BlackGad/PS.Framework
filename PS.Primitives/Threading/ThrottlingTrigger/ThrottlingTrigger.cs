using System;

namespace PS.Threading.ThrottlingTrigger
{
    public class ThrottlingTrigger : IDisposable
    {
        public static IThrottlingTriggerConfiguration Setup()
        {
            return new ThrottlingTriggerConfiguration();
        }

        private readonly ThrottlingTriggerConfiguration _configuration;
        private readonly object _delayedActivationLocker;

        private readonly Timer _timer;

        private DateTime? _delayedActivation;
        private bool _isActive;
        private bool _wasTriggeredWhileNonActive;

        internal ThrottlingTrigger(ThrottlingTriggerConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _delayedActivationLocker = new object();
            _timer = new Timer(_ => { _configuration.Raise(this, EventArgs.Empty); });
        }

        public bool IsActive
        {
            get
            {
                lock (_delayedActivationLocker)
                {
                    if (_delayedActivation.HasValue && _delayedActivation.Value <= DateTime.UtcNow)
                    {
                        _isActive = true;
                        _delayedActivation = null;
                    }
                }

                return _isActive;
            }
            private set
            {
                lock (_delayedActivationLocker)
                {
                    _delayedActivation = null;
                }

                _isActive = value;
            }
        }

        public event EventHandler<RecoverableExceptionEventArgs> Failed
        {
            add { _timer.Failed += value; }
            remove { _timer.Failed -= value; }
        }

        public event EventHandler Triggered;

        public void Dispose()
        {
            Deactivate();
        }

        public ThrottlingTrigger Activate(TimeSpan activationDelay)
        {
            lock (_delayedActivationLocker)
            {
                _delayedActivation = DateTime.UtcNow + activationDelay;
            }

            return this;
        }

        public ThrottlingTrigger Activate()
        {
            IsActive = true;

            if (_wasTriggeredWhileNonActive)
            {
                _wasTriggeredWhileNonActive = false;
                Trigger();
            }

            return this;
        }

        public ThrottlingTrigger Deactivate()
        {
            IsActive = false;
            _timer.Stop();
            return this;
        }

        public void ExecuteImmediately()
        {
            _configuration.Raise(this, EventArgs.Empty);
        }

        public void Trigger()
        {
            if (!IsActive)
            {
                if (_configuration.TrackTriggerWhenInactive) _wasTriggeredWhileNonActive = true;
                return;
            }

            Triggered?.Invoke(this, EventArgs.Empty);

            _timer.Stop();
            _timer.Start(_configuration.ThrottleTime);
        }
    }
}