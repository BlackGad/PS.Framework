using System;
using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker.Filters
{
    public class RelayInclude : IIncludeTrackRoute
    {
        private readonly Func<PropertyReference, Lazy<object>, Route, bool> _allowedFunc;

        #region Constructors

        public RelayInclude(Func<PropertyReference, Lazy<object>, Route, bool> allowedFunc = null)
        {
            _allowedFunc = allowedFunc ?? ((reference, value, route) => true);
        }

        #endregion

        #region IIncludeTrackRoute Members

        public bool Include(PropertyReference propertyReference, Lazy<object> value, Route route)
        {
            return _allowedFunc(propertyReference, value, route);
        }

        #endregion
    }
}