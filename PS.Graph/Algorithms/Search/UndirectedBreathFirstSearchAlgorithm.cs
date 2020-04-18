using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using PS.Graph.Algorithms.Services;
using PS.Graph.Collections;

namespace PS.Graph.Algorithms.Search
{
    /// <summary>
    ///     A breath first search algorithm for undirected graphs
    /// </summary>
    /// <reference-ref
    ///     idref="gross98graphtheory"
    ///     chapter="4.2" />
    [Serializable]
    public sealed class UndirectedBreadthFirstSearchAlgorithm<TVertex, TEdge> : RootedAlgorithmBase<TVertex, IUndirectedGraph<TVertex, TEdge>>,
                                                                                IUndirectedVertexPredecessorRecorderAlgorithm<TVertex, TEdge>,
                                                                                IDistanceRecorderAlgorithm<TVertex>,
                                                                                IVertexColorizerAlgorithm<TVertex>
        where TEdge : IEdge<TVertex>
    {
        private IQueue<TVertex> _vertexQueue;

        #region Constructors

        public UndirectedBreadthFirstSearchAlgorithm(IUndirectedGraph<TVertex, TEdge> g)
            : this(g, new Collections.Queue<TVertex>(), new Dictionary<TVertex, GraphColor>())
        {
        }

        public UndirectedBreadthFirstSearchAlgorithm(
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            IQueue<TVertex> vertexQueue,
            IDictionary<TVertex, GraphColor> vertexColors
        )
            : this(null, visitedGraph, vertexQueue, vertexColors)
        {
        }

        public UndirectedBreadthFirstSearchAlgorithm(
            IAlgorithmComponent host,
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            IQueue<TVertex> vertexQueue,
            IDictionary<TVertex, GraphColor> vertexColors
        )
            : base(host, visitedGraph)
        {
            Contract.Requires(vertexQueue != null);
            Contract.Requires(vertexColors != null);

            VertexColors = vertexColors;
            _vertexQueue = vertexQueue;
        }

        #endregion

        #region Properties

        public IDictionary<TVertex, GraphColor> VertexColors { get; }

        #endregion

        #region Events

        public event UndirectedEdgeAction<TVertex, TEdge> BlackTarget;

        public event EdgeAction<TVertex, TEdge> ExamineEdge;

        public event VertexAction<TVertex> ExamineVertex;

        public event UndirectedEdgeAction<TVertex, TEdge> GrayTarget;

        public event UndirectedEdgeAction<TVertex, TEdge> NonTreeEdge;

        #endregion

        #region Override members

        protected override void Initialize()
        {
            base.Initialize();

            // initialize vertex u
            var cancelManager = Services.CancelManager;
            if (cancelManager.IsCancelling)
            {
                return;
            }

            foreach (var v in VisitedGraph.Vertices)
            {
                VertexColors[v] = GraphColor.White;
                OnInitializeVertex(v);
            }
        }

        protected override void InternalCompute()
        {
            if (!TryGetRootVertex(out var rootVertex))
            {
                throw new InvalidOperationException("missing root vertex");
            }

            EnqueueRoot(rootVertex);
            FlushVisitQueue();
        }

        #endregion

        #region IDistanceRecorderAlgorithm<TVertex,TEdge> Members

        public event VertexAction<TVertex> InitializeVertex;

        public event VertexAction<TVertex> DiscoverVertex;

        #endregion

        #region IUndirectedVertexPredecessorRecorderAlgorithm<TVertex,TEdge> Members

        public event VertexAction<TVertex> StartVertex;

        public event UndirectedEdgeAction<TVertex, TEdge> TreeEdge;

        public event VertexAction<TVertex> FinishVertex;

        #endregion

        #region IVertexColorizerAlgorithm<TVertex,TEdge> Members

        public GraphColor GetVertexColor(TVertex vertex)
        {
            return VertexColors[vertex];
        }

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

            while (_vertexQueue.Count > 0)
            {
                if (cancelManager.IsCancelling) return;

                var u = _vertexQueue.Dequeue();

                OnExamineVertex(u);
                foreach (var e in VisitedGraph.AdjacentEdges(u))
                {
                    var reversed = e.Target.Equals(u);
                    var v = reversed ? e.Source : e.Target;
                    OnExamineEdge(e);

                    var vColor = VertexColors[v];
                    if (vColor == GraphColor.White)
                    {
                        OnTreeEdge(e, reversed);
                        VertexColors[v] = GraphColor.Gray;
                        OnDiscoverVertex(v);
                        _vertexQueue.Enqueue(v);
                    }
                    else
                    {
                        OnNonTreeEdge(e, reversed);
                        if (vColor == GraphColor.Gray)
                        {
                            OnGrayTarget(e, reversed);
                        }
                        else
                        {
                            OnBlackTarget(e, reversed);
                        }
                    }
                }

                VertexColors[u] = GraphColor.Black;
                OnFinishVertex(u);
            }
        }

        private void OnBlackTarget(TEdge e, bool reversed)
        {
            var eh = BlackTarget;
            eh?.Invoke(this, new UndirectedEdgeEventArgs<TVertex, TEdge>(e, reversed));
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

        private void OnGrayTarget(TEdge e, bool reversed)
        {
            var eh = GrayTarget;
            eh?.Invoke(this, new UndirectedEdgeEventArgs<TVertex, TEdge>(e, reversed));
        }

        private void OnInitializeVertex(TVertex v)
        {
            var eh = InitializeVertex;
            eh?.Invoke(v);
        }

        private void OnNonTreeEdge(TEdge e, bool reversed)
        {
            var eh = NonTreeEdge;
            eh?.Invoke(this, new UndirectedEdgeEventArgs<TVertex, TEdge>(e, reversed));
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

        #endregion
    }
}