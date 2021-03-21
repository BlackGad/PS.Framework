using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker
{
    public abstract class ChangedEventArgs : RouteEventArgs
    {
        protected ChangedEventArgs(Route route)
            : base(route)
        {
        }
    }
}