using System;
using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker
{
    public interface IExcludeTrackRoute
    {
        #region Members

        bool Exclude(PropertyReference propertyReference, Lazy<object> value, Route route);

        #endregion
    }
}