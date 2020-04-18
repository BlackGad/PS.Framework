using System;
using PS.Graph.Algorithms.Services;

namespace PS.Graph.Algorithms.ShortestPath
{
    /// <summary>
    ///     A single-source shortest path algorithm for directed acyclic
    ///     graph.
    /// </summary>
    /// <typeparam name="TVertex">type of a vertex</typeparam>
    /// <typeparam name="TEdge">type of an edge</typeparam>
    /// <reference-ref
    ///     id="boost" />
    [Serializable]
    public sealed class DagShortestPathAlgorithm<TVertex, TEdge> : ShortestPathAlgorithmBase<TVertex, TEdge, IVertexListGraph<TVertex, TEdge>>,
                                                                   IVertexColorizerAlgorithm<TVertex>,
                                                                   IDistanceRecorderAlgorithm<TVertex>,
                                                                   IVertexPredecessorRecorderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Constructors

        public DagShortestPathAlgorithm(
            IVertexListGraph<TVertex, TEdge> g,
            Func<TEdge, double> weights
        )
            : this(g, weights, DistanceRelaxers.ShortestDistance)
        {
        }

        public DagShortestPathAlgorithm(
            IVertexListGraph<TVertex, TEdge> g,
            Func<TEdge, double> weights,
            IDistanceRelaxer distanceRelaxer
        )
            : this(null, g, weights, distanceRelaxer)
        {
        }

        public DagShortestPathAlgorithm(
            IAlgorithmComponent host,
            IVertexListGraph<TVertex, TEdge> g,
            Func<TEdge, double> weights,
            IDistanceRelaxer distanceRelaxer
        )
            : base(host, g, weights, distanceRelaxer)
        {
        }

        #endregion

        #region Events

        public event EdgeAction<TVertex, TEdge> EdgeNotRelaxed;

        public event EdgeAction<TVertex, TEdge> ExamineEdge;

        public event VertexAction<TVertex> ExamineVertex;

        #endregion

        #region Override members

        protected override void Initialize()
        {
            base.Initialize();
            // init color, distance
            var initialDistance = DistanceRelaxer.InitialDistance;
            foreach (var u in VisitedGraph.Vertices)
            {
                VertexColors.Add(u, GraphColor.White);
                Distances.Add(u, initialDistance);
                OnInitializeVertex(u);
            }
        }

        protected override void InternalCompute()
        {
            if (!TryGetRootVertex(out var rootVertex))
            {
                throw new InvalidOperationException("RootVertex not initialized");
            }

            VertexColors[rootVertex] = GraphColor.Gray;
            Distances[rootVertex] = 0;
            ComputeNoInit(rootVertex);
        }

        #endregion

        #region IDistanceRecorderAlgorithm<TVertex> Members

        public event VertexAction<TVertex> InitializeVertex;

        public event VertexAction<TVertex> DiscoverVertex;

        #endregion

        #region IVertexPredecessorRecorderAlgorithm<TVertex,TEdge> Members

        public event VertexAction<TVertex> StartVertex;

        public event VertexAction<TVertex> FinishVertex;

        #endregion

        #region Members

        public void ComputeNoInit(TVertex s)
        {
            var orderedVertices = VisitedGraph.TopologicalSort();

            OnDiscoverVertex(s);
            foreach (var v in orderedVertices)
            {
                OnExamineVertex(v);
                foreach (var e in VisitedGraph.OutEdges(v))
                {
                    OnDiscoverVertex(e.Target);
                    var decreased = Relax(e);
                    if (decreased)
                    {
                        OnTreeEdge(e);
                    }
                    else
                    {
                        OnEdgeNotRelaxed(e);
                    }
                }

                OnFinishVertex(v);
            }
        }

        private void OnDiscoverVertex(TVertex v)
        {
            DiscoverVertex?.Invoke(v);
        }

        private void OnEdgeNotRelaxed(TEdge e)
        {
            EdgeNotRelaxed?.Invoke(e);
        }

        private void OnExamineEdge(TEdge e)
        {
            ExamineEdge?.Invoke(e);
        }

        private void OnExamineVertex(TVertex v)
        {
            ExamineVertex?.Invoke(v);
        }

        private void OnFinishVertex(TVertex v)
        {
            FinishVertex?.Invoke(v);
        }

        private void OnInitializeVertex(TVertex v)
        {
            InitializeVertex?.Invoke(v);
        }

        private void OnStartVertex(TVertex v)
        {
            var eh = StartVertex;
            eh?.Invoke(v);
        }

        #endregion
    }
}