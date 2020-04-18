using System;
using System.Collections.Generic;
using PS.Graph.Algorithms.Search;

namespace PS.Graph.Algorithms.TopologicalSort
{
    [Serializable]
    public sealed class UndirectedTopologicalSortAlgorithm<TVertex, TEdge> : AlgorithmBase<IUndirectedGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private bool _allowCyclicGraph;

        #region Constructors

        public UndirectedTopologicalSortAlgorithm(IUndirectedGraph<TVertex, TEdge> g)
            : this(g, new List<TVertex>())
        {
        }

        public UndirectedTopologicalSortAlgorithm(
            IUndirectedGraph<TVertex, TEdge> g,
            IList<TVertex> vertices)
            : base(g)
        {
            SortedVertices = vertices;
        }

        #endregion

        #region Properties

        public bool AllowCyclicGraph
        {
            get { return _allowCyclicGraph; }
            set { _allowCyclicGraph = value; }
        }

        public IList<TVertex> SortedVertices { get; private set; }

        #endregion

        #region Override members

        protected override void InternalCompute()
        {
            UndirectedDepthFirstSearchAlgorithm<TVertex, TEdge> dfs = null;
            try
            {
                dfs = new UndirectedDepthFirstSearchAlgorithm<TVertex, TEdge>(
                    this,
                    VisitedGraph,
                    new Dictionary<TVertex, GraphColor>(VisitedGraph.VertexCount)
                );
                dfs.BackEdge += BackEdge;
                dfs.FinishVertex += FinishVertex;

                dfs.Compute();
            }
            finally
            {
                if (dfs != null)
                {
                    dfs.BackEdge -= BackEdge;
                    dfs.FinishVertex -= FinishVertex;
                }
            }
        }

        #endregion

        #region Event handlers

        private void BackEdge(object sender, UndirectedEdgeEventArgs<TVertex, TEdge> a)
        {
            if (!AllowCyclicGraph)
            {
                throw new NonAcyclicGraphException();
            }
        }

        private void FinishVertex(TVertex v)
        {
            SortedVertices.Insert(0, v);
        }

        #endregion

        #region Members

        public void Compute(IList<TVertex> vertices)
        {
            SortedVertices = vertices;
            SortedVertices.Clear();
            Compute();
        }

        #endregion
    }
}