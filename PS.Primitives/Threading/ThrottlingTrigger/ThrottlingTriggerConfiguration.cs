using System;
using System.Threading;
using PS.ComponentModel;

namespace PS.Threading.ThrottlingTrigger
{
    internal class ThrottlingTriggerConfiguration : IThrottlingTriggerConfiguration
    {
        private readonly EventHandlerSubscriptionStorage _subscriptionStorage;
        private SynchronizationContext _context;

        private bool _isCreated;

        public ThrottlingTriggerConfiguration()
        {
            _subscriptionStorage = new EventHandlerSubscriptionStorage();
        }

        public TimeSpan ThrottleTime { get; private set; }

        public bool TrackTriggerWhenInactive { get; private set; }

        public ThrottlingTrigger Create()
        {
            _isCreated = true;
            return new ThrottlingTrigger(this);
        }

        public IThrottlingTriggerConfiguration Subscribe<T>(EventHandler<T> handler)
        {
            CheckCreated();
            _subscriptionStorage.Subscribe(handler);
            return this;
        }

        public IThrottlingTriggerConfiguration DispatchWith(SynchronizationContext context)
        {
            CheckCreated();
            _context = context;
            return this;
        }

        public IThrottlingTriggerConfiguration TrackWhenInactive(bool track)
        {
            CheckCreated();
            TrackTriggerWhenInactive = track;
            return this;
        }

        public IThrottlingTriggerConfiguration Throttle(TimeSpan time)
        {
            CheckCreated();
            ThrottleTime = time;
            return this;
        }

        public void Raise(object sender, EventArgs args)
        {
            if (_context != null)
            {
                _context.Send(_ => _subscriptionStorage.Raise(sender, args), null);
            }
            else
            {
                _subscriptionStorage.Raise(sender, args);
            }
        }

        private void CheckCreated()
        {
            if (_isCreated) throw new InvalidOperationException("ThrottlingTrigger already created");
        }
    }
}
