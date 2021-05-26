using System;
using System.Collections.Generic;
using System.Linq;
using PS.ComponentModel.Navigation;
using PS.ComponentModel.Navigation.Extensions;
using PS.Extensions;

namespace PS.ComponentModel.DeepTracker.Filters
{
    public class ExcludeRoute : IExcludeTrackRoute
    {
        #region Constructors

        public ExcludeRoute(params Route[] routes)
        {
            Routes = routes.Enumerate().ToList();
        }

        #endregion

        #region Properties

        public IReadOnlyCollection<Route> Routes { get; }

        #endregion

        #region IExcludeTrackRoute Members

        public bool Exclude(PropertyReference propertyReference, Lazy<object> value, Route route)
        {
            return Routes.Any(r => route.MatchPartially(r));
        }

        #endregion
    }
}