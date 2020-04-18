using System;
using System.Collections.Generic;
using PS.Graph.Algorithms.Observers;
using PS.Graph.Algorithms.ShortestPath;

namespace PS.Graph.Algorithms
{
    [Serializable]
    public sealed class CentralityApproximationAlgorithm<TVertex, TEdge> : AlgorithmBase<IVertexListGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private IDictionary<TVertex, double> _centralities = new Dictionary<TVertex, double>();
        private DijkstraShortestPathAlgorithm<TVertex, TEdge> _dijkstra;
        private VertexPredecessorRecorderObserver<TVertex, TEdge> _predecessorRecorder;

        #region Constructors

        public CentralityApproximationAlgorithm(
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> distances
        )
            : base(visitedGraph)
        {
            MaxIterationCount = 50;
            Rand = new Random();

            _dijkstra = new DijkstraShortestPathAlgorithm<TVertex, TEdge>(
                VisitedGraph,
                distances,
                DistanceRelaxers.ShortestDistance
            );
            _predecessorRecorder = new VertexPredecessorRecorderObserver<TVertex, TEdge>();
            _predecessorRecorder.Attach(_dijkstra);
        }

        #endregion

        #region Properties

        public Func<TEdge, double> Distances
        {
            get { return _dijkstra.Weights; }
        }

        public int MaxIterationCount { get; set; }

        public Random Rand { get; set; }

        #endregion

        #region Override members

        protected override void Initialize()
        {
            base.Initialize();
            _centralities.Clear();
            foreach (var v in VisitedGraph.Vertices)
            {
                _centralities.Add(v, 0);
            }
        }

        protected override void InternalCompute()
        {
            if (VisitedGraph.VertexCount == 0)
            {
                return;
            }

            // compute temporary values
            var n = VisitedGraph.VertexCount;
            for (var i = 0; i < MaxIterationCount; ++i)
            {
                var v = RandomGraphFactory.GetVertex(VisitedGraph, Rand);
                _dijkstra.Compute(v);

                foreach (var u in VisitedGraph.Vertices)
                {
                    if (_dijkstra.TryGetDistance(u, out var d))
                    {
                        _centralities[u] += n * d / (MaxIterationCount * (n - 1));
                    }
                }
            }

            // update
            foreach (var v in _centralities.Keys)
            {
                _centralities[v] = 1.0 / _centralities[v];
            }
        }

        #endregion
    }
}