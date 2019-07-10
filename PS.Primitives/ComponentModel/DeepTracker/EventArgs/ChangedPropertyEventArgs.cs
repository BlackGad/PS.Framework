using System;
using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker
{
    public class ChangedPropertyEventArgs : ChangedEventArgs
    {
        #region Constructors

        public ChangedPropertyEventArgs(Route route, PropertyReference propertyReference, object oldValue, object newValue)
            : base(route)
        {
            PropertyReference = propertyReference ?? throw new ArgumentNullException(nameof(propertyReference));
            OldValue = oldValue;
            NewValue = newValue;
        }

        #endregion

        #region Properties

        public object NewValue { get; }
        public object OldValue { get; }
        public PropertyReference PropertyReference { get; }

        #endregion
    }
}