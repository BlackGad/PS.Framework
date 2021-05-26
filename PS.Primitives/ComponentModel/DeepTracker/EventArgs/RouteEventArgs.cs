using System;
using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker
{
    public abstract class RouteEventArgs : EventArgs
    {
        #region Constructors

        protected RouteEventArgs(Route route)
        {
            Route = route ?? throw new ArgumentNullException(nameof(route));
        }

        #endregion

        #region Properties

        public Route Route { get; }

        #endregion
    }
}