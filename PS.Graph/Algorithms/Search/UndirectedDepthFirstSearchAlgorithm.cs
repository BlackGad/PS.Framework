using System;
using System.Collections.Generic;
using PS.Graph.Algorithms.Services;

namespace PS.Graph.Algorithms.Search
{
    /// <summary>
    ///     A depth first search algorithm for directed graph
    /// </summary>
    /// <typeparam name="TVertex">type of a vertex</typeparam>
    /// <typeparam name="TEdge">type of an edge</typeparam>
    /// <reference-ref
    ///     idref="gross98graphtheory"
    ///     chapter="4.2" />
    [Serializable]
    public sealed class UndirectedDepthFirstSearchAlgorithm<TVertex, TEdge> : RootedAlgorithmBase<TVertex, IUndirectedGraph<TVertex, TEdge>>,
                                                                              IDistanceRecorderAlgorithm<TVertex>,
                                                                              IVertexColorizerAlgorithm<TVertex>,
                                                                              IUndirectedVertexPredecessorRecorderAlgorithm<TVertex, TEdge>,
                                                                              IVertexTimeStamperAlgorithm<TVertex>
        where TEdge : IEdge<TVertex>
    {
        private int _maxDepth = int.MaxValue;

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the algorithm.
        /// </summary>
        /// <param name="visitedGraph">visited graph</param>
        public UndirectedDepthFirstSearchAlgorithm(IUndirectedGraph<TVertex, TEdge> visitedGraph)
            : this(visitedGraph, new Dictionary<TVertex, GraphColor>())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the algorithm.
        /// </summary>
        /// <param name="visitedGraph">visited graph</param>
        /// <param name="colors">vertex color map</param>
        public UndirectedDepthFirstSearchAlgorithm(
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TVertex, GraphColor> colors
        )
            : this(null, visitedGraph, colors)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the algorithm.
        /// </summary>
        /// <param name="host">algorithm host</param>
        /// <param name="visitedGraph">visited graph</param>
        /// <param name="colors">vertex color map</param>
        public UndirectedDepthFirstSearchAlgorithm(
            IAlgorithmComponent host,
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TVertex, GraphColor> colors
        )
            : this(host, visitedGraph, colors, e => e)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the algorithm.
        /// </summary>
        /// <param name="host">algorithm host</param>
        /// <param name="visitedGraph">visited graph</param>
        /// <param name="colors">vertex color map</param>
        /// <param name="adjacentEdgeEnumerator">
        ///     Delegate that takes the enumeration of out-edges and reorders
        ///     them. All vertices passed to the method should be enumerated once and only once.
        ///     May be null.
        /// </param>
        public UndirectedDepthFirstSearchAlgorithm(
            IAlgorithmComponent host,
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TVertex, GraphColor> colors,
            Func<IEnumerable<TEdge>, IEnumerable<TEdge>> adjacentEdgeEnumerator
        )
            : base(host, visitedGraph)
        {
            VertexColors = colors;
            AdjacentEdgeEnumerator = adjacentEdgeEnumerator;
        }

        #endregion

        #region Properties

        public Func<IEnumerable<TEdge>, IEnumerable<TEdge>> AdjacentEdgeEnumerator { get; }

        public int MaxDepth
        {
            get { return _maxDepth; }
            set { _maxDepth = value; }
        }

        public IDictionary<TVertex, GraphColor> VertexColors { get; }

        #endregion

        #region Events

        public event UndirectedEdgeAction<TVertex, TEdge> BackEdge;

        public event UndirectedEdgeAction<TVertex, TEdge> ExamineEdge;

        public event UndirectedEdgeAction<TVertex, TEdge> ForwardOrCrossEdge;

        public event VertexAction<TVertex> VertexMaxDepthReached;

        #endregion

        #region Override members

        protected override void InternalCompute()
        {
            // if there is a starting vertex, start with him:
            if (TryGetRootVertex(out var rootVertex))
            {
                OnStartVertex(rootVertex);
                Visit(rootVertex);
            }
            else
            {
                var cancelManager = Services.CancelManager;
                // process each vertex 
                foreach (var u in VisitedGraph.Vertices)
                {
                    if (cancelManager.IsCancelling)
                    {
                        return;
                    }

                    if (VertexColors[u] == GraphColor.White)
                    {
                        OnStartVertex(u);
                        Visit(u);
                    }
                }
            }
        }

        protected override void Initialize()
        {
            base.Initialize();

            VertexColors.Clear();
            foreach (var u in VisitedGraph.Vertices)
            {
                VertexColors[u] = GraphColor.White;
                OnInitializeVertex(u);
            }
        }

        #endregion

        #region IDistanceRecorderAlgorithm<TVertex> Members

        public event VertexAction<TVertex> InitializeVertex;

        public event VertexAction<TVertex> DiscoverVertex;

        #endregion

        #region IUndirectedVertexPredecessorRecorderAlgorithm<TVertex,TEdge> Members

        public event VertexAction<TVertex> StartVertex;

        public event UndirectedEdgeAction<TVertex, TEdge> TreeEdge;

        public event VertexAction<TVertex> FinishVertex;

        #endregion

        #region IVertexColorizerAlgorithm<TVertex> Members

        public GraphColor GetVertexColor(TVertex vertex)
        {
            return VertexColors[vertex];
        }

        #endregion

        #region Members

        public void Visit(TVertex root)
        {
            var todo = new Stack<SearchFrame>();
            var oee = AdjacentEdgeEnumerator;
            var visitedEdges = new Dictionary<TEdge, int>(VisitedGraph.EdgeCount);

            VertexColors[root] = GraphColor.Gray;
            OnDiscoverVertex(root);

            var cancelManager = Services.CancelManager;
            var enumerable = oee(VisitedGraph.AdjacentEdges(root));
            todo.Push(new SearchFrame(root, enumerable.GetEnumerator(), 0));
            while (todo.Count > 0)
            {
                if (cancelManager.IsCancelling) return;

                var frame = todo.Pop();
                var u = frame.Vertex;
                var depth = frame.Depth;

                if (depth > MaxDepth)
                {
                    OnVertexMaxDepthReached(u);
                    VertexColors[u] = GraphColor.Black;
                    OnFinishVertex(u);
                    continue;
                }

                var edges = frame.Edges;
                while (edges.MoveNext())
                {
                    var e = edges.Current;
                    if (cancelManager.IsCancelling) return;
                    // ReSharper disable once AssignNullToNotNullAttribute
                    if (visitedEdges.ContainsKey(e)) continue; // edge already visited

                    visitedEdges.Add(e, 0);
                    var reversed = e.Target.Equals(u);
                    OnExamineEdge(e, reversed);
                    var v = reversed ? e.Source : e.Target;
                    var c = VertexColors[v];
                    switch (c)
                    {
                        case GraphColor.White:
                            OnTreeEdge(e, reversed);
                            todo.Push(new SearchFrame(u, edges, frame.Depth + 1));
                            u = v;
                            edges = oee(VisitedGraph.AdjacentEdges(u)).GetEnumerator();
                            VertexColors[u] = GraphColor.Gray;
                            OnDiscoverVertex(u);
                            break;
                        case GraphColor.Gray:
                            OnBackEdge(e, reversed);
                            break;
                        case GraphColor.Black:
                            OnForwardOrCrossEdge(e, reversed);
                            break;
                    }
                }

                VertexColors[u] = GraphColor.Black;
                OnFinishVertex(u);
            }
        }

        private void OnBackEdge(TEdge e, bool reversed)
        {
            var eh = BackEdge;
            eh?.Invoke(this, new UndirectedEdgeEventArgs<TVertex, TEdge>(e, reversed));
        }

        private void OnDiscoverVertex(TVertex v)
        {
            var eh = DiscoverVertex;
            eh?.Invoke(v);
        }

        private void OnExamineEdge(TEdge e, bool reversed)
        {
            var eh = ExamineEdge;
            eh?.Invoke(this, new UndirectedEdgeEventArgs<TVertex, TEdge>(e, reversed));
        }

        private void OnFinishVertex(TVertex v)
        {
            var eh = FinishVertex;
            eh?.Invoke(v);
        }

        private void OnForwardOrCrossEdge(TEdge e, bool reversed)
        {
            var eh = ForwardOrCrossEdge;
            eh?.Invoke(this, new UndirectedEdgeEventArgs<TVertex, TEdge>(e, reversed));
        }

        private void OnInitializeVertex(TVertex v)
        {
            var eh = InitializeVertex;
            eh?.Invoke(v);
        }

        private void OnStartVertex(TVertex v)
        {
            var eh = StartVertex;
            eh?.Invoke(v);
        }

        private void OnTreeEdge(TEdge e, bool reversed)
        {
            var eh = TreeEdge;
            eh?.Invoke(this, new UndirectedEdgeEventArgs<TVertex, TEdge>(e, reversed));
        }

        private void OnVertexMaxDepthReached(TVertex v)
        {
            var eh = VertexMaxDepthReached;
            eh?.Invoke(v);
        }

        #endregion

        #region Nested type: SearchFrame

        private struct SearchFrame
        {
            public readonly int Depth;
            public readonly IEnumerator<TEdge> Edges;
            public readonly TVertex Vertex;

            #region Constructors

            public SearchFrame(
                TVertex vertex,
                IEnumerator<TEdge> edges,
                int depth)
            {
                Vertex = vertex;
                Edges = edges;
                Depth = depth;
            }

            #endregion
        }

        #endregion
    }
}