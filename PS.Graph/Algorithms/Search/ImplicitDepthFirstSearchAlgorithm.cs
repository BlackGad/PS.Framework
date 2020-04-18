using System;
using System.Collections.Generic;
using PS.Graph.Algorithms.Services;

namespace PS.Graph.Algorithms.Search
{
    /// <summary>
    ///     A depth first search algorithm for implicit directed graphs
    /// </summary>
    /// <reference-ref
    ///     idref="gross98graphtheory"
    ///     chapter="4.2" />
    [Serializable]
    public sealed class ImplicitDepthFirstSearchAlgorithm<TVertex, TEdge> : RootedAlgorithmBase<TVertex, IIncidenceGraph<TVertex, TEdge>>,
                                                                            IVertexPredecessorRecorderAlgorithm<TVertex, TEdge>,
                                                                            IVertexTimeStamperAlgorithm<TVertex>
        where TEdge : IEdge<TVertex>
    {
        private int _maxDepth = int.MaxValue;

        #region Constructors

        public ImplicitDepthFirstSearchAlgorithm(
            IIncidenceGraph<TVertex, TEdge> visitedGraph)
            : this(null, visitedGraph)
        {
        }

        public ImplicitDepthFirstSearchAlgorithm(
            IAlgorithmComponent host,
            IIncidenceGraph<TVertex, TEdge> visitedGraph)
            : base(host, visitedGraph)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the maximum exploration depth, from
        ///     the start vertex.
        /// </summary>
        /// <remarks>
        ///     Defaulted at <c>int.MaxValue</c>.
        /// </remarks>
        /// <value>
        ///     Maximum exploration depth.
        /// </value>
        public int MaxDepth
        {
            get { return _maxDepth; }
            set { _maxDepth = value; }
        }

        /// <summary>
        ///     Gets the vertex color map
        /// </summary>
        /// <value>
        ///     Vertex color (<see cref="GraphColor" />) dictionary
        /// </value>
        public IDictionary<TVertex, GraphColor> VertexColors { get; } = new Dictionary<TVertex, GraphColor>();

        #endregion

        #region Events

        /// <summary>
        ///     Invoked on the back edges in the graph.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> BackEdge;

        /// <summary>
        ///     Invoked on every out-edge of each vertex after it is discovered.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> ExamineEdge;

        /// <summary>
        ///     Invoked on forward or cross edges in the graph.
        ///     (In an undirected graph this method is never called.)
        /// </summary>
        public event EdgeAction<TVertex, TEdge> ForwardOrCrossEdge;

        #endregion

        #region Override members

        protected override void InternalCompute()
        {
            if (!TryGetRootVertex(out var rootVertex))
            {
                throw new InvalidOperationException("root vertex not set");
            }

            Initialize();
            Visit(rootVertex, 0);
        }

        protected override void Initialize()
        {
            base.Initialize();

            VertexColors.Clear();
        }

        #endregion

        #region IVertexPredecessorRecorderAlgorithm<TVertex,TEdge> Members

        /// <summary>
        ///     Invoked on the source vertex once before the start of the search.
        /// </summary>
        public event VertexAction<TVertex> StartVertex;

        /// <summary>
        ///     Invoked on each edge as it becomes a member of the edges that form
        ///     the search tree. If you wish to record predecessors, do so at this
        ///     event point.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> TreeEdge;

        /// <summary>
        ///     Invoked on a vertex after all of its out edges have been added to
        ///     the search tree and all of the adjacent vertices have been
        ///     discovered (but before their out-edges have been examined).
        /// </summary>
        public event VertexAction<TVertex> FinishVertex;

        #endregion

        #region IVertexTimeStamperAlgorithm<TVertex> Members

        /// <summary>
        ///     Invoked when a vertex is encountered for the first time.
        /// </summary>
        public event VertexAction<TVertex> DiscoverVertex;

        #endregion

        #region Members

        /// <summary>
        ///     Raises the <see cref="BackEdge" /> event.
        /// </summary>
        /// <param name="e">edge that raised the event</param>
        private void OnBackEdge(TEdge e)
        {
            BackEdge?.Invoke(e);
        }

        /// <summary>
        ///     Raises the <see cref="DiscoverVertex" /> event.
        /// </summary>
        /// <param name="v">vertex that raised the event</param>
        private void OnDiscoverVertex(TVertex v)
        {
            DiscoverVertex?.Invoke(v);
        }

        /// <summary>
        ///     Raises the <see cref="ExamineEdge" /> event.
        /// </summary>
        /// <param name="e">edge that raised the event</param>
        private void OnExamineEdge(TEdge e)
        {
            ExamineEdge?.Invoke(e);
        }

        /// <summary>
        ///     Raises the <see cref="FinishVertex" /> event.
        /// </summary>
        /// <param name="v">vertex that raised the event</param>
        private void OnFinishVertex(TVertex v)
        {
            FinishVertex?.Invoke(v);
        }

        /// <summary>
        ///     Raises the <see cref="ForwardOrCrossEdge" /> event.
        /// </summary>
        /// <param name="e">edge that raised the event</param>
        private void OnForwardOrCrossEdge(TEdge e)
        {
            ForwardOrCrossEdge?.Invoke(e);
        }

        /// <summary>
        ///     Raises the <see cref="StartVertex" /> event.
        /// </summary>
        /// <param name="v">vertex that raised the event</param>
        private void OnStartVertex(TVertex v)
        {
            StartVertex?.Invoke(v);
        }

        /// <summary>
        ///     Raises the <see cref="TreeEdge" /> event.
        /// </summary>
        /// <param name="e">edge that raised the event</param>
        private void OnTreeEdge(TEdge e)
        {
            TreeEdge?.Invoke(e);
        }

        private void Visit(TVertex u, int depth)
        {
            if (depth > MaxDepth)
            {
                return;
            }

            VertexColors[u] = GraphColor.Gray;
            OnDiscoverVertex(u);

            var cancelManager = Services.CancelManager;
            foreach (var e in VisitedGraph.OutEdges(u))
            {
                if (cancelManager.IsCancelling) return;

                OnExamineEdge(e);
                var v = e.Target;

                if (!VertexColors.ContainsKey(v))
                {
                    OnTreeEdge(e);
                    Visit(v, depth + 1);
                }
                else
                {
                    var c = VertexColors[v];
                    if (c == GraphColor.Gray)
                    {
                        OnBackEdge(e);
                    }
                    else
                    {
                        OnForwardOrCrossEdge(e);
                    }
                }
            }

            VertexColors[u] = GraphColor.Black;
            OnFinishVertex(u);
        }

        #endregion
    }
}