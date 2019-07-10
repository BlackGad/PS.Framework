﻿using System;
using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker.Filters
{
    public class RelayExclude : IExcludeTrackRoute
    {
        private readonly Func<PropertyReference, object, Route, bool> _allowedFunc;

        #region Constructors

        public RelayExclude(Func<PropertyReference, object, Route, bool> allowedFunc = null)
        {
            _allowedFunc = allowedFunc ?? ((reference, value, route) => true);
        }

        #endregion

        #region IExcludeTrackRoute Members

        public bool Exclude(PropertyReference propertyReference, object value, Route route)
        {
            return _allowedFunc(propertyReference, value, route);
        }

        #endregion
    }
}