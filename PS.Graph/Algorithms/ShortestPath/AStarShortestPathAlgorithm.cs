using System;
using System.Collections.Generic;
using PS.Graph.Algorithms.Search;
using PS.Graph.Algorithms.Services;
using PS.Graph.Collections;

namespace PS.Graph.Algorithms.ShortestPath
{
    /// <summary>
    ///     A* single-source shortest path algorithm for directed graph
    ///     with positive distance.
    /// </summary>
    /// <typeparam name="TVertex">type of a vertex</typeparam>
    /// <typeparam name="TEdge">type of an edge</typeparam>
    /// <reference-ref
    ///     idref="lawler01combinatorial" />
    [Serializable]
    public sealed class AStarShortestPathAlgorithm<TVertex, TEdge> : ShortestPathAlgorithmBase<TVertex, TEdge, IVertexListGraph<TVertex, TEdge>>,
                                                                     IVertexColorizerAlgorithm<TVertex>,
                                                                     IVertexPredecessorRecorderAlgorithm<TVertex, TEdge>,
                                                                     IDistanceRecorderAlgorithm<TVertex>
        where TEdge : IEdge<TVertex>
    {
        private Dictionary<TVertex, double> _costs;
        private FibonacciQueue<TVertex, double> _vertexQueue;

        #region Constructors

        public AStarShortestPathAlgorithm(
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> weights,
            Func<TVertex, double> costHeuristic
        )
            : this(visitedGraph, weights, costHeuristic, DistanceRelaxers.ShortestDistance)
        {
        }

        public AStarShortestPathAlgorithm(
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> weights,
            Func<TVertex, double> costHeuristic,
            IDistanceRelaxer distanceRelaxer
        )
            : this(null, visitedGraph, weights, costHeuristic, distanceRelaxer)
        {
        }

        public AStarShortestPathAlgorithm(
            IAlgorithmComponent host,
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> weights,
            Func<TVertex, double> costHeuristic,
            IDistanceRelaxer distanceRelaxer
        )
            : base(host, visitedGraph, weights, distanceRelaxer)
        {
            CostHeuristic = costHeuristic;
        }

        #endregion

        #region Properties

        public Func<TVertex, double> CostHeuristic { get; }

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

            VertexColors.Clear();
            _costs = new Dictionary<TVertex, double>(VisitedGraph.VertexCount);
            // init color, distance
            var initialDistance = DistanceRelaxer.InitialDistance;
            foreach (var u in VisitedGraph.Vertices)
            {
                VertexColors.Add(u, GraphColor.White);
                Distances.Add(u, initialDistance);
                _costs.Add(u, initialDistance);
            }

            _vertexQueue = new FibonacciQueue<TVertex, double>(_costs, DistanceRelaxer.Compare);
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

        private void InternalBlackTarget(TEdge e)
        {
            var target = e.Target;

            var decreased = Relax(e);
            var distance = Distances[target];
            if (decreased)
            {
                OnTreeEdge(e);
                _costs[target] = DistanceRelaxer.Combine(distance, CostHeuristic(target));
                _vertexQueue.Enqueue(target);
                VertexColors[target] = GraphColor.Gray;
            }
            else
            {
                OnEdgeNotRelaxed(e);
            }
        }

        private void InternalExamineEdge(TEdge args)
        {
            if (Weights(args) < 0)
            {
                throw new NegativeWeightException();
            }
        }

        private void InternalGrayTarget(TEdge e)
        {
            var target = e.Target;

            var decreased = Relax(e);
            var distance = Distances[target];
            if (decreased)
            {
                _costs[target] = DistanceRelaxer.Combine(distance, CostHeuristic(target));
                _vertexQueue.Update(target);
                OnTreeEdge(e);
            }
            else
            {
                OnEdgeNotRelaxed(e);
            }
        }

        private void InternalTreeEdge(TEdge args)
        {
            var decreased = Relax(args);
            if (decreased)
            {
                OnTreeEdge(args);
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
            BreathFirstSearchAlgorithm<TVertex, TEdge> bfs = null;

            try
            {
                bfs = new BreathFirstSearchAlgorithm<TVertex, TEdge>(
                    this,
                    VisitedGraph,
                    _vertexQueue,
                    VertexColors
                );

                bfs.InitializeVertex += InitializeVertex;
                bfs.DiscoverVertex += DiscoverVertex;
                bfs.StartVertex += StartVertex;
                bfs.ExamineEdge += ExamineEdge;
                bfs.ExamineVertex += ExamineVertex;
                bfs.FinishVertex += FinishVertex;

                bfs.ExamineEdge += InternalExamineEdge;
                bfs.TreeEdge += InternalTreeEdge;
                bfs.GrayTarget += InternalGrayTarget;
                bfs.BlackTarget += InternalBlackTarget;

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
                    bfs.BlackTarget -= InternalBlackTarget;
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