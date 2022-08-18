using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker
{
    public class PropertyDetachedEventArgs : PropertyAttachmentEventArgs
    {
        public PropertyDetachedEventArgs(Route route, PropertyReference propertyReference, object value)
            : base(route, propertyReference, value)
        {
        }
    }
}
