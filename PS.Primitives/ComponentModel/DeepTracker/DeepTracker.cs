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

namespace PS.ComponentModel.DeepTracker
{
    public class DeepTracker : IDisposable
    {
        #region Static members

        public static ITrackRouteConfiguration Setup(object source, params object[] routeParts)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var route = Route.Create(routeParts);
            if (route.IsEmpty()) route = Routes.WildcardRecursive;

            return new TrackRouteConfiguration(source, route);
        }

        #endregion

        private readonly List<Tuple<Route, WeakReference>> _attachmentObjectRegistry;

        private readonly List<Tuple<Route, WeakReference, object[]>> _collectionChangedRegistry;
        private readonly DynamicSubscription<INotifyCollectionChanged, NotifyCollectionChangedEventHandler> _collectionChangedSubscriptions;
        private readonly ConditionalWeakTable<object, string> _collectionChildrenIds;
        private readonly TrackRouteConfiguration _configuration;
        private readonly List<Tuple<Route, PropertyReference, object>> _propertyChangedRegistry;
        private readonly DynamicSubscription<PropertyReference, EventHandler> _propertyChangedSubscriptions;
        private readonly WeakReference _sourceReference;

        #region Constructors

        internal DeepTracker(TrackRouteConfiguration configuration, object source)
        {
            _configuration = configuration;
            _sourceReference = new WeakReference(source);
            _propertyChangedRegistry = new List<Tuple<Route, PropertyReference, object>>();
            _collectionChangedRegistry = new List<Tuple<Route, WeakReference, object[]>>();
            _collectionChildrenIds = new ConditionalWeakTable<object, string>();
            _attachmentObjectRegistry = new List<Tuple<Route, WeakReference>>();

            _propertyChangedSubscriptions = new DynamicSubscription<PropertyReference, EventHandler>(
                (changed, handler) => changed.TryAddValueChanged(handler),
                (changed, handler) => changed.TryRemoveValueChanged(handler));

            _collectionChangedSubscriptions = new DynamicSubscription<INotifyCollectionChanged, NotifyCollectionChangedEventHandler>(
                (changed, handler) => changed.CollectionChanged += handler,
                (changed, handler) => changed.CollectionChanged -= handler);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Deactivate();
        }

        #endregion

        #region Members

        public DeepTracker Activate()
        {
            var source = _sourceReference.Target;
            if (source == null) return this;

            AddBranch(Routes.Empty, source, _configuration, Enumerable.Empty<int>().ToList());

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

            return this;
        }

        public object GetObject(Route route)
        {
            lock (_attachmentObjectRegistry)
            {
                return _attachmentObjectRegistry.FirstOrDefault(r => r.Item1.Equals(route))?.Item2?.Target;
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
                _attachmentObjectRegistry.Add(new Tuple<Route, WeakReference>(visitedRoute, new WeakReference(source)));
            }

            visitedObjects = visitedObjects.UnionWith(source.GetHash()).ToList();
            if (source is IEnumerable collection && !(source is string))
            {
                var sourceItems = collection.Enumerate().ToArray();

                if (collection is INotifyCollectionChanged notifyCollection)
                {
                    var closureSourceItems = new WeakReference(sourceItems);

                    void NotifyCollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs args)
                    {
                        var previousItems = closureSourceItems.Target as object[];

                        if (args.Action == NotifyCollectionChangedAction.Reset)
                        {
                            foreach (var item in previousItems.Enumerate())
                            {
                                if (_collectionChildrenIds.TryGetValue(item, out var id))
                                {
                                    RemoveBranch(Route.Create(visitedRoute, id));
                                }
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
                            }

                            foreach (var item in args.OldItems.Enumerate())
                            {
                                if (_collectionChildrenIds.TryGetValue(item, out var id))
                                {
                                    RemoveBranch(Route.Create(visitedRoute, id));
                                }
                            }
                        }

                        closureSourceItems = new WeakReference(sender.Enumerate().ToArray());

                        var collectionChangedEventArgs = new ChangedCollectionEventArgs(visitedRoute, sender, args);
                        _configuration.Raise(this, collectionChangedEventArgs);
                    }

                    lock (_collectionChangedRegistry)
                    {
                        var args = new CollectionAttachedEventArgs(visitedRoute, collection);
                        _configuration.Raise(this, args);

                        var record = new Tuple<Route, WeakReference, object[]>(visitedRoute, new WeakReference(collection), sourceItems);
                        _collectionChangedRegistry.Add(record);
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

            var validValue = reference.TryGetValue(out var value);
            var isAllowed = configuration.IsAllowed(reference, value, propertyRoute);
            if (!isAllowed) return;

            try
            {
                if (!validValue) return;

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

                        var record = new Tuple<Route, PropertyReference, object>(propertyRoute, reference, value);
                        _propertyChangedRegistry.Add(record);
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
                var recordsToRemove = _propertyChangedRegistry.Where(r => r.Item1.StartWith(contextRoute)).ToList();
                foreach (var recordToRemove in recordsToRemove)
                {
                    if (recordToRemove.Item1.Match(contextRoute)) continue;

                    var args = new PropertyDetachedEventArgs(recordToRemove.Item1, recordToRemove.Item2, recordToRemove.Item3);
                    _configuration.Raise(this, args);

                    _propertyChangedSubscriptions.Unsubscribe(recordToRemove.Item2);
                    _propertyChangedRegistry.Remove(recordToRemove);
                }
            }

            lock (_collectionChangedRegistry)
            {
                var recordsToRemove = _collectionChangedRegistry.Where(r => r.Item1.StartWith(contextRoute)).ToList();
                foreach (var recordToRemove in recordsToRemove)
                {
                    var collection = recordToRemove.Item2.Target as IEnumerable;
                    _collectionChangedSubscriptions.Unsubscribe(collection);
                    _collectionChangedRegistry.Remove(recordToRemove);

                    if (collection == null) continue;

                    var args = new CollectionDetachedEventArgs(recordToRemove.Item1, collection);
                    _configuration.Raise(this, args);
                }
            }

            lock (_attachmentObjectRegistry)
            {
                var recordsToRemove = _attachmentObjectRegistry.Where(r => r.Item1.StartWith(contextRoute)).ToList();
                foreach (var recordToRemove in recordsToRemove)
                {
                    _attachmentObjectRegistry.Remove(recordToRemove);

                    var attachedObject = recordToRemove.Item2.Target;
                    if (attachedObject != null)
                    {
                        var objectDetachedEventArgs = new ObjectDetachedEventArgs(contextRoute, recordToRemove.Item2.Target);
                        _configuration.Raise(this, objectDetachedEventArgs);
                    }
                }
            }
        }

        #endregion
    }
}