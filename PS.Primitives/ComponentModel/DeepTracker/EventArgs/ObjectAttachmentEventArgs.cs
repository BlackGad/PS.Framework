using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker
{
    public abstract class ObjectAttachmentEventArgs : AttachmentEventArgs
    {
        protected ObjectAttachmentEventArgs(Route route, object @object)
            : base(route)
        {
            Object = @object;
        }

        public object Object { get; }
    }
}
