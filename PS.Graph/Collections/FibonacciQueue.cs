using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using PS.Graph.Algorithms;

namespace PS.Graph.Collections
{
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public sealed class FibonacciQueue<TVertex, TDistance> : IPriorityQueue<TVertex>
    {
        private readonly Dictionary<TVertex, FibonacciHeapCell<TDistance, TVertex>> _cells;
        private readonly Func<TVertex, TDistance> _distances;

        private readonly FibonacciHeap<TDistance, TVertex> _heap;

        #region Constructors

        public FibonacciQueue(Func<TVertex, TDistance> distances)
            : this(0, null, distances, Comparer<TDistance>.Default.Compare)
        {
        }

        public FibonacciQueue(
            int valueCount,
            IEnumerable<TVertex> values,
            Func<TVertex, TDistance> distances
        )
            : this(valueCount, values, distances, Comparer<TDistance>.Default.Compare)
        {
        }

        public FibonacciQueue(
            int valueCount,
            IEnumerable<TVertex> values,
            Func<TVertex, TDistance> distances,
            Func<TDistance, TDistance, int> distanceComparison
        )
        {
            _distances = distances;
            _cells = new Dictionary<TVertex, FibonacciHeapCell<TDistance, TVertex>>(valueCount);
            if (valueCount > 0)
            {
                foreach (var x in values)
                {
                    _cells.Add(
                        x,
                        new FibonacciHeapCell<TDistance, TVertex>
                        {
                            Priority = _distances(x),
                            Value = x,
                            Removed = true
                        }
                    );
                }
            }

            _heap = new FibonacciHeap<TDistance, TVertex>(HeapDirection.Increasing, distanceComparison);
        }

        public FibonacciQueue(
            Dictionary<TVertex, TDistance> values,
            Func<TDistance, TDistance, int> distanceComparison
        )
        {
            _distances = AlgorithmExtensions.GetIndexer(values);
            _cells = new Dictionary<TVertex, FibonacciHeapCell<TDistance, TVertex>>(values.Count);
            foreach (var kv in values)
            {
                _cells.Add(
                    kv.Key,
                    new FibonacciHeapCell<TDistance, TVertex>
                    {
                        Priority = kv.Value,
                        Value = kv.Key,
                        Removed = true
                    }
                );
            }

            _heap = new FibonacciHeap<TDistance, TVertex>(HeapDirection.Increasing, distanceComparison);
        }

        public FibonacciQueue(
            Dictionary<TVertex, TDistance> values)
            : this(values, Comparer<TDistance>.Default.Compare)
        {
        }

        #endregion

        #region IPriorityQueue<TVertex> Members

        public int Count
        {
            [Pure] get { return _heap.Count; }
        }

        [Pure]
        public bool Contains(TVertex value)
        {
            return
                _cells.TryGetValue(value, out var result) &&
                !result.Removed;
        }

        public void Update(TVertex v)
        {
            _heap.ChangeKey(_cells[v], _distances(v));
        }

        public void Enqueue(TVertex value)
        {
            _cells[value] = _heap.Enqueue(_distances(value), value);
        }

        public TVertex Dequeue()
        {
            var result = _heap.Top;
            Contract.Assert(result != null);
            _heap.Dequeue();
            return result.Value;
        }

        public TVertex Peek()
        {
            Contract.Assert(_heap.Top != null);

            return _heap.Top.Value;
        }

        [Pure]
        public TVertex[] ToArray()
        {
            var result = new TVertex[_heap.Count];
            var i = 0;
            foreach (var entry in _heap)
            {
                result[i++] = entry.Value;
            }

            return result;
        }

        #endregion
    }
}