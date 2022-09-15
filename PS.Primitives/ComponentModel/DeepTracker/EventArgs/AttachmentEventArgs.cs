using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker
{
    public abstract class AttachmentEventArgs : RouteEventArgs
    {
        protected AttachmentEventArgs(Route route)
            : base(route)
        {
        }
    }
}
