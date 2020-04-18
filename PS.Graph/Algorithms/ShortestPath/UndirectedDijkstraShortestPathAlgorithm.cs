using System;
using System.Diagnostics;
using PS.Graph.Algorithms.Search;
using PS.Graph.Algorithms.Services;
using PS.Graph.Collections;

namespace PS.Graph.Algorithms.ShortestPath
{
    /// <summary>
    ///     A single-source shortest path algorithm for undirected graph
    ///     with positive distance.
    /// </summary>
    /// <reference-ref
    ///     idref="lawler01combinatorial" />
    [Serializable]
    public sealed class UndirectedDijkstraShortestPathAlgorithm<TVertex, TEdge> : UndirectedShortestPathAlgorithmBase<TVertex, TEdge>,
                                                                                  IVertexColorizerAlgorithm<TVertex>,
                                                                                  IUndirectedVertexPredecessorRecorderAlgorithm<TVertex, TEdge>,
                                                                                  IDistanceRecorderAlgorithm<TVertex>
        where TEdge : IEdge<TVertex>
    {
        private IPriorityQueue<TVertex> _vertexQueue;

        #region Constructors

        public UndirectedDijkstraShortestPathAlgorithm(
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> weights)
            : this(visitedGraph, weights, DistanceRelaxers.ShortestDistance)
        {
        }

        public UndirectedDijkstraShortestPathAlgorithm(
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> weights,
            IDistanceRelaxer distanceRelaxer
        )
            : this(null, visitedGraph, weights, distanceRelaxer)
        {
        }

        public UndirectedDijkstraShortestPathAlgorithm(
            IAlgorithmComponent host,
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> weights,
            IDistanceRelaxer distanceRelaxer
        )
            : base(host, visitedGraph, weights, distanceRelaxer)
        {
        }

        #endregion

        #region Events

        public event UndirectedEdgeAction<TVertex, TEdge> EdgeNotRelaxed;
        public event EdgeAction<TVertex, TEdge> ExamineEdge;
        public event VertexAction<TVertex> ExamineVertex;

        #endregion

        #region Override members

        protected override void Initialize()
        {
            base.Initialize();

            var initialDistance = DistanceRelaxer.InitialDistance;
            // init color, distance
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

        #region IUndirectedVertexPredecessorRecorderAlgorithm<TVertex,TEdge> Members

        public event VertexAction<TVertex> StartVertex;
        public event VertexAction<TVertex> FinishVertex;

        #endregion

        #region Event handlers

        private void InternalGrayTarget(Object sender, UndirectedEdgeEventArgs<TVertex, TEdge> args)
        {
            var decreased = Relax(args.Edge, args.Source, args.Target);
            if (decreased)
            {
                _vertexQueue.Update(args.Target);
                AssertHeap();
                OnTreeEdge(args.Edge, args.Reversed);
            }
            else
            {
                OnEdgeNotRelaxed(args.Edge, args.Reversed);
            }
        }

        private void InternalTreeEdge(Object sender, UndirectedEdgeEventArgs<TVertex, TEdge> args)
        {
            var decreased = Relax(args.Edge, args.Source, args.Target);
            if (decreased)
            {
                OnTreeEdge(args.Edge, args.Reversed);
            }
            else
            {
                OnEdgeNotRelaxed(args.Edge, args.Reversed);
            }
        }

        #endregion

        #region Members

        public void ComputeNoInit(TVertex s)
        {
            UndirectedBreathFirstSearchAlgorithm<TVertex, TEdge> bfs = null;
            try
            {
                bfs = new UndirectedBreathFirstSearchAlgorithm<TVertex, TEdge>(
                    this,
                    VisitedGraph,
                    _vertexQueue,
                    VertexColors
                );

                bfs.InitializeVertex += InitializeVertex;
                bfs.DiscoverVertex += DiscoverVertex;
                bfs.StartVertex += StartVertex;
                bfs.ExamineEdge += ExamineEdge;
                #if DEBUG
                bfs.ExamineEdge += e => AssertHeap();
                #endif
                bfs.ExamineVertex += ExamineVertex;
                bfs.FinishVertex += FinishVertex;

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
                }
            }
        }

        private void ComputeFromRoot(TVertex rootVertex)
        {
            VertexColors[rootVertex] = GraphColor.Gray;
            Distances[rootVertex] = 0;
            ComputeNoInit(rootVertex);
        }

        private void OnEdgeNotRelaxed(TEdge e, bool reversed)
        {
            var eh = EdgeNotRelaxed;
            eh?.Invoke(this, new UndirectedEdgeEventArgs<TVertex, TEdge>(e, reversed));
        }

        #endregion
    }
}