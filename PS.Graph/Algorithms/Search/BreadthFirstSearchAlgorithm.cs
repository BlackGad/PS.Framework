using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using PS.Graph.Algorithms.Services;
using PS.Graph.Collections;

namespace PS.Graph.Algorithms.Search
{
    /// <summary>
    ///     A breath first search algorithm for directed graphs
    /// </summary>
    /// <reference-ref
    ///     idref="gross98graphtheory"
    ///     chapter="4.2" />
    public sealed class BreadthFirstSearchAlgorithm<TVertex, TEdge> : RootedAlgorithmBase<TVertex, IVertexListGraph<TVertex, TEdge>>,
                                                                      IVertexPredecessorRecorderAlgorithm<TVertex, TEdge>,
                                                                      IDistanceRecorderAlgorithm<TVertex>,
                                                                      IVertexColorizerAlgorithm<TVertex>
        where TEdge : IEdge<TVertex>
    {
        private readonly IQueue<TVertex> _vertexQueue;

        #region Constructors

        public BreadthFirstSearchAlgorithm(IVertexListGraph<TVertex, TEdge> g)
            : this(g, new Collections.Queue<TVertex>(), new Dictionary<TVertex, GraphColor>())
        {
        }

        public BreadthFirstSearchAlgorithm(
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            IQueue<TVertex> vertexQueue,
            IDictionary<TVertex, GraphColor> vertexColors
        )
            : this(null, visitedGraph, vertexQueue, vertexColors)
        {
        }

        public BreadthFirstSearchAlgorithm(
            IAlgorithmComponent host,
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            IQueue<TVertex> vertexQueue,
            IDictionary<TVertex, GraphColor> vertexColors
        )
            : this(host, visitedGraph, vertexQueue, vertexColors, e => e)
        {
        }

        public BreadthFirstSearchAlgorithm(
            IAlgorithmComponent host,
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            IQueue<TVertex> vertexQueue,
            IDictionary<TVertex, GraphColor> vertexColors,
            Func<IEnumerable<TEdge>, IEnumerable<TEdge>> outEdgeEnumerator
        )
            : base(host, visitedGraph)
        {
            Contract.Requires(vertexQueue != null);
            Contract.Requires(vertexColors != null);
            Contract.Requires(outEdgeEnumerator != null);

            VertexColors = vertexColors;
            _vertexQueue = vertexQueue;
            OutEdgeEnumerator = outEdgeEnumerator;
        }

        #endregion

        #region Properties

        public Func<IEnumerable<TEdge>, IEnumerable<TEdge>> OutEdgeEnumerator { get; }

        public IDictionary<TVertex, GraphColor> VertexColors { get; }

        #endregion

        #region Events

        public event EdgeAction<TVertex, TEdge> BlackTarget;

        public event EdgeAction<TVertex, TEdge> ExamineEdge;

        public event VertexAction<TVertex> ExamineVertex;

        public event EdgeAction<TVertex, TEdge> GrayTarget;

        public event EdgeAction<TVertex, TEdge> NonTreeEdge;

        #endregion

        #region Override members

        protected override void Initialize()
        {
            base.Initialize();

            var cancelManager = Services.CancelManager;
            if (cancelManager.IsCancelling)
            {
                return;
            }

            // initialize vertex u
            foreach (var v in VisitedGraph.Vertices)
            {
                VertexColors[v] = GraphColor.White;
                OnInitializeVertex(v);
            }
        }

        protected override void InternalCompute()
        {
            if (VisitedGraph.VertexCount == 0)
            {
                return;
            }

            if (!TryGetRootVertex(out var rootVertex))
            {
                // enqueue roots
                foreach (var root in VisitedGraph.Roots())
                {
                    EnqueueRoot(root);
                }
            }
            else // enqueue select root only
            {
                EnqueueRoot(rootVertex);
            }

            FlushVisitQueue();
        }

        #endregion

        #region IDistanceRecorderAlgorithm<TVertex,TEdge> Members

        public event VertexAction<TVertex> InitializeVertex;

        public event VertexAction<TVertex> DiscoverVertex;

        #endregion

        #region IVertexColorizerAlgorithm<TVertex,TEdge> Members

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

        public void Visit(TVertex s)
        {
            EnqueueRoot(s);
            FlushVisitQueue();
        }

        private void EnqueueRoot(TVertex s)
        {
            OnStartVertex(s);

            VertexColors[s] = GraphColor.Gray;

            OnDiscoverVertex(s);
            _vertexQueue.Enqueue(s);
        }

        private void FlushVisitQueue()
        {
            var cancelManager = Services.CancelManager;
            var oee = OutEdgeEnumerator;

            while (_vertexQueue.Count > 0)
            {
                if (cancelManager.IsCancelling) return;

                var u = _vertexQueue.Dequeue();
                OnExamineVertex(u);
                foreach (var e in oee(VisitedGraph.OutEdges(u)))
                {
                    var v = e.Target;
                    OnExamineEdge(e);

                    var vColor = VertexColors[v];
                    if (vColor == GraphColor.White)
                    {
                        OnTreeEdge(e);
                        VertexColors[v] = GraphColor.Gray;
                        OnDiscoverVertex(v);
                        _vertexQueue.Enqueue(v);
                    }
                    else
                    {
                        OnNonTreeEdge(e);
                        if (vColor == GraphColor.Gray)
                        {
                            OnGrayTarget(e);
                        }
                        else
                        {
                            OnBlackTarget(e);
                        }
                    }
                }

                VertexColors[u] = GraphColor.Black;
                OnFinishVertex(u);
            }
        }

        private void OnBlackTarget(TEdge e)
        {
            var eh = BlackTarget;
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

        private void OnExamineVertex(TVertex v)
        {
            var eh = ExamineVertex;
            eh?.Invoke(v);
        }

        private void OnFinishVertex(TVertex v)
        {
            var eh = FinishVertex;
            eh?.Invoke(v);
        }

        private void OnGrayTarget(TEdge e)
        {
            var eh = GrayTarget;
            eh?.Invoke(e);
        }

        private void OnInitializeVertex(TVertex v)
        {
            var eh = InitializeVertex;
            eh?.Invoke(v);
        }

        private void OnNonTreeEdge(TEdge e)
        {
            var eh = NonTreeEdge;
            eh?.Invoke(e);
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