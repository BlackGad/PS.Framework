using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker
{
    public class ObjectAttachedEventArgs : ObjectAttachmentEventArgs
    {
        public ObjectAttachedEventArgs(Route route, object @object)
            : base(route, @object)
        {
        }
    }
}
