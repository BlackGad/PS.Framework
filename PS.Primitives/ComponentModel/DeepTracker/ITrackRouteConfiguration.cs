using System;
using PS.Patterns;

namespace PS.ComponentModel.DeepTracker
{
    public interface ITrackRouteConfiguration : IFluentBuilder<DeepTracker>
    {
        ITrackRouteConfiguration Exclude(IExcludeTrackRoute exclude);

        ITrackRouteConfiguration Include(IIncludeTrackRoute include);

        ITrackRouteConfiguration Subscribe<T>(EventHandler<T> handler)
            where T : RouteEventArgs;
    }
}
