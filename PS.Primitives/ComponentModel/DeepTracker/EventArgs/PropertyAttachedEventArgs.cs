using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker
{
    public class PropertyAttachedEventArgs : PropertyAttachmentEventArgs
    {
        #region Constructors

        public PropertyAttachedEventArgs(Route route, PropertyReference propertyReference, object value)
            : base(route, propertyReference, value)
        {
        }

        #endregion
    }
}