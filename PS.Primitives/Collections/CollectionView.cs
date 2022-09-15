using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using PS.ComponentModel.DeepTracker;
using PS.ComponentModel.DeepTracker.Extensions;
using PS.ComponentModel.Navigation;
using PS.Extensions;
using PS.Extensions.Ducking;

namespace PS.Collections
{
    public class CollectionView<T> : CollectionView<T, T>
    {
        public CollectionView(Func<T, bool> filterPredicate = null)
            : base(arg => arg, arg => arg, filterPredicate)
        {
        }
    }

    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    [DebuggerTypeProxy(typeof(CollectionViewProxy<,>))]
    public class CollectionView<TSource, TTarget> : IList<TTarget>,
                                                    IReadOnlyList<TTarget>,
                                                    IList,
                                                    INotifyCollectionChanged,
                                                    INotifyPropertyChanged,
                                                    IDisposable
    {
        private readonly Func<TTarget, TSource> _convertBackFunc;
        private readonly Dictionary<int, TSource> _convertBackMap;
        private readonly Func<TSource, TTarget> _convertFunc;
        private readonly Dictionary<int, TTarget> _convertMap;
        private readonly Action<TTarget> _disposeAction;
        private readonly Func<TSource, bool> _filterPredicate;
        private readonly DeepTracker _itemsTracker;
        private readonly object _syncRoot;
        private IList<TTarget> _cachedList;

        private IEnumerable _itemsSource;

        public CollectionView(Func<TSource, TTarget> convertFunc,
                              Func<TTarget, TSource> convertBackFunc = null,
                              Func<TSource, bool> filterPredicate = null,
                              Action<TTarget> disposeAction = null)
        {
            _convertFunc = convertFunc ?? throw new ArgumentNullException(nameof(convertFunc));
            _convertBackFunc = convertBackFunc;
            _disposeAction = disposeAction;
            _filterPredicate = filterPredicate ?? (source => true);

            _convertMap = new Dictionary<int, TTarget>();
            _convertBackMap = new Dictionary<int, TSource>();
            _syncRoot = new object();

            _itemsTracker = DeepTracker.Setup(this)
                                       .Include<CollectionView<TSource, TTarget>>(instance => instance.ItemsSource)
                                       .Subscribe<ChangedPropertyEventArgs>(OnPropertyChanged)
                                       .Subscribe<ChangedCollectionEventArgs>(OnCollectionChanged)
                                       .Create()
                                       .Activate();
        }

        public IEnumerable ItemsSource
        {
            get { return _itemsSource; }
            set
            {
                if (_itemsSource.AreEqual(value)) return;
                _itemsSource = value;
                OnPropertyChanged();
            }
        }

        private IList<TTarget> Items
        {
            get { return _cachedList ?? (_cachedList = GetSourceItems()); }
        }

        ~CollectionView()
        {
            Dispose();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _itemsTracker.Dispose();
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        bool ICollection.IsSynchronized
        {
            get { return _cachedList != null; }
        }

        object IList.this[int index]
        {
            get { return this[index]; }
            set
            {
                if (value is TTarget || Equals(value, default(TTarget))) this[index] = (TTarget)value;
            }
        }

        object ICollection.SyncRoot
        {
            get { return _syncRoot; }
        }

        int IList.Add(object value)
        {
            if (value is TTarget || Equals(value, default(TTarget)))
            {
                Add((TTarget)value);
                return IndexOf((TTarget)value);
            }

            throw new InvalidCastException();
        }

        bool IList.Contains(object value)
        {
            if (value is TTarget || Equals(value, default(TTarget))) return Contains((TTarget)value);
            throw new InvalidCastException();
        }

        int IList.IndexOf(object value)
        {
            if (value is TTarget || Equals(value, default(TTarget))) return IndexOf((TTarget)value);
            throw new InvalidCastException();
        }

        void IList.Insert(int index, object value)
        {
            if (value is TTarget || Equals(value, default(TTarget)))
            {
                Insert(index, (TTarget)value);
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        void IList.Remove(object value)
        {
            if (value is TTarget || Equals(value, default(TTarget))) Remove((TTarget)value);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            var items = Items;
            for (var i = 0; i < items.Count; i++)
            {
                array.SetValue(items[i], i + index);
            }
        }

        public int Count
        {
            get { return Items.Count; }
        }

        public bool IsReadOnly
        {
            get { return ItemsSource.CollectionIsReadOnly() || _convertBackFunc == null; }
        }

        public TTarget this[int index]
        {
            get { return Items[index]; }
            set
            {
                if (IsReadOnly) throw new InvalidOperationException("Source collection is readonly");
                var collection = ItemsSource;
                if (collection == null) throw new InvalidOperationException("Source collection not set");

                var sourceIndex = collection.CollectionGetIndex(ConvertBack(this[index]));
                collection.CollectionReplace(sourceIndex, ConvertBack(value));
            }
        }

        public IEnumerator<TTarget> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(TTarget item)
        {
            if (IsReadOnly) throw new InvalidOperationException("Source collection is readonly");
            var collection = ItemsSource;
            if (collection == null) throw new InvalidOperationException("Source collection not set");
            collection.CollectionAdd(ConvertBack(item));
        }

        public void Clear()
        {
            if (IsReadOnly) throw new InvalidOperationException("Source collection is readonly");
            var collection = ItemsSource;
            if (collection == null) throw new InvalidOperationException("Source collection not set");
            try
            {
                _itemsTracker.Deactivate();
                Items.ToList().ForEach(i => Remove(i));
            }
            finally
            {
                _itemsTracker.Activate();
            }

            DeferResync();
            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            CollectionChanged?.Invoke(this, args);
        }

        public bool Contains(TTarget item)
        {
            return Items.Contains(item);
        }

        public void CopyTo(TTarget[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        public bool Remove(TTarget item)
        {
            if (IsReadOnly) throw new InvalidOperationException("Source collection is readonly");
            var collection = ItemsSource;
            if (collection == null) throw new InvalidOperationException("Source collection not set");

            return collection.CollectionRemove(ConvertBack(item));
        }

        public int IndexOf(TTarget item)
        {
            return Items.IndexOf(item);
        }

        public void Insert(int index, TTarget item)
        {
            var collection = ItemsSource;
            if (collection == null) throw new InvalidOperationException("Source collection not set");

            if (IsReadOnly) throw new InvalidOperationException("Source collection is readonly");

            if (index == Count)
            {
                Add(item);
            }
            else
            {
                var sourceIndex = collection.CollectionGetIndex(ConvertBack(this[index]));
                collection.CollectionInsert(sourceIndex, ConvertBack(item));
            }
        }

        public void RemoveAt(int index)
        {
            if (IsReadOnly) throw new InvalidOperationException("Source collection is readonly");
            var collection = ItemsSource;
            if (collection == null) throw new InvalidOperationException("Source collection not set");

            var sourceIndex = collection.CollectionGetIndex(ConvertBack(this[index]));
            if (sourceIndex != -1) collection.CollectionRemoveAt(index);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnCollectionChanged(object sender, ChangedCollectionEventArgs e)
        {
            if (e.Collection.AreEqual(this)) return;

            NotifyCollectionChangedEventArgs args;

            var newItems = e.EventArgs.NewItems.Enumerate<TSource>().Where(_filterPredicate).Select(Convert).ToList();
            var oldItems = e.EventArgs.OldItems.Enumerate<TSource>().Where(_filterPredicate).Select(Convert).ToList();
            var action = e.EventArgs.Action;

            if (action == NotifyCollectionChangedAction.Add && !newItems.Any()) return;
            if (action == NotifyCollectionChangedAction.Remove && !oldItems.Any()) return;
            if (action == NotifyCollectionChangedAction.Replace)
            {
                if (newItems.Any() && !oldItems.Any()) action = NotifyCollectionChangedAction.Add;
                if (!newItems.Any() && oldItems.Any()) action = NotifyCollectionChangedAction.Remove;
                if (!newItems.Any() && !oldItems.Any()) return;
            }

            if (action == NotifyCollectionChangedAction.Move && !newItems.Any()) return;

            IList<TTarget> removedItems = oldItems;
            int newStartIndex, oldStartIndex;
            switch (action)
            {
                case NotifyCollectionChangedAction.Add:
                    newStartIndex = GetSourceItems().IndexOf(newItems.First());
                    args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems, newStartIndex);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    oldStartIndex = Items.IndexOf(oldItems.First());
                    args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItems, oldStartIndex);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    oldStartIndex = Items.IndexOf(newItems.First());
                    args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItems, oldItems, oldStartIndex);
                    break;
                case NotifyCollectionChangedAction.Move:
                    newStartIndex = GetSourceItems().IndexOf(newItems.First());
                    oldStartIndex = Items.IndexOf(newItems.First());
                    args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, newItems, newStartIndex, oldStartIndex);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                    removedItems = Items;

                    _convertMap.Clear();
                    _convertBackMap.Clear();

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            DeferResync();
            CollectionChanged?.Invoke(this, args);

            if (_disposeAction != null && removedItems.Any())
            {
                foreach (var removedItem in removedItems)
                {
                    _disposeAction(removedItem);
                }
            }
        }

        private void OnPropertyChanged(object sender, ChangedPropertyEventArgs e)
        {
            if (e.PropertyReference.Name.AreEqual(nameof(ItemsSource)))
            {
                _convertMap.Clear();
                _convertBackMap.Clear();

                DeferResync();

                var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                CollectionChanged?.Invoke(this, args);
            }

            if (e.PropertyReference.Name.AreEqual(nameof(IList.Count)) || e.PropertyReference.Name.AreEqual(Route.Create("Item[]")))
            {
                DeferResync();
                OnPropertyChanged(e.PropertyReference.Name);
            }
        }

        public void DeferResync()
        {
            _cachedList = null;
        }

        public TSource GetSourceItem(TTarget container)
        {
            return _convertBackMap.TryGetValue(RuntimeHelpers.GetHashCode(container), out var result) ? result : default;
        }

        public TTarget GetTargetItem(TSource item)
        {
            return _convertMap.TryGetValue(RuntimeHelpers.GetHashCode(item), out var result) ? result : default;
        }

        public void Resync()
        {
            _cachedList = GetSourceItems();
        }

        private TTarget Convert(TSource item)
        {
            return _convertMap.GetOrAdd(RuntimeHelpers.GetHashCode(item),
                                        key =>
                                        {
                                            var wrapped = _convertFunc(item);
                                            _convertBackMap.AddOrUpdate(RuntimeHelpers.GetHashCode(wrapped), k => item, (k, old) => item);
                                            return wrapped;
                                        });
        }

        private TSource ConvertBack(TTarget item)
        {
            if (_convertBackFunc == null) throw new NotSupportedException();

            return _convertBackMap.GetOrAdd(RuntimeHelpers.GetHashCode(item),
                                            key =>
                                            {
                                                var source = _convertBackFunc(item);
                                                _convertMap.AddOrUpdate(RuntimeHelpers.GetHashCode(source), k => item, (k, old) => item);
                                                return source;
                                            });
        }

        private IList<TTarget> GetSourceItems()
        {
            return ItemsSource.Enumerate<TSource>().Where(_filterPredicate).Select(Convert).ToList();
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
