using System;

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

        #region IGraph Members

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