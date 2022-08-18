using System;
using System.Linq;
using PS.ComponentModel.Navigation;
using PS.Extensions;

namespace PS.ComponentModel.DeepTracker.Filters
{
    public class ExcludeByAttribute : IExcludeTrackRoute
    {
        public bool Exclude(PropertyReference propertyReference, Lazy<object> value, Route route)
        {
            if (propertyReference.TryGetDescriptor(out var descriptor))
            {
                var disableTrackingAttribute = descriptor.Attributes.Enumerate<DisableTrackingAttribute>().FirstOrDefault();
                if (disableTrackingAttribute != null) return true;
            }

            return false;
        }
    }
}
