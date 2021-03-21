using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker
{
    public class ObjectDetachedEventArgs : ObjectAttachmentEventArgs
    {
        public ObjectDetachedEventArgs(Route route, object @object)
            : base(route, @object)
        {
        }
    }
}