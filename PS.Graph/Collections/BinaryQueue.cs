using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace PS.Graph.Collections
{
    [Serializable]
    public sealed class BinaryQueue<TVertex, TDistance> : IPriorityQueue<TVertex>
    {
        private readonly Func<TVertex, TDistance> _distances;
        private readonly BinaryHeap<TDistance, TVertex> _heap;

        #region Constructors

        public BinaryQueue(
            Func<TVertex, TDistance> distances
        )
            : this(distances, Comparer<TDistance>.Default.Compare)
        {
        }

        public BinaryQueue(
            Func<TVertex, TDistance> distances,
            Func<TDistance, TDistance, int> distanceComparison
        )
        {
            Contract.Requires(distances != null);
            Contract.Requires(distanceComparison != null);

            _distances = distances;
            _heap = new BinaryHeap<TDistance, TVertex>(distanceComparison);
        }

        #endregion

        #region IPriorityQueue<TVertex> Members

        public void Update(TVertex v)
        {
            _heap.Update(_distances(v), v);
        }

        public int Count
        {
            get { return _heap.Count; }
        }

        public bool Contains(TVertex value)
        {
            return _heap.IndexOf(value) > -1;
        }

        public void Enqueue(TVertex value)
        {
            _heap.Add(_distances(value), value);
        }

        public TVertex Dequeue()
        {
            return _heap.RemoveMinimum().Value;
        }

        public TVertex Peek()
        {
            return _heap.Minimum().Value;
        }

        public TVertex[] ToArray()
        {
            return _heap.ToValueArray();
        }

        #endregion
    }
}