using System;
using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker
{
    public interface IExcludeTrackRoute
    {
        bool Exclude(PropertyReference propertyReference, Lazy<object> value, Route route);
    }
}