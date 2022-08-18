using System;
using System.Collections.Generic;
using System.Linq;
using PS.ComponentModel.Navigation;
using PS.ComponentModel.Navigation.Extensions;
using PS.Extensions;

namespace PS.ComponentModel.DeepTracker.Filters
{
    public class IncludeRoute : IIncludeTrackRoute
    {
        public IncludeRoute(params Route[] routes)
        {
            Routes = routes.Enumerate().ToList();
        }

        public IReadOnlyCollection<Route> Routes { get; }

        public bool Include(PropertyReference propertyReference, Lazy<object> value, Route route)
        {
            return Routes.Any(r => route.MatchPartially(r));
        }
    }
}
