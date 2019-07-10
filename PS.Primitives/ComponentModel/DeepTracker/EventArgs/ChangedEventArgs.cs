using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker
{
    public abstract class ChangedEventArgs : RouteEventArgs
    {
        #region Constructors

        protected ChangedEventArgs(Route route)
            : base(route)
        {
        }

        #endregion
    }
}