using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace PS.Graph.Algorithms.Observers
{
    /// <summary>
    ///     A distance recorder for directed tree builder algorithms
    /// </summary>
    /// <typeparam name="TVertex">type of a vertex</typeparam>
    /// <typeparam name="TEdge">type of an edge</typeparam>
    [Serializable]
    public sealed class VertexDistanceRecorderObserver<TVertex, TEdge> : IObserver<ITreeBuilderAlgorithm<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        #region Constructors

        public VertexDistanceRecorderObserver(Func<TEdge, double> edgeWeights)
            : this(edgeWeights, DistanceRelaxers.EdgeShortestDistance, new Dictionary<TVertex, double>())
        {
        }

        public VertexDistanceRecorderObserver(
            Func<TEdge, double> edgeWeights,
            IDistanceRelaxer distanceRelaxer,
            IDictionary<TVertex, double> distances)
        {
            Contract.Requires(edgeWeights != null);
            Contract.Requires(distanceRelaxer != null);
            Contract.Requires(distances != null);

            EdgeWeights = edgeWeights;
            DistanceRelaxer = distanceRelaxer;
            Distances = distances;
        }

        #endregion

        #region Properties

        public IDistanceRelaxer DistanceRelaxer { get; }

        public IDictionary<TVertex, double> Distances { get; }

        public Func<TEdge, double> EdgeWeights { get; }

        #endregion

        #region IObserver<ITreeBuilderAlgorithm<TVertex,TEdge>> Members

        public IDisposable Attach(ITreeBuilderAlgorithm<TVertex, TEdge> algorithm)
        {
            algorithm.TreeEdge += TreeEdge;
            return new DisposableAction(
                () => algorithm.TreeEdge -= TreeEdge
            );
        }

        #endregion

        #region Event handlers

        private void TreeEdge(TEdge edge)
        {
            var source = edge.Source;
            var target = edge.Target;

            if (!Distances.TryGetValue(source, out var sourceDistance))
            {
                Distances[source] = sourceDistance = DistanceRelaxer.InitialDistance;
            }

            Distances[target] = DistanceRelaxer.Combine(sourceDistance, EdgeWeights(edge));
        }

        #endregion
    }
}