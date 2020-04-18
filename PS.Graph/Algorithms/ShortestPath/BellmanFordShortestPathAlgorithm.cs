using System;
using System.Collections.Generic;
using PS.Graph.Algorithms.Services;

namespace PS.Graph.Algorithms.ShortestPath
{
    /// <summary>
    ///     Bellman Ford shortest path algorithm.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The Bellman-Ford algorithm solves the single-source shortest paths
    ///         problem for a graph with both positive and negative edge weights.
    ///     </para>
    ///     <para>
    ///         If you only need to solve the shortest paths problem for positive
    ///         edge weights, Dijkstra's algorithm provides a more efficient
    ///         alternative.
    ///     </para>
    ///     <para>
    ///         If all the edge weights are all equal to one then breadth-first search
    ///         provides an even more efficient alternative.
    ///     </para>
    /// </remarks>
    /// <reference-ref
    ///     idref="shi03datastructures" />
    public sealed class BellmanFordShortestPathAlgorithm<TVertex, TEdge> : ShortestPathAlgorithmBase<TVertex, TEdge, IVertexAndEdgeListGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private readonly Dictionary<TVertex, TVertex> _predecessors;

        #region Constructors

        public BellmanFordShortestPathAlgorithm(
            IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> weights
        )
            : this(visitedGraph, weights, DistanceRelaxers.ShortestDistance)
        {
        }

        public BellmanFordShortestPathAlgorithm(
            IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> weights,
            IDistanceRelaxer distanceRelaxer
        )
            : this(null, visitedGraph, weights, distanceRelaxer)
        {
        }

        public BellmanFordShortestPathAlgorithm(
            IAlgorithmComponent host,
            IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> weights,
            IDistanceRelaxer distanceRelaxer
        )
            : base(host, visitedGraph, weights, distanceRelaxer)
        {
            _predecessors = new Dictionary<TVertex, TVertex>();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Indicates if a negative cycle was found
        /// </summary>
        public bool FoundNegativeCycle { get; private set; }

        /// <summary>
        ///     Constructed predecessor map
        /// </summary>
        public IDictionary<TVertex, TVertex> Predecessors
        {
            get { return _predecessors; }
        }

        #endregion

        #region Events

        /// <summary>
        ///     Invoked during the second stage of the algorithm,
        ///     during the test of whether each edge was minimized.
        ///     If the edge is minimized then this function is invoked.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> EdgeMinimized;

        /// <summary>
        ///     Invoked during the second stage of the algorithm,
        ///     during the test of whether each edge was minimized.
        ///     If the edge was not minimized, this function is invoked.
        ///     This happens when there is a negative cycle in the graph.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> EdgeNotMinimized;

        /// <summary>
        ///     Invoked if the distance label for the target vertex is not
        ///     decreased.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> EdgeNotRelaxed;

        /// <summary>
        ///     Invoked on every edge in the graph |V| times.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> ExamineEdge;

        /// <summary>
        ///     Invoked on each vertex in the graph before the start of the
        ///     algorithm.
        /// </summary>
        public event VertexAction<TVertex> InitializeVertex;

        #endregion

        #region Override members

        protected override void Initialize()
        {
            base.Initialize();

            FoundNegativeCycle = false;
            // init color, distance
            VertexColors.Clear();
            foreach (var u in VisitedGraph.Vertices)
            {
                VertexColors[u] = GraphColor.White;
                Distances[u] = double.PositiveInfinity;
                OnInitializeVertex(u);
            }

            if (!TryGetRootVertex(out var root))
            {
                foreach (var v in VisitedGraph.Vertices)
                {
                    root = v;
                    break;
                }
            }

            Distances[root] = 0;
        }

        /// <summary>
        ///     Applies the Bellman Ford algorithm
        /// </summary>
        /// <remarks>
        ///     Does not initialize the predecessor and distance map.
        /// </remarks>
        protected override void InternalCompute()
        {
            // getting the number of 
            var n = VisitedGraph.VertexCount;
            for (var k = 0; k < n; ++k)
            {
                var atLeastOneTreeEdge = false;
                foreach (var e in VisitedGraph.Edges)
                {
                    OnExamineEdge(e);

                    if (Relax(e))
                    {
                        atLeastOneTreeEdge = true;
                        OnTreeEdge(e);
                    }
                    else
                    {
                        OnEdgeNotRelaxed(e);
                    }
                }

                if (!atLeastOneTreeEdge)
                {
                    break;
                }
            }

            var relaxer = DistanceRelaxer;
            foreach (var e in VisitedGraph.Edges)
            {
                var edgeWeight = Weights(e);
                if (relaxer.Compare(
                        relaxer.Combine(
                            Distances[e.Source],
                            edgeWeight),
                        Distances[e.Target]
                    ) < 0
                )
                {
                    OnEdgeMinimized(e);
                    FoundNegativeCycle = true;
                    return;
                }

                OnEdgeNotMinimized(e);
            }

            FoundNegativeCycle = false;
        }

        #endregion

        #region Members

        /// <summary>
        ///     Raises the <see cref="EdgeMinimized" /> event.
        /// </summary>
        /// <param name="e">edge that raised the event</param>
        private void OnEdgeMinimized(TEdge e)
        {
            var eh = EdgeMinimized;
            eh?.Invoke(e);
        }

        /// <summary>
        ///     Raises the <see cref="EdgeNotMinimized" /> event.
        /// </summary>
        /// <param name="e">edge that raised the event</param>
        private void OnEdgeNotMinimized(TEdge e)
        {
            var eh = EdgeNotMinimized;
            eh?.Invoke(e);
        }

        /// <summary>
        ///     Raises the <see cref="EdgeNotRelaxed" /> event.
        /// </summary>
        /// <param name="e">edge that raised the event</param>
        private void OnEdgeNotRelaxed(TEdge e)
        {
            var eh = EdgeNotRelaxed;
            eh?.Invoke(e);
        }

        /// <summary>
        ///     Raises the <see cref="ExamineEdge" /> event.
        /// </summary>
        /// <param name="e">edge that raised the event</param>
        private void OnExamineEdge(TEdge e)
        {
            var eh = ExamineEdge;
            eh?.Invoke(e);
        }

        /// <summary>
        ///     Raises the <see cref="InitializeVertex" /> event.
        /// </summary>
        /// <param name="v">vertex that raised the event</param>
        private void OnInitializeVertex(TVertex v)
        {
            var eh = InitializeVertex;
            eh?.Invoke(v);
        }

        #endregion
    }
}