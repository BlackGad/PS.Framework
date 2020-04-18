using System;
using System.Collections.Generic;
using PS.Graph.Algorithms.Search;
using PS.Graph.Algorithms.Services;

namespace PS.Graph.Algorithms.ConnectedComponents
{
    [Serializable]
    public sealed class WeaklyConnectedComponentAlgorithm<TVertex, TEdge> : AlgorithmBase<IVertexListGraph<TVertex, TEdge>>,
                                                                            IConnectedComponentAlgorithm<TVertex, IVertexListGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private readonly Dictionary<int, int> _componentEquivalences = new Dictionary<int, int>();
        private int _currentComponent;

        #region Constructors

        public WeaklyConnectedComponentAlgorithm(IVertexListGraph<TVertex, TEdge> visitedGraph)
            : this(visitedGraph, new Dictionary<TVertex, int>())
        {
        }

        public WeaklyConnectedComponentAlgorithm(
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TVertex, int> components)
            : this(null, visitedGraph, components)
        {
        }

        public WeaklyConnectedComponentAlgorithm(
            IAlgorithmComponent host,
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TVertex, int> components)
            : base(host, visitedGraph)
        {
            Components = components;
        }

        #endregion

        #region Override members

        protected override void Initialize()
        {
            ComponentCount = 0;
            _currentComponent = 0;
            _componentEquivalences.Clear();
            Components.Clear();
        }

        protected override void InternalCompute()
        {
            // shortcut for empty graph
            if (VisitedGraph.IsVerticesEmpty)
            {
                return;
            }

            var dfs = new DepthFirstSearchAlgorithm<TVertex, TEdge>(VisitedGraph);
            try
            {
                dfs.StartVertex += dfs_StartVertex;
                dfs.TreeEdge += dfs_TreeEdge;
                dfs.ForwardOrCrossEdge += dfs_ForwardOrCrossEdge;

                dfs.Compute();
            }
            finally
            {
                dfs.StartVertex -= dfs_StartVertex;
                dfs.TreeEdge -= dfs_TreeEdge;
                dfs.ForwardOrCrossEdge -= dfs_ForwardOrCrossEdge;
            }

            // updating component numbers
            foreach (var v in VisitedGraph.Vertices)
            {
                var component = Components[v];
                var equivalent = GetComponentEquivalence(component);
                if (component != equivalent)
                {
                    Components[v] = equivalent;
                }
            }

            _componentEquivalences.Clear();
        }

        #endregion

        #region IConnectedComponentAlgorithm<TVertex,IVertexListGraph<TVertex,TEdge>> Members

        public IDictionary<TVertex, int> Components { get; }

        public int ComponentCount { get; private set; }

        #endregion

        #region Event handlers

        private void dfs_ForwardOrCrossEdge(TEdge e)
        {
            // we have touched another tree, updating count and current component
            var otherComponent = GetComponentEquivalence(Components[e.Target]);
            if (otherComponent != _currentComponent)
            {
                ComponentCount--;

                if (_currentComponent > otherComponent)
                {
                    _componentEquivalences[_currentComponent] = otherComponent;
                    _currentComponent = otherComponent;
                }
                else
                {
                    _componentEquivalences[otherComponent] = _currentComponent;
                }
            }
        }

        private void dfs_StartVertex(TVertex v)
        {
            // we are looking on a new tree
            _currentComponent = _componentEquivalences.Count;
            _componentEquivalences.Add(_currentComponent, _currentComponent);
            ComponentCount++;
            Components.Add(v, _currentComponent);
        }

        private void dfs_TreeEdge(TEdge e)
        {
            // new edge, we store with the current component number
            Components.Add(e.Target, _currentComponent);
        }

        #endregion

        #region Members

        private int GetComponentEquivalence(int component)
        {
            var equivalent = component;
            var temp = _componentEquivalences[equivalent];
            var compress = false;
            while (temp != equivalent)
            {
                equivalent = temp;
                temp = _componentEquivalences[equivalent];
                compress = true;
            }

            // path compression
            if (compress)
            {
                var c = component;
                temp = _componentEquivalences[c];
                while (temp != equivalent)
                {
                    temp = _componentEquivalences[c];
                    _componentEquivalences[c] = equivalent;
                }
            }

            return equivalent;
        }

        #endregion
    }
}