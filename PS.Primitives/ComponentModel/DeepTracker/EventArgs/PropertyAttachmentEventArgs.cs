using System;
using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker
{
    public abstract class PropertyAttachmentEventArgs : AttachmentEventArgs
    {
        #region Constructors

        protected PropertyAttachmentEventArgs(Route route, PropertyReference propertyReference, object value)
            : base(route)
        {
            PropertyReference = propertyReference ?? throw new ArgumentNullException(nameof(propertyReference));
            Value = value;
        }

        #endregion

        #region Properties

        public PropertyReference PropertyReference { get; }

        public object Value { get; }

        #endregion
    }
}