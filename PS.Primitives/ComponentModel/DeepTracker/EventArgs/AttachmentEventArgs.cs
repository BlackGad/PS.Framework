using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker
{
    public abstract class AttachmentEventArgs : RouteEventArgs
    {
        #region Constructors

        protected AttachmentEventArgs(Route route)
            : base(route)
        {
        }

        #endregion
    }
}