using System;

namespace PS.Graph.Predicates
{
    [Serializable]
    public class FilteredImplicitVertexSet<TVertex, TEdge, TGraph> : FilteredGraph<TVertex, TEdge, TGraph>,
                                                                     IImplicitVertexSet<TVertex>
        where TEdge : IEdge<TVertex>
        where TGraph : IGraph, IImplicitVertexSet<TVertex>
    {
        #region Constructors

        public FilteredImplicitVertexSet(
            TGraph baseGraph,
            VertexPredicate<TVertex> vertexPredicate,
            EdgePredicate<TVertex, TEdge> edgePredicate
        )
            : base(baseGraph, vertexPredicate, edgePredicate)
        {
        }

        #endregion

        #region IImplicitVertexSet<TVertex> Members

        public bool ContainsVertex(TVertex vertex)
        {
            return
                VertexPredicate(vertex) &&
                BaseGraph.ContainsVertex(vertex);
        }

        #endregion
    }
}