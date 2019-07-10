using System;
using PS.Patterns;

namespace PS.ComponentModel.DeepTracker
{
    public interface ITrackRouteConfiguration : IFluentBuilder<DeepTracker>
    {
        #region Members

        ITrackRouteConfiguration Exclude(IExcludeTrackRoute exclude);
        ITrackRouteConfiguration Include(IIncludeTrackRoute include);
        ITrackRouteConfiguration Subscribe(Delegate handler);

        #endregion
    }
}