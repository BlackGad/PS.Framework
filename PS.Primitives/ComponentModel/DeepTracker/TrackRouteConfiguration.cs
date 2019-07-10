using System;
using System.Collections.Generic;
using System.Linq;
using PS.ComponentModel.DeepTracker.Extensions;
using PS.ComponentModel.DeepTracker.Filters;
using PS.Extensions;

namespace PS.ComponentModel.DeepTracker
{
    internal class TrackRouteConfiguration : ITrackRouteConfiguration
    {
        #region Constants

        private static readonly IReadOnlyList<Type> PossibleHandlerTypes;

        #endregion

        private readonly List<IExcludeTrackRoute> _excludeList;
        private readonly List<IIncludeTrackRoute> _includeList;

        private readonly EventHandlerSubscriptionStorage _subscriptionStorage;

        private bool _isCreated;
        private object _source;

        #region Constructors

        static TrackRouteConfiguration()
        {
            PossibleHandlerTypes = new List<Type>
            {
                typeof(CollectionAttachedEventArgs),
                typeof(CollectionDetachedEventArgs),

                typeof(ObjectAttachedEventArgs),
                typeof(ObjectDetachedEventArgs),

                typeof(PropertyAttachedEventArgs),
                typeof(PropertyDetachedEventArgs),

                typeof(ChangedCollectionEventArgs),
                typeof(ChangedPropertyEventArgs)
            };
        }

        public TrackRouteConfiguration(object source, Navigation.Route route)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _excludeList = new List<IExcludeTrackRoute>
            {
                Excludes.KnownPropertyTypes
            };
            _includeList = new List<IIncludeTrackRoute>();
            _subscriptionStorage = new EventHandlerSubscriptionStorage();
            _subscriptionStorage.PossibleHandlerTypes.PutRange(PossibleHandlerTypes);
            Route = route ?? throw new ArgumentNullException(nameof(route));

            //Do not track inside string
            this.ExcludeSourceWithType(typeof(string));
        }

        #endregion

        #region Properties

        public Navigation.Route Route { get; }

        #endregion

        #region ITrackRouteConfiguration Members

        public ITrackRouteConfiguration Exclude(IExcludeTrackRoute exclude)
        {
            if (exclude == null) throw new ArgumentNullException(nameof(exclude));
            CheckCreated();
            _excludeList.Add(exclude);
            return this;
        }

        public ITrackRouteConfiguration Include(IIncludeTrackRoute include)
        {
            if (include == null) throw new ArgumentNullException(nameof(include));
            CheckCreated();
            _includeList.Add(include);
            return this;
        }

        public ITrackRouteConfiguration Subscribe(Delegate handler)
        {
            CheckCreated();
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            _subscriptionStorage.Subscribe(handler);

            return this;
        }

        public DeepTracker Create()
        {
            CheckCreated();

            //First step optimization - collapse well known include filters
            var includeList = _includeList.ToList();
            _includeList.Clear();

            var includeRouteFilters = includeList.Enumerate<IncludeRoute>().ToList();
            if (includeRouteFilters.Any())
            {
                _includeList.Add(new IncludeRoute(includeRouteFilters.SelectMany(f => f.Routes).Distinct().ToArray()));
                includeList = includeList.Except(includeRouteFilters).ToList();
            }

            _includeList.AddRange(includeList);

            //First step optimization - collapse well known exclude filters
            var excludeList = _excludeList.ToList();
            _excludeList.Clear();

            var excludeRouteFilters = excludeList.Enumerate<ExcludeRoute>().ToList();
            if (excludeRouteFilters.Any())
            {
                _excludeList.Add(new ExcludeRoute(excludeRouteFilters.SelectMany(f => f.Routes).Distinct().ToArray()));
                excludeList = excludeList.Except(excludeRouteFilters).ToList();
            }

            var excludeSourceTypeWithDerivedFilters = excludeList.Enumerate<ExcludeSourceType>().Where(f => f.Derived).ToList();
            if (excludeSourceTypeWithDerivedFilters.Any())
            {
                _excludeList.Add(new ExcludeSourceType(true, excludeSourceTypeWithDerivedFilters.SelectMany(f => f.Types).Distinct().ToArray()));
                excludeList = excludeList.Except(excludeSourceTypeWithDerivedFilters).ToList();
            }

            var excludeSourceTypeWithNonDerivedFilters = excludeList.Enumerate<ExcludeSourceType>().Where(f => !f.Derived).ToList();
            if (excludeSourceTypeWithNonDerivedFilters.Any())
            {
                _excludeList.Add(new ExcludeSourceType(false, excludeSourceTypeWithNonDerivedFilters.SelectMany(f => f.Types).Distinct().ToArray()));
                excludeList = excludeList.Except(excludeSourceTypeWithNonDerivedFilters).ToList();
            }

            var excludePropertyTypeWithDerivedFilters = excludeList.Enumerate<ExcludePropertyType>().Where(f => f.Derived).ToList();
            if (excludePropertyTypeWithDerivedFilters.Any())
            {
                _excludeList.Add(new ExcludePropertyType(true, excludePropertyTypeWithDerivedFilters.SelectMany(f => f.Types).Distinct().ToArray()));
                excludeList = excludeList.Except(excludePropertyTypeWithDerivedFilters).ToList();
            }

            var excludePropertyTypeWithNonDerivedFilters = excludeList.Enumerate<ExcludePropertyType>().Where(f => !f.Derived).ToList();
            if (excludePropertyTypeWithNonDerivedFilters.Any())
            {
                _excludeList.Add(new ExcludePropertyType(false, excludePropertyTypeWithNonDerivedFilters.SelectMany(f => f.Types).Distinct().ToArray()));
                excludeList = excludeList.Except(excludePropertyTypeWithNonDerivedFilters).ToList();
            }

            var excludeObjectPropertyFilters = excludeList.Enumerate<ExcludeObjectProperty>().ToList();
            if (excludeObjectPropertyFilters.Any())
            {
                var collapsedPairs = excludeObjectPropertyFilters
                                     .SelectMany(f => f.ObjectProperties.SelectMany(g => g.Select(property => new KeyValuePair<Type, string>(g.Key, property))))
                                     .DistinctBy(p => p.Key.GetHash().MergeHash(p.Value.GetHash()))
                                     .ToArray();

                _excludeList.Add(new ExcludeObjectProperty(collapsedPairs));
                excludeList = excludeList.Except(excludeObjectPropertyFilters).ToList();
            }

            _excludeList.AddRange(excludeList);

            var result = new DeepTracker(this, _source);
            _source = null;
            _isCreated = true;
            return result;
        }

        #endregion

        #region Members

        public bool IsAllowed(PropertyReference reference, object value, Navigation.Route propertyRoute)
        {
            foreach (var exclude in _excludeList)
            {
                if (exclude.Exclude(reference, value, propertyRoute)) return false;
            }

            if (!_includeList.Any()) return true;

            var result = false;
            foreach (var include in _includeList)
            {
                result = result || include.Include(reference, value, propertyRoute);
            }

            return result;
        }

        public void Raise(DeepTracker deepTracker, RouteEventArgs args)
        {
            _subscriptionStorage.Raise(deepTracker, args);
        }

        private void CheckCreated()
        {
            if (_isCreated) throw new InvalidOperationException("Tracker already created");
        }

        #endregion
    }
}