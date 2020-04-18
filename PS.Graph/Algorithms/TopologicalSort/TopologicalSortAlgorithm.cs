using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using PS.Graph.Algorithms.Search;

namespace PS.Graph.Algorithms.TopologicalSort
{
    [Serializable]
    public sealed class TopologicalSortAlgorithm<TVertex, TEdge> : AlgorithmBase<IVertexListGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        #region Constructors

        public TopologicalSortAlgorithm(IVertexListGraph<TVertex, TEdge> g)
            : this(g, new List<TVertex>())
        {
        }

        public TopologicalSortAlgorithm(
            IVertexListGraph<TVertex, TEdge> g,
            IList<TVertex> vertices)
            : base(g)
        {
            Contract.Requires(vertices != null);

            SortedVertices = vertices;
        }

        #endregion

        #region Properties

        public bool AllowCyclicGraph { get; } = false;

        public IList<TVertex> SortedVertices { get; private set; }

        #endregion

        #region Override members

        protected override void InternalCompute()
        {
            DepthFirstSearchAlgorithm<TVertex, TEdge> dfs = null;
            try
            {
                dfs = new DepthFirstSearchAlgorithm<TVertex, TEdge>(
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

        private void BackEdge(TEdge args)
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