﻿using System;
using System.Collections.Generic;
using System.Linq;
using PS.ComponentModel.Navigation;
using PS.Extensions;

namespace PS.ComponentModel.DeepTracker.Filters
{
    public class ExcludePropertyType : IExcludeTrackRoute
    {
        #region Constructors

        public ExcludePropertyType(bool derived, params Type[] types)
        {
            Derived = derived;
            Types = types.Enumerate().ToArray();
        }

        public ExcludePropertyType(params Type[] types)
            : this(true, types)
        {
        }

        #endregion

        #region Properties

        public bool Derived { get; }

        public IReadOnlyCollection<Type> Types { get; }

        #endregion

        #region IExcludeTrackRoute Members

        public bool Exclude(PropertyReference propertyReference, Lazy<object> value, Route route)
        {
            var propertyType = value?.GetType() ?? propertyReference.PropertyType;
            if (Derived) return Types.Any(t => t.IsAssignableFrom(propertyType));
            return Types.Any(t => t == propertyType);
        }

        #endregion
    }
}