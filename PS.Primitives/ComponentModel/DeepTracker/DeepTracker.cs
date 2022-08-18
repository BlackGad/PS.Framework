using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using PS.ComponentModel.Dynamic;
using PS.ComponentModel.Extensions;
using PS.ComponentModel.Navigation;
using PS.ComponentModel.Navigation.Extensions;
using PS.Extensions;
using PS.Patterns.Aware;

namespace PS.ComponentModel.DeepTracker
{
    public class DeepTracker : IDisposable,
                               IFluentActivationAware<DeepTracker>
    {
        public static readonly object NotValidValue = new object();

        public static ITrackRouteConfiguration Setup(object source, params object[] routeParts)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var route = Route.Create(routeParts);
            if (route.IsEmpty()) route = Routes.WildcardRecursive;

            return new TrackRouteConfiguration(source, route);
        }

        private readonly Registry<WeakReference> _attachmentObjectRegistry;
        private readonly Registry<Tuple<WeakReference, object[]>> _collectionChangedRegistry;
        private readonly DynamicSubscription<INotifyCollectionChanged, NotifyCollectionChangedEventHandler> _collectionChangedSubscriptions;
        private readonly ConditionalWeakTable<object, string> _collectionChildrenIds;
        private readonly TrackRouteConfiguration _configuration;
        private readonly Registry<Tuple<PropertyReference, object>> _propertyChangedRegistry;
        private readonly DynamicSubscription<PropertyReference, EventHandler> _propertyChangedSubscriptions;
        private readonly WeakReference _sourceReference;

        internal DeepTracker(TrackRouteConfiguration configuration, object source)
        {
            _configuration = configuration;
            _sourceReference = new WeakReference(source);
            _propertyChangedRegistry = new Registry<Tuple<PropertyReference, object>>();
            _collectionChangedRegistry = new Registry<Tuple<WeakReference, object[]>>();
            _collectionChildrenIds = new ConditionalWeakTable<object, string>();
            _attachmentObjectRegistry = new Registry<WeakReference>();

            _propertyChangedSubscriptions = new DynamicSubscription<PropertyReference, EventHandler>(
                (changed, handler) => changed.TryAddValueChanged(handler),
                (changed, handler) => changed.TryRemoveValueChanged(handler));

            _collectionChangedSubscriptions = new DynamicSubscription<INotifyCollectionChanged, NotifyCollectionChangedEventHandler>(
                (changed, handler) => changed.CollectionChanged += handler,
                (changed, handler) => changed.CollectionChanged -= handler);
        }

        public void Dispose()
        {
            Deactivate();
        }

        public DeepTracker Activate()
        {
            var source = _sourceReference.Target;
            if (source == null) return this;

            AddBranch(Routes.Empty, source, _configuration, Enumerable.Empty<int>().ToList());
            IsActive = true;

            return this;
        }

        public DeepTracker Deactivate()
        {
            lock (_propertyChangedRegistry)
            {
                _propertyChangedRegistry.Clear();
                _propertyChangedSubscriptions.UnsubscribeAll();
            }

            lock (_collectionChangedRegistry)
            {
                _collectionChangedRegistry.Clear();
                _collectionChangedSubscriptions.UnsubscribeAll();
            }

            lock (_attachmentObjectRegistry)
            {
                _attachmentObjectRegistry.Clear();
            }

            var source = _sourceReference.Target;
            if (source != null)
            {
                var args = new ObjectDetachedEventArgs(Routes.Empty, source);
                _configuration.Raise(this, args);
            }

            IsActive = false;
            return this;
        }

        public bool IsActive { get; private set; }

        public object GetObject(Route route)
        {
            lock (_attachmentObjectRegistry)
            {
                if (_attachmentObjectRegistry.TryGetValue(route, out var result))
                {
                    return result?.Target;
                }

                return null;
            }
        }

        private void AddBranch(Route visitedRoute, object source, TrackRouteConfiguration configuration, IReadOnlyList<int> visitedObjects)
        {
            var sourceHash = source.GetHash();
            if (visitedObjects.Contains(sourceHash)) return;

            lock (_attachmentObjectRegistry)
            {
                var objectAttachedEventArgs = new ObjectAttachedEventArgs(visitedRoute, source);
                _configuration.Raise(this, objectAttachedEventArgs);
                _attachmentObjectRegistry.Add(visitedRoute, new WeakReference(source));
            }

            visitedObjects = visitedObjects.UnionWith(source.GetHash()).ToList();
            if (source is IEnumerable collection && !(source is string))
            {
                var sourceItems = collection.Enumerate().ToArray();

                if (collection is INotifyCollectionChanged notifyCollection)
                {
                    var closureSourceItems = new WeakReference(sourceItems.Enumerate().ToList());

                    void NotifyCollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs args)
                    {
                        var previousItems = closureSourceItems.Target as List<object>;
                        if (args.Action == NotifyCollectionChangedAction.Reset)
                        {
                            var newClosureSourceItems = sender.Enumerate().ToList();
                            foreach (var item in previousItems.Enumerate())
                            {
                                if (_collectionChildrenIds.TryGetValue(item, out var id))
                                {
                                    RemoveBranch(Route.Create(visitedRoute, id));
                                }
                            }

                            closureSourceItems = new WeakReference(newClosureSourceItems);

                            foreach (var item in newClosureSourceItems)
                            {
                                if (item == null) continue;

                                if (!_collectionChildrenIds.TryGetValue(item, out var id))
                                {
                                    id = Guid.NewGuid().CreateDynamicRoute().ToString();
                                    _collectionChildrenIds.Add(item, id);
                                }

                                AddBranch(Route.Create(visitedRoute, id), item, _configuration, visitedObjects);
                            }
                        }
                        else
                        {
                            foreach (var item in args.NewItems.Enumerate())
                            {
                                if (item == null) continue;

                                if (!_collectionChildrenIds.TryGetValue(item, out var id))
                                {
                                    id = Guid.NewGuid().CreateDynamicRoute().ToString();
                                    _collectionChildrenIds.Add(item, id);
                                }

                                AddBranch(Route.Create(visitedRoute, id), item, _configuration, visitedObjects);
                                previousItems?.Add(item);
                            }

                            foreach (var item in args.OldItems.Enumerate())
                            {
                                if (_collectionChildrenIds.TryGetValue(item, out var id))
                                {
                                    RemoveBranch(Route.Create(visitedRoute, id));
                                }

                                previousItems?.Remove(item);
                            }
                        }

                        var collectionChangedEventArgs = new ChangedCollectionEventArgs(visitedRoute, sender, args);
                        _configuration.Raise(this, collectionChangedEventArgs);
                    }

                    lock (_collectionChangedRegistry)
                    {
                        var args = new CollectionAttachedEventArgs(visitedRoute, collection);
                        _configuration.Raise(this, args);

                        var record = new Tuple<WeakReference, object[]>(new WeakReference(collection), sourceItems);
                        _collectionChangedRegistry.Add(visitedRoute, record);
                    }

                    _collectionChangedSubscriptions.Subscribe(notifyCollection, NotifyCollectionChangedHandler);
                }

                foreach (var item in sourceItems)
                {
                    if (item == null) continue;

                    if (!_collectionChildrenIds.TryGetValue(item, out var id))
                    {
                        id = Guid.NewGuid().CreateDynamicRoute().ToString();
                        _collectionChildrenIds.Add(item, id);
                    }

                    AddBranch(Route.Create(visitedRoute, id), item, _configuration, visitedObjects);
                }
            }

            foreach (var reference in source.GetPropertyReferences())
            {
                AddBranch(visitedRoute, reference, configuration, visitedObjects);
            }
        }

        private void AddBranch(Route visitedRoute,
                               PropertyReference reference,
                               TrackRouteConfiguration configuration,
                               IReadOnlyList<int> visitedObjects)
        {
            if (reference.Name.Contains(".")) return;
            var propertyRoute = Route.Create(visitedRoute, reference.Name);

            var match = propertyRoute.MatchPartially(configuration.Route);
            if (!match) return;

            var lazyValue = new Lazy<object>(() =>
            {
                try
                {
                    if (reference.TryGetValue(out var scopedValue)) return scopedValue;
                }
                catch
                {
                    //Nothing
                }

                return NotValidValue;
            });

            if (!configuration.IsAllowed(reference, lazyValue, propertyRoute)) return;

            try
            {
                var value = lazyValue.Value;
                if (value.AreEqual(NotValidValue)) return;

                if (reference.SupportsChangeEvents)
                {
                    var closureValue = value.WrapValue();

                    void PropertyChangedHandler(object sender, EventArgs e)
                    {
                        var oldValue = closureValue.UnwrapValue();

                        object newValue = null;

                        try
                        {
                            reference.TryGetValue(out newValue);
                        }
                        catch
                        {
                            //Nothing
                        }

                        if (ReferenceEquals(oldValue, newValue)) return;

                        RemoveBranch(propertyRoute);

                        try
                        {
                            if (newValue != null)
                            {
                                closureValue = newValue.WrapValue();
                                AddBranch(propertyRoute, newValue, configuration, visitedObjects);
                            }
                            else
                            {
                                closureValue = new WeakReference(null);
                            }

                            if (!Equals(oldValue, newValue))
                            {
                                var args = new ChangedPropertyEventArgs(propertyRoute, reference, oldValue, newValue);
                                _configuration.Raise(this, args);
                            }
                        }
                        catch
                        {
                            //Nothing
                        }
                    }

                    lock (_propertyChangedRegistry)
                    {
                        var args = new PropertyAttachedEventArgs(propertyRoute, reference, value);
                        _configuration.Raise(this, args);

                        var record = new Tuple<PropertyReference, object>(reference, value);
                        _propertyChangedRegistry.Add(propertyRoute, record);
                        _propertyChangedSubscriptions.Subscribe(reference, PropertyChangedHandler);
                    }
                }

                if (value != null) AddBranch(propertyRoute, value, configuration, visitedObjects);
            }
            catch
            {
                //Nothing
            }
        }

        private void RemoveBranch(Route contextRoute)
        {
            lock (_propertyChangedRegistry)
            {
                foreach (var pair in _propertyChangedRegistry.Remove(contextRoute, true))
                {
                    var args = new PropertyDetachedEventArgs(pair.Key, pair.Value.Item1, pair.Value.Item2);
                    _configuration.Raise(this, args);

                    _propertyChangedSubscriptions.Unsubscribe(pair.Value.Item1);
                }
            }

            lock (_collectionChangedRegistry)
            {
                foreach (var pair in _collectionChangedRegistry.Remove(contextRoute, false))
                {
                    var collection = pair.Value.Item1.Target as IEnumerable;
                    _collectionChangedSubscriptions.Unsubscribe(collection);

                    if (collection == null) continue;

                    var args = new CollectionDetachedEventArgs(pair.Key, collection);
                    _configuration.Raise(this, args);
                }
            }

            lock (_attachmentObjectRegistry)
            {
                foreach (var pair in _attachmentObjectRegistry.Remove(contextRoute, false))
                {
                    var attachedObject = pair.Value.Target;
                    if (attachedObject != null)
                    {
                        var objectDetachedEventArgs = new ObjectDetachedEventArgs(pair.Key, attachedObject);
                        _configuration.Raise(this, objectDetachedEventArgs);
                    }
                }
            }
        }

        #region Nested type: Registry

        private class Registry<TPayload>
        {
            private readonly Dictionary<int, HashSet<int>> _hierarchy;
            private readonly Dictionary<int, KeyValuePair<Route, TPayload>> _registry;

            public Registry()
            {
                _registry = new Dictionary<int, KeyValuePair<Route, TPayload>>();
                _hierarchy = new Dictionary<int, HashSet<int>>();
            }

            public void Add(Route route, TPayload payload)
            {
                if (_registry.ContainsKey(route.GetHashCode())) return;

                _registry.Add(route.GetHashCode(), new KeyValuePair<Route, TPayload>(route, payload));

                var hashes = route.GetHashCodes(RouteCaseMode.Sensitive);

                for (var i = 0; i < hashes.Count; i++)
                {
                    var levelHash = hashes[i];
                    if (!_hierarchy.ContainsKey(levelHash))
                    {
                        _hierarchy.Add(levelHash, new HashSet<int>());
                    }

                    if (i == 0) continue;

                    var prevLevelHash = hashes[i - 1];
                    _hierarchy[prevLevelHash].Add(levelHash);
                }
            }

            public void Clear()
            {
                _registry.Clear();
            }

            public IEnumerable<KeyValuePair<Route, TPayload>> Remove(Route route, bool childrenOnly)
            {
                var hash = route.GetHashCode();

                if (!_hierarchy.ContainsKey(hash)) yield break;

                var keysToRemove = TakeHierarchyKeysForRemove(_hierarchy[hash]);
                if (!childrenOnly) keysToRemove.Add(hash);

                foreach (var keyToRemove in keysToRemove)
                {
                    if (!_registry.ContainsKey(keyToRemove)) continue;
                    var valueToRemove = _registry[keyToRemove];
                    _registry.Remove(keyToRemove);

                    yield return new KeyValuePair<Route, TPayload>(valueToRemove.Key, valueToRemove.Value);
                }
            }

            public bool TryGetValue(Route route, out TPayload payload)
            {
                if (route == null)
                {
                    payload = default;
                    return false;
                }

                var hash = route.GetHashCode();
                if (_registry.TryGetValue(hash, out var pair))
                {
                    payload = pair.Value;
                    return true;
                }

                payload = default;
                return false;
            }

            private IList<int> TakeHierarchyKeysForRemove(IEnumerable<int> from)
            {
                var result = new List<int>();
                foreach (var key in from)
                {
                    result.Add(key);

                    if (_hierarchy.ContainsKey(key))
                    {
                        var subKeys = _hierarchy[key];
                        _hierarchy.Remove(key);
                        result.AddRange(TakeHierarchyKeysForRemove(subKeys));
                    }
                }

                return result;
            }
        }

        #endregion
    }
}
