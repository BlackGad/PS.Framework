using System;
using System.Collections.Generic;
using System.Linq;
using PS.ComponentModel.DeepTracker.Extensions;
using PS.ComponentModel.DeepTracker.Filters;
using PS.ComponentModel.Navigation;
using PS.Extensions;

namespace PS.ComponentModel.DeepTracker
{
    internal class TrackRouteConfiguration : ITrackRouteConfiguration
    {
        private readonly List<IExcludeTrackRoute> _excludeList;
        private readonly List<IIncludeTrackRoute> _includeList;

        private readonly EventHandlerSubscriptionStorage _subscriptionStorage;

        private bool _isCreated;
        private object _source;

        public TrackRouteConfiguration(object source, Route route)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _excludeList = new List<IExcludeTrackRoute>
            {
                Excludes.KnownSourceTypes
            };
            _includeList = new List<IIncludeTrackRoute>();
            _subscriptionStorage = new EventHandlerSubscriptionStorage();
            Route = route ?? throw new ArgumentNullException(nameof(route));

            //Do not track structs
            this.Exclude((reference, value, r) => !reference.SourceType.IsClass);
        }

        public Route Route { get; }

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

        public ITrackRouteConfiguration Subscribe<T>(EventHandler<T> handler)
            where T : RouteEventArgs
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

            var includeObjectPropertyFilters = includeList.Enumerate<IncludeObjectProperty>().ToList();
            if (includeObjectPropertyFilters.Any())
            {
                var collapsedPairs = includeObjectPropertyFilters
                                     .SelectMany(f => f.ObjectProperties.SelectMany(g => g.Select(property => new KeyValuePair<Type, string>(g.Key, property))))
                                     .DistinctBy(p => p.Key.GetHash().MergeHash(p.Value.GetHash()))
                                     .ToArray();

                _includeList.Add(new IncludeObjectProperty(collapsedPairs));
                includeList = includeList.Except(includeObjectPropertyFilters).ToList();
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
            _excludeList.Add(new ExcludeByAttribute());

            var result = new DeepTracker(this, _source);
            _source = null;
            _isCreated = true;
            return result;
        }

        public bool IsAllowed(PropertyReference reference, Lazy<object> value, Route propertyRoute)
        {
            var isIncluded = true;
            if (_includeList.Any())
            {
                isIncluded = false;
                foreach (var include in _includeList)
                {
                    isIncluded = isIncluded || include.Include(reference, value, propertyRoute);
                }
            }

            if (!isIncluded) return false;

            foreach (var exclude in _excludeList)
            {
                if (exclude.Exclude(reference, value, propertyRoute)) return false;
            }

            return true;
        }

        public bool IsAllowed(PropertyReference reference, Route propertyRoute)
        {
            if (reference.TryGetDescriptor(out var descriptor))
            {
                var disableTrackingAttribute = descriptor.Attributes.Enumerate<DisableTrackingAttribute>().FirstOrDefault();
                if (disableTrackingAttribute != null) return false;
            }

            return true;
        }

        public void Raise(DeepTracker deepTracker, RouteEventArgs args)
        {
            _subscriptionStorage.Raise(deepTracker, args);
        }

        private void CheckCreated()
        {
            if (_isCreated) throw new InvalidOperationException("Tracker already created");
        }
    }
}
