using System;
using System.Diagnostics.Contracts;

namespace PS.Graph.Predicates
{
    [Serializable]
    public class FilteredGraph<TVertex, TEdge, TGraph> : IGraph
        where TEdge : IEdge<TVertex>
        where TGraph : IGraph
    {
        #region Constructors

        public FilteredGraph(
            TGraph baseGraph,
            VertexPredicate<TVertex> vertexPredicate,
            EdgePredicate<TVertex, TEdge> edgePredicate
        )
        {
            Contract.Requires(baseGraph != null);
            Contract.Requires(vertexPredicate != null);
            Contract.Requires(edgePredicate != null);

            BaseGraph = baseGraph;
            VertexPredicate = vertexPredicate;
            EdgePredicate = edgePredicate;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Underlying filtered graph
        /// </summary>
        public TGraph BaseGraph { get; }

        /// <summary>
        ///     Edge predicate used to filter the edges
        /// </summary>
        public EdgePredicate<TVertex, TEdge> EdgePredicate { get; }

        public VertexPredicate<TVertex> VertexPredicate { get; }

        #endregion

        #region IGraph<TVertex,TEdge> Members

        public bool IsDirected
        {
            get { return BaseGraph.IsDirected; }
        }

        public bool AllowParallelEdges
        {
            get { return BaseGraph.AllowParallelEdges; }
        }

        #endregion

        #region Members

        protected bool TestEdge(TEdge edge)
        {
            return VertexPredicate(edge.Source)
                   && VertexPredicate(edge.Target)
                   && EdgePredicate(edge);
        }

        #endregion
    }
}