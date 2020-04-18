using System;
using System.Collections.Generic;
using PS.Graph.Algorithms.Services;

namespace PS.Graph.Algorithms.Search
{
    /// <summary>
    ///     A edge depth first search algorithm for directed graphs
    /// </summary>
    /// <remarks>
    ///     This is a variant of the classic DFS algorithm where the
    ///     edges are color marked instead of the vertices.
    /// </remarks>
    /// <reference-ref
    ///     idref="gross98graphtheory"
    ///     chapter="4.2" />
    [Serializable]
    public sealed class EdgeDepthFirstSearchAlgorithm<TVertex, TEdge> : RootedAlgorithmBase<TVertex, IEdgeListAndIncidenceGraph<TVertex, TEdge>>,
                                                                        IEdgeColorizerAlgorithm<TVertex, TEdge>,
                                                                        IEdgePredecessorRecorderAlgorithm<TVertex, TEdge>,
                                                                        ITreeBuilderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private int _maxDepth;

        #region Constructors

        public EdgeDepthFirstSearchAlgorithm(IEdgeListAndIncidenceGraph<TVertex, TEdge> g)
            : this(g, new Dictionary<TEdge, GraphColor>())
        {
        }

        public EdgeDepthFirstSearchAlgorithm(
            IEdgeListAndIncidenceGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TEdge, GraphColor> colors
        )
            : this(null, visitedGraph, colors)
        {
        }

        public EdgeDepthFirstSearchAlgorithm(
            IAlgorithmComponent host,
            IEdgeListAndIncidenceGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TEdge, GraphColor> colors
        )
            : base(host, visitedGraph)
        {
            EdgeColors = colors;
            _maxDepth = int.MaxValue;
        }

        #endregion

        #region Properties

        public int MaxDepth
        {
            get { return _maxDepth; }
            set { _maxDepth = value; }
        }

        #endregion

        #region Events

        public event EdgeAction<TVertex, TEdge> BackEdge;

        public event EdgeAction<TVertex, TEdge> ExamineEdge;

        public event EdgeAction<TVertex, TEdge> ForwardOrCrossEdge;

        public event EdgeAction<TVertex, TEdge> InitializeEdge;

        public event EdgeAction<TVertex, TEdge> StartEdge;

        public event VertexAction<TVertex> StartVertex;

        #endregion

        #region Override members

        protected override void InternalCompute()
        {
            Initialize();
            var cancelManager = Services.CancelManager;
            if (cancelManager.IsCancelling)
            {
                return;
            }

            // start with him:
            if (TryGetRootVertex(out var rootVertex))
            {
                OnStartVertex(rootVertex);

                // process each out edge of v
                foreach (var e in VisitedGraph.OutEdges(rootVertex))
                {
                    if (cancelManager.IsCancelling)
                    {
                        return;
                    }

                    if (EdgeColors[e] == GraphColor.White)
                    {
                        OnStartEdge(e);
                        Visit(e, 0);
                    }
                }
            }

            // process the rest of the graph edges
            foreach (var e in VisitedGraph.Edges)
            {
                if (cancelManager.IsCancelling)
                {
                    return;
                }

                if (EdgeColors[e] == GraphColor.White)
                {
                    OnStartEdge(e);
                    Visit(e, 0);
                }
            }
        }

        protected override void Initialize()
        {
            // put all vertex to white
            var cancelManager = Services.CancelManager;
            foreach (var e in VisitedGraph.Edges)
            {
                if (cancelManager.IsCancelling)
                {
                    return;
                }

                EdgeColors[e] = GraphColor.White;
                OnInitializeEdge(e);
            }
        }

        #endregion

        #region IEdgeColorizerAlgorithm<TVertex,TEdge> Members

        public IDictionary<TEdge, GraphColor> EdgeColors { get; }

        #endregion

        #region IEdgePredecessorRecorderAlgorithm<TVertex,TEdge> Members

        public event EdgeEdgeAction<TVertex, TEdge> DiscoverTreeEdge;

        public event EdgeAction<TVertex, TEdge> FinishEdge;

        #endregion

        #region ITreeBuilderAlgorithm<TVertex,TEdge> Members

        public event EdgeAction<TVertex, TEdge> TreeEdge;

        #endregion

        #region Members

        public void Visit(TEdge se, int depth)
        {
            if (depth > _maxDepth)
            {
                return;
            }

            var cancelManager = Services.CancelManager;

            // mark edge as gray
            EdgeColors[se] = GraphColor.Gray;
            // add edge to the search tree
            OnTreeEdge(se);

            // iterate over out-edges
            foreach (var e in VisitedGraph.OutEdges(se.Target))
            {
                if (cancelManager.IsCancelling) return;

                // check edge is not explored yet,
                // if not, explore it.
                if (EdgeColors[e] == GraphColor.White)
                {
                    OnDiscoverTreeEdge(se, e);
                    Visit(e, depth + 1);
                }
                else if (EdgeColors[e] == GraphColor.Gray)
                {
                    // edge is being explored
                    OnBackEdge(e);
                }
                else
                    // edge is black
                {
                    OnForwardOrCrossEdge(e);
                }
            }

            // all out-edges have been explored
            EdgeColors[se] = GraphColor.Black;
            OnFinishEdge(se);
        }

        private void OnBackEdge(TEdge e)
        {
            BackEdge?.Invoke(e);
        }

        private void OnDiscoverTreeEdge(TEdge e, TEdge targetEge)
        {
            var eh = DiscoverTreeEdge;
            eh?.Invoke(e, targetEge);
        }

        private void OnExamineEdge(TEdge e)
        {
            var eh = ExamineEdge;
            eh?.Invoke(e);
        }

        private void OnFinishEdge(TEdge e)
        {
            FinishEdge?.Invoke(e);
        }

        private void OnForwardOrCrossEdge(TEdge e)
        {
            ForwardOrCrossEdge?.Invoke(e);
        }

        private void OnInitializeEdge(TEdge e)
        {
            var eh = InitializeEdge;
            eh?.Invoke(e);
        }

        private void OnStartEdge(TEdge e)
        {
            StartEdge?.Invoke(e);
        }

        private void OnStartVertex(TVertex v)
        {
            var eh = StartVertex;
            eh?.Invoke(v);
        }

        private void OnTreeEdge(TEdge e)
        {
            var eh = TreeEdge;
            eh?.Invoke(e);
        }

        #endregion
    }
}