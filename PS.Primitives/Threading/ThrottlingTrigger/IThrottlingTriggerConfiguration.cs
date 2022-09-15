using System;
using System.Threading;
using PS.Patterns;

namespace PS.Threading.ThrottlingTrigger
{
    public interface IThrottlingTriggerConfiguration : IFluentBuilder<ThrottlingTrigger>
    {
        IThrottlingTriggerConfiguration DispatchWith(SynchronizationContext context);

        IThrottlingTriggerConfiguration Subscribe<T>(EventHandler<T> handler);

        IThrottlingTriggerConfiguration Throttle(TimeSpan time);

        IThrottlingTriggerConfiguration TrackWhenInactive(bool track);
    }
}
