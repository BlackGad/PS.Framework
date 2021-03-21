using System;
using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker
{
    public interface IIncludeTrackRoute
    {
        bool Include(PropertyReference propertyReference, Lazy<object> value, Route route);
    }
}