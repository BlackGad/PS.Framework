using System;
using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker
{
    public abstract class PropertyAttachmentEventArgs : AttachmentEventArgs
    {
        protected PropertyAttachmentEventArgs(Route route, PropertyReference propertyReference, object value)
            : base(route)
        {
            PropertyReference = propertyReference ?? throw new ArgumentNullException(nameof(propertyReference));
            Value = value;
        }

        public PropertyReference PropertyReference { get; }

        public object Value { get; }
    }
}
