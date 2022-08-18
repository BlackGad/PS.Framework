using System;
using System.Collections.Generic;
using System.Linq;
using PS.ComponentModel.Navigation;
using PS.Extensions;

namespace PS.ComponentModel.DeepTracker.Filters
{
    public class ExcludeSourceType : IExcludeTrackRoute
    {
        public ExcludeSourceType(bool derived, params Type[] types)
        {
            Derived = derived;
            Types = types.Enumerate().ToArray();
        }

        public ExcludeSourceType(params Type[] types)
            : this(true, types)
        {
        }

        public bool Derived { get; }

        public IReadOnlyCollection<Type> Types { get; }

        public bool Exclude(PropertyReference propertyReference, Lazy<object> value, Route route)
        {
            if (Derived) return Types.Any(t => t.IsAssignableFrom(propertyReference.SourceType));
            return Types.Any(t => t == propertyReference.SourceType);
        }
    }
}
