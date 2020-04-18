using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using PS.Graph.Algorithms.Search;
using PS.Graph.Algorithms.Services;
using PS.Graph.Collections;

namespace PS.Graph.Algorithms.ShortestPath
{
    /// <summary>
    ///     Dijkstra single-source shortest path algorithm for directed graph
    ///     with positive distance.
    /// </summary>
    /// <typeparam name="TVertex">type of a vertex</typeparam>
    /// <typeparam name="TEdge">type of an edge</typeparam>
    /// <reference-ref
    ///     idref="lawler01combinatorial" />
    [Serializable]
    public sealed class DijkstraShortestPathAlgorithm<TVertex, TEdge> : ShortestPathAlgorithmBase<TVertex, TEdge, IVertexListGraph<TVertex, TEdge>>,
                                                                        IVertexColorizerAlgorithm<TVertex>,
                                                                        IVertexPredecessorRecorderAlgorithm<TVertex, TEdge>,
                                                                        IDistanceRecorderAlgorithm<TVertex>
        where TEdge : IEdge<TVertex>
    {
        private FibonacciQueue<TVertex, double> _vertexQueue;

        #region Constructors

        public DijkstraShortestPathAlgorithm(
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> weights)
            : this(visitedGraph, weights, DistanceRelaxers.ShortestDistance)
        {
        }

        public DijkstraShortestPathAlgorithm(
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> weights,
            IDistanceRelaxer distanceRelaxer
        )
            : this(null, visitedGraph, weights, distanceRelaxer)
        {
        }

        public DijkstraShortestPathAlgorithm(
            IAlgorithmComponent host,
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> weights,
            IDistanceRelaxer distanceRelaxer
        )
            : base(host, visitedGraph, weights, distanceRelaxer)
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
            }

            _vertexQueue = new FibonacciQueue<TVertex, double>(DistancesIndexGetter());
        }

        protected override void InternalCompute()
        {
            if (TryGetRootVertex(out var rootVertex))
            {
                ComputeFromRoot(rootVertex);
            }
            else
            {
                foreach (var v in VisitedGraph.Vertices)
                {
                    if (VertexColors[v] == GraphColor.White)
                    {
                        ComputeFromRoot(v);
                    }
                }
            }
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

        #region Event handlers

        private void InternalExamineEdge(TEdge args)
        {
            if (Weights(args) < 0)
            {
                throw new NegativeWeightException();
            }
        }

        private void InternalGrayTarget(TEdge edge)
        {
            var decreased = Relax(edge);
            if (decreased)
            {
                _vertexQueue.Update(edge.Target);
                AssertHeap();
                OnTreeEdge(edge);
            }
            else
            {
                OnEdgeNotRelaxed(edge);
            }
        }

        private void InternalTreeEdge(TEdge args)
        {
            var decreased = Relax(args);
            if (decreased)
            {
                OnTreeEdge(args);
                AssertHeap();
            }
            else
            {
                OnEdgeNotRelaxed(args);
            }
        }

        #endregion

        #region Members

        public void ComputeNoInit(TVertex s)
        {
            BreadthFirstSearchAlgorithm<TVertex, TEdge> bfs = null;

            try
            {
                bfs = new BreadthFirstSearchAlgorithm<TVertex, TEdge>(
                    this,
                    VisitedGraph,
                    _vertexQueue,
                    VertexColors
                );

                bfs.InitializeVertex += InitializeVertex;
                bfs.DiscoverVertex += DiscoverVertex;
                bfs.StartVertex += StartVertex;
                bfs.ExamineEdge += ExamineEdge;
                #if SUPERDEBUG
                bfs.ExamineEdge += e => this.AssertHeap();
                #endif
                bfs.ExamineVertex += ExamineVertex;
                bfs.FinishVertex += FinishVertex;

                bfs.ExamineEdge += InternalExamineEdge;
                bfs.TreeEdge += InternalTreeEdge;
                bfs.GrayTarget += InternalGrayTarget;

                bfs.Visit(s);
            }
            finally
            {
                if (bfs != null)
                {
                    bfs.InitializeVertex -= InitializeVertex;
                    bfs.DiscoverVertex -= DiscoverVertex;
                    bfs.StartVertex -= StartVertex;
                    bfs.ExamineEdge -= ExamineEdge;
                    bfs.ExamineVertex -= ExamineVertex;
                    bfs.FinishVertex -= FinishVertex;

                    bfs.ExamineEdge -= InternalExamineEdge;
                    bfs.TreeEdge -= InternalTreeEdge;
                    bfs.GrayTarget -= InternalGrayTarget;
                }
            }
        }

        [Conditional("DEBUG")]
        private void AssertHeap()
        {
            if (_vertexQueue.Count == 0) return;
            var top = _vertexQueue.Peek();
            var vertices = _vertexQueue.ToArray();
            for (var i = 1; i < vertices.Length; ++i)
            {
                if (Distances[top] > Distances[vertices[i]])
                {
                    Contract.Assert(false);
                }
            }
        }

        private void ComputeFromRoot(TVertex rootVertex)
        {
            VertexColors[rootVertex] = GraphColor.Gray;
            Distances[rootVertex] = 0;
            ComputeNoInit(rootVertex);
        }

        private void OnEdgeNotRelaxed(TEdge e)
        {
            var eh = EdgeNotRelaxed;
            eh?.Invoke(e);
        }

        #endregion
    }
}