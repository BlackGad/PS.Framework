using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using PS.ComponentModel.DeepTracker;
using PS.ComponentModel.DeepTracker.Extensions;
using PS.ComponentModel.Navigation;
using PS.ComponentModel.Navigation.Extensions;
using PS.WPF.Extensions;

namespace PS.WPF.Data
{
    [DebuggerDisplay("Count = {Count}")]
    [ContentProperty(nameof(Containers))]
    public class CompositeCollection : InheritanceContextPropagator,
                                       IList<object>,
                                       IReadOnlyList<object>,
                                       IList,
                                       INotifyCollectionChanged,
                                       IDisposable
    {
        #region Property definitions

        private static readonly DependencyPropertyKey CountPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(Count),
                                                typeof(int),
                                                typeof(CompositeCollection),
                                                new FrameworkPropertyMetadata(default(int)));

        public static readonly DependencyProperty CountProperty = CountPropertyKey.DependencyProperty;

        public static readonly DependencyProperty ContainersProperty =
            DependencyProperty.Register(nameof(Containers),
                                        typeof(CompositeContainerCollection),
                                        typeof(CompositeCollection),
                                        new FrameworkPropertyMetadata(null, CoerceContainersValueCallback));

        #endregion

        #region Static members

        private static object CoerceContainersValueCallback(DependencyObject d, object baseValue)
        {
            return baseValue ?? new CompositeContainerCollection();
        }

        #endregion

        private readonly List<Tuple<Route, object>> _itemsRegistry;

        private readonly DeepTracker _itemsTracker;
        private readonly object _syncRoot;

        #region Constructors

        public CompositeCollection()
        {
            _itemsRegistry = new List<Tuple<Route, object>>();

            CoerceValue(ContainersProperty);

            _syncRoot = new object();
            _itemsTracker = DeepTracker.Setup(this)
                                       .Include<CompositeCollection>(instance => instance.Containers)
                                       .Include<CompositeContainer>(instance => instance.Source)
                                       .Subscribe<ObjectAttachmentEventArgs>(OnObjectAttachment)
                                       .Create()
                                       .Activate();
        }

        #endregion

        #region Properties

        public CompositeContainerCollection Containers
        {
            get { return (CompositeContainerCollection)GetValue(ContainersProperty); }
            set { SetValue(ContainersProperty, value); }
        }

        private IList<object> Items
        {
            get { return _itemsRegistry.Select(i => i.Item2).ToList(); }
        }

        #endregion

        #region Override members

        ~CompositeCollection()
        {
            if (Dispatcher?.HasShutdownFinished == true) return;
            Dispatcher.SafeCall(Dispose);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                _itemsTracker.Dispose();
            }
            catch
            {
                //Nothing
            }

            GC.SuppressFinalize(this);
        }

        #endregion

        #region IList Members

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        bool ICollection.IsSynchronized
        {
            get { return true; }
        }

        object ICollection.SyncRoot
        {
            get { return _syncRoot; }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            var items = Items;
            for (var i = 0; i < items.Count; i++)
            {
                array.SetValue(items[i], i + index);
            }
        }

        void IList.Clear()
        {
            throw new InvalidOperationException("Collection is readonly");
        }

        void IList.Remove(object value)
        {
            throw new InvalidOperationException("Collection is readonly");
        }

        void IList.RemoveAt(int index)
        {
            throw new InvalidOperationException("Source collection not set");
        }

        int IList.Add(object value)
        {
            throw new InvalidOperationException("Collection is readonly");
        }

        void IList.Insert(int index, object item)
        {
            throw new InvalidOperationException("Source collection not set");
        }

        #endregion

        #region IList<object> Members

        public int Count
        {
            get { return (int)GetValue(CountProperty); }
            private set { SetValue(CountPropertyKey, value); }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public object this[int index]
        {
            get { return Items[index]; }
            set { throw new InvalidOperationException("Collection is readonly"); }
        }

        public IEnumerator<object> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void ICollection<object>.Add(object item)
        {
            throw new InvalidOperationException("Collection is readonly");
        }

        void ICollection<object>.Clear()
        {
            throw new InvalidOperationException("Collection is readonly");
        }

        public bool Contains(object item)
        {
            return Items.Contains(item);
        }

        public void CopyTo(object[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        bool ICollection<object>.Remove(object item)
        {
            throw new InvalidOperationException("Collection is readonly");
        }

        public int IndexOf(object item)
        {
            return Items.IndexOf(item);
        }

        void IList<object>.Insert(int index, object item)
        {
            throw new InvalidOperationException("Source collection not set");
        }

        void IList<object>.RemoveAt(int index)
        {
            throw new InvalidOperationException("Source collection not set");
        }

        #endregion

        #region INotifyCollectionChanged Members

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region Event handlers

        private void OnObjectAttachment(object sender, ObjectAttachmentEventArgs e)
        {
            if (e is ObjectDetachedEventArgs)
            {
                var removedItems = _itemsRegistry.Where(i => i.Item1.StartWith(e.Route)).ToList();
                if (!removedItems.Any()) return;

                foreach (var removedItem in removedItems)
                {
                    var oldStartIndex = _itemsRegistry.IndexOf(removedItem);
                    if (oldStartIndex < 0) return;

                    _itemsRegistry.Remove(removedItem);

                    Count = _itemsRegistry.Count;

                    var oldItems = new[] { removedItem.Item2 };
                    var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItems, oldStartIndex);
                    CollectionChanged?.Invoke(this, args);
                }
            }

            if (e is ObjectAttachedEventArgs)
            {
                //Item attachment to/from some container source collection
                if (e.Route.Match(Route.Create(ContainersProperty.Name, Routes.Wildcard, CompositeContainer.SourceProperty.Name, Routes.Wildcard)))
                {
                    if (!e.Route.LastOrDefault().IsDynamicRoute()) return;

                    if (!Filter(e.Object)) return;

                    var item = new Tuple<Route, object>(e.Route, e.Object);

                    _itemsRegistry.Add(item);
                    _itemsRegistry.Sort(Comparator);

                    Count = _itemsRegistry.Count;

                    var newStartIndex = _itemsRegistry.LastIndexOf(item);
                    var newItems = new[] { e.Object };

                    var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems, newStartIndex);
                    CollectionChanged?.Invoke(this, args);
                }
            }
        }

        #endregion

        #region Members

        protected virtual bool Filter(object item)
        {
            return true;
        }

        private int Comparator(Tuple<Route, object> x, Tuple<Route, object> y)
        {
            return 0;
        }

        #endregion
    }
}