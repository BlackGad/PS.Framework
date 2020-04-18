using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using PS.Graph.Algorithms.Services;

namespace PS.Graph.Algorithms.Search
{
    /// <summary>
    ///     A edge depth first search algorithm for implicit directed graphs
    /// </summary>
    /// <remarks>
    ///     This is a variant of the classic DFS where the edges are color
    ///     marked.
    /// </remarks>
    /// <reference-ref
    ///     idref="gross98graphtheory"
    ///     chapter="4.2" />
    [Serializable]
    public sealed class ImplicitEdgeDepthFirstSearchAlgorithm<TVertex, TEdge> : RootedAlgorithmBase<TVertex, IIncidenceGraph<TVertex, TEdge>>,
                                                                                ITreeBuilderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private int _maxDepth = int.MaxValue;

        #region Constructors

        public ImplicitEdgeDepthFirstSearchAlgorithm(IIncidenceGraph<TVertex, TEdge> visitedGraph)
            : this(null, visitedGraph)
        {
        }

        public ImplicitEdgeDepthFirstSearchAlgorithm(
            IAlgorithmComponent host,
            IIncidenceGraph<TVertex, TEdge> visitedGraph
        )
            : base(host, visitedGraph)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the vertex color map
        /// </summary>
        /// <value>
        ///     Vertex color (<see cref="GraphColor" />) dictionary
        /// </value>
        public IDictionary<TEdge, GraphColor> EdgeColors { get; } = new Dictionary<TEdge, GraphColor>();

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

        #endregion

        #region Events

        /// <summary>
        ///     Invoked on the back edges in the graph.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> BackEdge;

        /// <summary>
        /// </summary>
        public event EdgeEdgeAction<TVertex, TEdge> DiscoverTreeEdge;

        /// <summary>
        ///     Invoked on a edge after all of its out edges have been added to
        ///     the search tree and all of the adjacent vertices have been
        ///     discovered (but before their out-edges have been examined).
        /// </summary>
        public event EdgeAction<TVertex, TEdge> FinishEdge;

        /// <summary>
        ///     Invoked on forward or cross edges in the graph.
        ///     (In an undirected graph this method is never called.)
        /// </summary>
        public event EdgeAction<TVertex, TEdge> ForwardOrCrossEdge;

        /// <summary>
        ///     Invoked on the first edge of a test case
        /// </summary>
        public event EdgeAction<TVertex, TEdge> StartEdge;

        /// <summary>
        ///     Invoked on the source vertex once before the start of the search.
        /// </summary>
        public event VertexAction<TVertex> StartVertex;

        #endregion

        #region Override members

        protected override void InternalCompute()
        {
            if (!TryGetRootVertex(out var rootVertex))
            {
                throw new InvalidOperationException("root vertex not set");
            }

            // initialize algorithm
            Initialize();

            // start with him:
            OnStartVertex(rootVertex);

            var cancelManager = Services.CancelManager;
            // process each out edge of v
            foreach (var e in VisitedGraph.OutEdges(rootVertex))
            {
                if (cancelManager.IsCancelling) return;

                if (!EdgeColors.ContainsKey(e))
                {
                    OnStartEdge(e);
                    Visit(e, 0);
                }
            }
        }

        /// <summary>
        ///     Initializes the algorithm before computation.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            EdgeColors.Clear();
        }

        #endregion

        #region ITreeBuilderAlgorithm<TVertex,TEdge> Members

        /// <summary>
        ///     Invoked on each edge as it becomes a member of the edges that form
        ///     the search tree. If you wish to record predecessors, do so at this
        ///     event point.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> TreeEdge;

        #endregion

        #region Members

        /// <summary>
        ///     Triggers the BackEdge event.
        /// </summary>
        /// <param name="e"></param>
        private void OnBackEdge(TEdge e)
        {
            BackEdge?.Invoke(e);
        }

        /// <summary>
        ///     Triggers DiscoverEdge event
        /// </summary>
        /// <param name="se"></param>
        /// <param name="e"></param>
        private void OnDiscoverTreeEdge(TEdge se, TEdge e)
        {
            var eh = DiscoverTreeEdge;
            eh?.Invoke(se, e);
        }

        /// <summary>
        ///     Triggers the ForwardOrCrossEdge event.
        /// </summary>
        /// <param name="e"></param>
        private void OnFinishEdge(TEdge e)
        {
            FinishEdge?.Invoke(e);
        }

        /// <summary>
        ///     Triggers the ForwardOrCrossEdge event.
        /// </summary>
        /// <param name="e"></param>
        private void OnForwardOrCrossEdge(TEdge e)
        {
            ForwardOrCrossEdge?.Invoke(e);
        }

        /// <summary>
        ///     Triggers the StartEdge event.
        /// </summary>
        /// <param name="e"></param>
        private void OnStartEdge(TEdge e)
        {
            StartEdge?.Invoke(e);
        }

        /// <summary>
        ///     Triggers the StartVertex event.
        /// </summary>
        /// <param name="v"></param>
        private void OnStartVertex(TVertex v)
        {
            StartVertex?.Invoke(v);
        }

        /// <summary>
        ///     Triggers the TreeEdge event.
        /// </summary>
        /// <param name="e"></param>
        private void OnTreeEdge(TEdge e)
        {
            TreeEdge?.Invoke(e);
        }

        /// <summary>
        ///     Does a depth first search on the vertex u
        /// </summary>
        /// <param name="se">edge to explore</param>
        /// <param name="depth">current exploration depth</param>
        /// <exception cref="ArgumentNullException">se cannot be null</exception>
        private void Visit(TEdge se, int depth)
        {
            Contract.Requires(se != null);
            Contract.Requires(depth >= 0);

            if (depth > _maxDepth)
            {
                return;
            }

            // mark edge as gray
            EdgeColors[se] = GraphColor.Gray;
            // add edge to the search tree
            OnTreeEdge(se);

            var cancelManager = Services.CancelManager;
            // iterate over out-edges
            foreach (var e in VisitedGraph.OutEdges(se.Target))
            {
                if (cancelManager.IsCancelling) return;

                // check edge is not explored yet,
                // if not, explore it.
                if (!EdgeColors.ContainsKey(e))
                {
                    OnDiscoverTreeEdge(se, e);
                    Visit(e, depth + 1);
                }
                else
                {
                    if (EdgeColors[e] == GraphColor.Gray)
                    {
                        OnBackEdge(e);
                    }
                    else
                    {
                        OnForwardOrCrossEdge(e);
                    }
                }
            }

            // all out-edges have been explored
            EdgeColors[se] = GraphColor.Black;
            OnFinishEdge(se);
        }

        #endregion
    }
}