﻿using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker
{
    public interface IIncludeTrackRoute
    {
        #region Members

        bool Include(PropertyReference propertyReference, object value, Route route);

        #endregion
    }
}