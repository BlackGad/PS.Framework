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
    public sealed class DepthFirstSearchAlgorithm<TVertex, TEdge> : RootedAlgorithmBase<TVertex, IVertexListGraph<TVertex, TEdge>>,
                                                                    IDistanceRecorderAlgorithm<TVertex>,
                                                                    IVertexColorizerAlgorithm<TVertex>,
                                                                    IVertexPredecessorRecorderAlgorithm<TVertex, TEdge>,
                                                                    IVertexTimeStamperAlgorithm<TVertex>
        where TEdge : IEdge<TVertex>
    {
        private int _maxDepth = int.MaxValue;

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the algorithm.
        /// </summary>
        /// <param name="visitedGraph">visited graph</param>
        public DepthFirstSearchAlgorithm(IVertexListGraph<TVertex, TEdge> visitedGraph)
            : this(visitedGraph, new Dictionary<TVertex, GraphColor>())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the algorithm.
        /// </summary>
        /// <param name="visitedGraph">visited graph</param>
        /// <param name="colors">vertex color map</param>
        public DepthFirstSearchAlgorithm(
            IVertexListGraph<TVertex, TEdge> visitedGraph,
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
        public DepthFirstSearchAlgorithm(
            IAlgorithmComponent host,
            IVertexListGraph<TVertex, TEdge> visitedGraph
        )
            : this(host, visitedGraph, new Dictionary<TVertex, GraphColor>(), e => e)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the algorithm.
        /// </summary>
        /// <param name="host">algorithm host</param>
        /// <param name="visitedGraph">visited graph</param>
        /// <param name="colors">vertex color map</param>
        public DepthFirstSearchAlgorithm(
            IAlgorithmComponent host,
            IVertexListGraph<TVertex, TEdge> visitedGraph,
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
        /// <param name="outEdgeEnumerator">
        ///     Delegate that takes the enumeration of out-edges and reorders
        ///     them. All vertices passed to the method should be enumerated once and only once.
        ///     May be null.
        /// </param>
        public DepthFirstSearchAlgorithm(
            IAlgorithmComponent host,
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TVertex, GraphColor> colors,
            Func<IEnumerable<TEdge>, IEnumerable<TEdge>> outEdgeEnumerator
        )
            : base(host, visitedGraph)
        {
            VertexColors = colors;
            OutEdgeEnumerator = outEdgeEnumerator;
        }

        #endregion

        #region Properties

        public int MaxDepth
        {
            get { return _maxDepth; }
            set { _maxDepth = value; }
        }

        public Func<IEnumerable<TEdge>, IEnumerable<TEdge>> OutEdgeEnumerator { get; }

        public IDictionary<TVertex, GraphColor> VertexColors { get; }

        #endregion

        #region Events

        public event EdgeAction<TVertex, TEdge> BackEdge;

        public event EdgeAction<TVertex, TEdge> ExamineEdge;

        public event EdgeAction<TVertex, TEdge> ForwardOrCrossEdge;

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

        #region IVertexColorizerAlgorithm<TVertex> Members

        public GraphColor GetVertexColor(TVertex vertex)
        {
            return VertexColors[vertex];
        }

        #endregion

        #region IVertexPredecessorRecorderAlgorithm<TVertex,TEdge> Members

        public event VertexAction<TVertex> StartVertex;

        public event EdgeAction<TVertex, TEdge> TreeEdge;

        public event VertexAction<TVertex> FinishVertex;

        #endregion

        #region Members

        public void Visit(TVertex root)
        {
            var todo = new Stack<SearchFrame>();
            var oee = OutEdgeEnumerator;
            VertexColors[root] = GraphColor.Gray;
            OnDiscoverVertex(root);

            var cancelManager = Services.CancelManager;
            var enumerable = oee(VisitedGraph.OutEdges(root));
            todo.Push(new SearchFrame(root, enumerable.GetEnumerator(), 0));
            while (todo.Count > 0)
            {
                if (cancelManager.IsCancelling) return;

                var frame = todo.Pop();
                var u = frame.Vertex;
                var depth = frame.Depth;
                var edges = frame.Edges;

                if (depth > MaxDepth)
                {
                    edges?.Dispose();

                    VertexColors[u] = GraphColor.Black;
                    OnFinishVertex(u);
                    continue;
                }

                while (edges.MoveNext())
                {
                    var e = edges.Current;
                    if (cancelManager.IsCancelling) return;

                    OnExamineEdge(e);
                    // ReSharper disable once PossibleNullReferenceException
                    var v = e.Target;
                    var c = VertexColors[v];
                    switch (c)
                    {
                        case GraphColor.White:
                            OnTreeEdge(e);
                            todo.Push(new SearchFrame(u, edges, depth));
                            u = v;
                            edges = oee(VisitedGraph.OutEdges(u)).GetEnumerator();
                            depth++;
                            VertexColors[u] = GraphColor.Gray;
                            OnDiscoverVertex(u);
                            break;
                        case GraphColor.Gray:
                            OnBackEdge(e);
                            break;
                        case GraphColor.Black:
                            OnForwardOrCrossEdge(e);
                            break;
                    }
                }

                edges.Dispose();

                VertexColors[u] = GraphColor.Black;
                OnFinishVertex(u);
            }
        }

        private void OnBackEdge(TEdge e)
        {
            var eh = BackEdge;
            eh?.Invoke(e);
        }

        private void OnDiscoverVertex(TVertex v)
        {
            var eh = DiscoverVertex;
            eh?.Invoke(v);
        }

        private void OnExamineEdge(TEdge e)
        {
            var eh = ExamineEdge;
            eh?.Invoke(e);
        }

        private void OnFinishVertex(TVertex v)
        {
            var eh = FinishVertex;
            eh?.Invoke(v);
        }

        private void OnForwardOrCrossEdge(TEdge e)
        {
            var eh = ForwardOrCrossEdge;
            eh?.Invoke(e);
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

        private void OnTreeEdge(TEdge e)
        {
            var eh = TreeEdge;
            eh?.Invoke(e);
        }

        #endregion

        #region Nested type: SearchFrame

        private struct SearchFrame
        {
            public readonly int Depth;
            public readonly IEnumerator<TEdge> Edges;
            public readonly TVertex Vertex;

            #region Constructors

            public SearchFrame(TVertex vertex, IEnumerator<TEdge> edges, int depth)
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