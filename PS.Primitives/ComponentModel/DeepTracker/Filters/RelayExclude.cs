using System;
using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker.Filters
{
    public class RelayExclude : IExcludeTrackRoute
    {
        private readonly Func<PropertyReference, Lazy<object>, Route, bool> _allowedFunc;

        public RelayExclude(Func<PropertyReference, Lazy<object>, Route, bool> allowedFunc = null)
        {
            _allowedFunc = allowedFunc ?? ((reference, value, route) => true);
        }

        public bool Exclude(PropertyReference propertyReference, Lazy<object> value, Route route)
        {
            return _allowedFunc(propertyReference, value, route);
        }
    }
}
