using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace PS.Graph.Algorithms.Observers
{
    /// <summary>
    ///     A distance recorder for undirected tree builder algorithms
    /// </summary>
    /// <typeparam name="TVertex">type of a vertex</typeparam>
    /// <typeparam name="TEdge">type of an edge</typeparam>
    [Serializable]
    public sealed class UndirectedVertexDistanceRecorderObserver<TVertex, TEdge> : IObserver<IUndirectedTreeBuilderAlgorithm<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        #region Constructors

        public UndirectedVertexDistanceRecorderObserver(Func<TEdge, double> edgeWeights)
            : this(edgeWeights, DistanceRelaxers.EdgeShortestDistance, new Dictionary<TVertex, double>())
        {
        }

        public UndirectedVertexDistanceRecorderObserver(
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

        #region IObserver<IUndirectedTreeBuilderAlgorithm<TVertex,TEdge>> Members

        public IDisposable Attach(IUndirectedTreeBuilderAlgorithm<TVertex, TEdge> algorithm)
        {
            algorithm.TreeEdge += TreeEdge;
            return new DisposableAction(
                () => algorithm.TreeEdge -= TreeEdge
            );
        }

        #endregion

        #region Event handlers

        private void TreeEdge(Object sender, UndirectedEdgeEventArgs<TVertex, TEdge> args)
        {
            if (!Distances.TryGetValue(args.Source, out var sourceDistance))
            {
                Distances[args.Source] = sourceDistance = DistanceRelaxer.InitialDistance;
            }

            Distances[args.Target] = DistanceRelaxer.Combine(sourceDistance, EdgeWeights(args.Edge));
        }

        #endregion
    }
}