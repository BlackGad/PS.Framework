using System;
using System.Collections.Generic;
using PS.Graph.Algorithms.Search;
using PS.Graph.Algorithms.Services;

namespace PS.Graph.Algorithms.ConnectedComponents
{
    [Serializable]
    public sealed class StronglyConnectedComponentAlgorithm<TVertex, TEdge> : AlgorithmBase<IVertexListGraph<TVertex, TEdge>>,
                                                                              IConnectedComponentAlgorithm<TVertex, IVertexListGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private readonly Dictionary<TVertex, int> _discoverTimes;
        private readonly Dictionary<TVertex, TVertex> _roots;
        private int _dfsTime;
        private Stack<TVertex> _stack;

        #region Constructors

        public StronglyConnectedComponentAlgorithm(
            IVertexListGraph<TVertex, TEdge> g)
            : this(g, new Dictionary<TVertex, int>())
        {
        }

        public StronglyConnectedComponentAlgorithm(
            IVertexListGraph<TVertex, TEdge> g,
            IDictionary<TVertex, int> components)
            : this(null, g, components)
        {
        }

        public StronglyConnectedComponentAlgorithm(
            IAlgorithmComponent host,
            IVertexListGraph<TVertex, TEdge> g,
            IDictionary<TVertex, int> components)
            : base(host, g)
        {
            Components = components;
            _roots = new Dictionary<TVertex, TVertex>();
            _discoverTimes = new Dictionary<TVertex, int>();
            _stack = new Stack<TVertex>();
            ComponentCount = 0;
            _dfsTime = 0;
        }

        #endregion

        #region Properties

        public IDictionary<TVertex, int> DiscoverTimes
        {
            get { return _discoverTimes; }
        }

        public IDictionary<TVertex, TVertex> Roots
        {
            get { return _roots; }
        }

        #endregion

        #region Override members

        protected override void InternalCompute()
        {
            Components.Clear();
            Roots.Clear();
            DiscoverTimes.Clear();
            _stack.Clear();
            ComponentCount = 0;
            _dfsTime = 0;

            DepthFirstSearchAlgorithm<TVertex, TEdge> dfs = null;
            try
            {
                dfs = new DepthFirstSearchAlgorithm<TVertex, TEdge>(
                    this,
                    VisitedGraph,
                    new Dictionary<TVertex, GraphColor>(VisitedGraph.VertexCount)
                );
                dfs.DiscoverVertex += DiscoverVertex;
                dfs.FinishVertex += FinishVertex;

                dfs.Compute();
            }
            finally
            {
                if (dfs != null)
                {
                    dfs.DiscoverVertex -= DiscoverVertex;
                    dfs.FinishVertex -= FinishVertex;
                }
            }
        }

        #endregion

        #region IConnectedComponentAlgorithm<TVertex,IVertexListGraph<TVertex,TEdge>> Members

        public IDictionary<TVertex, int> Components { get; }

        public int ComponentCount { get; private set; }

        #endregion

        #region Event handlers

        private void DiscoverVertex(TVertex v)
        {
            Roots[v] = v;
            Components[v] = int.MaxValue;
            DiscoverTimes[v] = _dfsTime++;
            _stack.Push(v);
        }

        /// <summary>
        ///     Used internally
        /// </summary>
        private void FinishVertex(TVertex v)
        {
            var roots = Roots;

            foreach (var e in VisitedGraph.OutEdges(v))
            {
                var w = e.Target;
                if (Components[w] == int.MaxValue)
                {
                    roots[v] = MinDiscoverTime(roots[v], roots[w]);
                }
            }

            if (_roots[v].Equals(v))
            {
                TVertex w;
                do
                {
                    w = _stack.Pop();
                    Components[w] = ComponentCount;
                } while (!w.Equals(v));

                ++ComponentCount;
            }
        }

        #endregion

        #region Members

        private TVertex MinDiscoverTime(TVertex u, TVertex v)
        {
            if (_discoverTimes[u] < _discoverTimes[v])
            {
                return u;
            }

            return v;
        }

        #endregion
    }
}