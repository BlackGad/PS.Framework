using System;
using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker
{
    public abstract class RouteEventArgs : EventArgs
    {
        protected RouteEventArgs(Route route)
        {
            Route = route ?? throw new ArgumentNullException(nameof(route));
        }

        public Route Route { get; }
    }
}