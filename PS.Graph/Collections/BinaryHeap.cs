using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace PS.Graph.Collections
{
    /// <summary>
    ///     Binary heap
    /// </summary>
    /// <remarks>
    ///     Indexing rules:
    ///     parent index: index ¡ 1)/2
    ///     left child: 2 * index + 1
    ///     right child: 2 * index + 2
    ///     Reference:
    ///     http://dotnetslackers.com/Community/files/folders/data-structures-and-algorithms/entry28722.aspx
    /// </remarks>
    /// <typeparam name="TValue">type of the value</typeparam>
    /// <typeparam name="TPriority">type of the priority metric</typeparam>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class BinaryHeap<TPriority, TValue> : IEnumerable<KeyValuePair<TPriority, TValue>>
    {
        private KeyValuePair<TPriority, TValue>[] _items;
        private int _version;

        private const int DefaultCapacity = 16;

        public BinaryHeap()
            : this(DefaultCapacity, Comparer<TPriority>.Default.Compare)
        {
        }

        public BinaryHeap(Func<TPriority, TPriority, int> priorityComparison)
            : this(DefaultCapacity, priorityComparison)
        {
        }

        public BinaryHeap(int capacity, Func<TPriority, TPriority, int> priorityComparison)
        {
            _items = new KeyValuePair<TPriority, TValue>[capacity];
            PriorityComparison = priorityComparison;
        }

        public Func<TPriority, TPriority, int> PriorityComparison { get; }

        public int Capacity
        {
            get { return _items.Length; }
        }

        public int Count { get; private set; }

        public void Add(TPriority priority, TValue value)
        {
            _version++;
            ResizeArray();
            _items[Count++] = new KeyValuePair<TPriority, TValue>(priority, value);
            MinHeapifyDown(Count - 1);
        }

        private void MinHeapifyDown(int start)
        {
            var i = start;
            var j = (i - 1) / 2;
            while (i > 0 &&
                   Less(i, j))
            {
                Swap(i, j);
                i = j;
                j = (i - 1) / 2;
            }
        }

        public TValue[] ToValueArray()
        {
            var values = new TValue[_items.Length];
            for (var i = 0; i < values.Length; ++i)
            {
                values[i] = _items[i].Value;
            }

            return values;
        }

        private void ResizeArray()
        {
            if (Count == _items.Length)
            {
                var newItems = new KeyValuePair<TPriority, TValue>[Count * 2 + 1];
                Array.Copy(_items, newItems, Count);
                _items = newItems;
            }
        }

        public KeyValuePair<TPriority, TValue> Minimum()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException();
            }

            return _items[0];
        }

        public KeyValuePair<TPriority, TValue> RemoveMinimum()
        {
            // shortcut for heap with 1 element.
            if (Count == 1)
            {
                _version++;
                return _items[--Count];
            }

            return RemoveAt(0);
        }

        public KeyValuePair<TPriority, TValue> RemoveAt(int index)
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("heap is empty");
            }

            if ((index < 0) | (index >= Count) | (index + Count < Count))
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            _version++;
            // shortcut for heap with 1 element.
            if (Count == 1)
            {
                return _items[--Count];
            }

            if (index < Count - 1)
            {
                Swap(index, Count - 1);
                MinHeapifyUp(index);
            }

            return _items[--Count];
        }

        private void MinHeapifyUp(int index)
        {
            var left = 2 * index + 1;
            var right = 2 * index + 2;
            while (
                left < Count - 1 && !Less(index, left) ||
                right < Count - 1 && !Less(index, right)
            )
            {
                if (right >= Count - 1 ||
                    Less(left, right))
                {
                    Swap(left, index);
                    index = left;
                }
                else
                {
                    Swap(right, index);
                    index = right;
                }

                left = 2 * index + 1;
                right = 2 * index + 2;
            }
        }

        public int IndexOf(TValue value)
        {
            for (var i = 0; i < Count; i++)
            {
                if (Equals(value, _items[i].Value))
                {
                    return i;
                }
            }

            return -1;
        }

        public bool MinimumUpdate(TPriority priority, TValue value)
        {
            // find index
            for (var i = 0; i < Count; i++)
            {
                if (Equals(value, _items[i].Value))
                {
                    if (PriorityComparison(priority, _items[i].Key) <= 0)
                    {
                        RemoveAt(i);
                        Add(priority, value);
                        return true;
                    }

                    return false;
                }
            }

            // not in collection
            Add(priority, value);
            return true;
        }

        public void Update(TPriority priority, TValue value)
        {
            // find index
            var index = IndexOf(value);
            // remove if needed
            if (index > -1)
            {
                RemoveAt(index);
            }

            Add(priority, value);
        }

        private bool Less(int i, int j)
        {
            return PriorityComparison(_items[i].Key, _items[j].Key) <= 0;
        }

        private void Swap(int i, int j)
        {
            var kv = _items[i];
            _items[i] = _items[j];
            _items[j] = kv;
        }

        #if DEEP_INVARIANT
        )
            );
        }
        #endif

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public IEnumerator<KeyValuePair<TPriority, TValue>> GetEnumerator()
        {
            return new Enumerator(this);
        }

        private struct Enumerator :
            IEnumerator<KeyValuePair<TPriority, TValue>>
        {
            private readonly int _count;
            private readonly int _version;
            private int _index;
            private KeyValuePair<TPriority, TValue>[] _items;
            private BinaryHeap<TPriority, TValue> _owner;

            #region Constructors

            public Enumerator(BinaryHeap<TPriority, TValue> owner)
            {
                _owner = owner;
                _items = owner._items;
                _count = owner.Count;
                _version = owner._version;
                _index = -1;
            }

            #endregion

            #region IEnumerator<KeyValuePair<TPriority,TValue>> Members

            public KeyValuePair<TPriority, TValue> Current
            {
                get
                {
                    if (_version != _owner._version)
                    {
                        throw new InvalidOperationException();
                    }

                    if ((_index < 0) | (_index == _count))
                    {
                        throw new InvalidOperationException();
                    }

                    return _items[_index];
                }
            }

            void IDisposable.Dispose()
            {
                _owner = null;
                _items = null;
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                if (_version != _owner._version)
                {
                    throw new InvalidOperationException();
                }

                return ++_index < _count;
            }

            public void Reset()
            {
                if (_version != _owner._version)
                {
                    throw new InvalidOperationException();
                }

                _index = -1;
            }

            #endregion
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}