using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace PS.Graph.Algorithms.Observers
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TVertex">type of a vertex</typeparam>
    /// <reference-ref
    ///     idref="boost" />
    [Serializable]
    public sealed class VertexTimeStamperObserver<TVertex> : IObserver<IVertexTimeStamperAlgorithm<TVertex>>
    {
        private readonly Dictionary<TVertex, int> _discoverTimes;
        private readonly Dictionary<TVertex, int> _finishTimes;
        private int _currentTime;

        #region Constructors

        public VertexTimeStamperObserver()
            : this(new Dictionary<TVertex, int>(), new Dictionary<TVertex, int>())
        {
        }

        public VertexTimeStamperObserver(Dictionary<TVertex, int> discoverTimes)
        {
            Contract.Requires(discoverTimes != null);

            _discoverTimes = discoverTimes;
            _finishTimes = null;
        }

        public VertexTimeStamperObserver(
            Dictionary<TVertex, int> discoverTimes,
            Dictionary<TVertex, int> finishTimes)
        {
            Contract.Requires(discoverTimes != null);
            Contract.Requires(finishTimes != null);

            _discoverTimes = discoverTimes;
            _finishTimes = finishTimes;
        }

        #endregion

        #region Properties

        public IDictionary<TVertex, int> DiscoverTimes
        {
            get { return _discoverTimes; }
        }

        public IDictionary<TVertex, int> FinishTimes
        {
            get { return _finishTimes; }
        }

        #endregion

        #region IObserver<IVertexTimeStamperAlgorithm<TVertex,TEdge>> Members

        public IDisposable Attach(IVertexTimeStamperAlgorithm<TVertex> algorithm)
        {
            algorithm.DiscoverVertex += DiscoverVertex;
            if (_finishTimes != null)
            {
                algorithm.FinishVertex += FinishVertex;
            }

            return new DisposableAction(
                () =>
                {
                    algorithm.DiscoverVertex -= DiscoverVertex;
                    if (_finishTimes != null)
                    {
                        algorithm.FinishVertex -= FinishVertex;
                    }
                });
        }

        #endregion

        #region Event handlers

        private void DiscoverVertex(TVertex v)
        {
            _discoverTimes[v] = _currentTime++;
        }

        private void FinishVertex(TVertex v)
        {
            _finishTimes[v] = _currentTime++;
        }

        #endregion
    }
}