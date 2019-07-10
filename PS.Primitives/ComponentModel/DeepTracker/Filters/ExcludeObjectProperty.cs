using System;
using System.Collections.Generic;
using System.Linq;
using PS.ComponentModel.Navigation;
using PS.Extensions;

namespace PS.ComponentModel.DeepTracker.Filters
{
    public class ExcludeObjectProperty : IExcludeTrackRoute
    {
        #region Constructors

        public ExcludeObjectProperty(ILookup<Type, string> lookup)
        {
            ObjectProperties = lookup ?? throw new ArgumentNullException(nameof(lookup));
        }

        public ExcludeObjectProperty(params KeyValuePair<Type, string>[] objectProperties)
            : this(objectProperties.ToLookup(p => p.Key, p => p.Value))
        {
        }

        #endregion

        #region Properties

        public ILookup<Type, string> ObjectProperties { get; }

        #endregion

        #region IExcludeTrackRoute Members

        public bool Exclude(PropertyReference propertyReference, object value, Route route)
        {
            foreach (var objectType in ObjectProperties)
            {
                if (objectType.Key.IsAssignableFrom(propertyReference.SourceType))
                {
                    if (objectType.Any(p => p.AreEqual(propertyReference.Name))) return true;
                }
            }

            return false;
        }

        #endregion
    }
}