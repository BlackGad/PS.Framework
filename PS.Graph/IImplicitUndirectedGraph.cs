using System.Collections.Generic;

namespace PS.Graph
{
    public interface IImplicitUndirectedGraph<in TVertex, TEdge> : IImplicitVertexSet<TVertex>,
                                                                   IGraph
        where TEdge : IEdge<TVertex>
    {
        #region Properties

        EdgeEqualityComparer<TVertex, TEdge> EdgeEqualityComparer { get; }

        #endregion

        #region Members

        int AdjacentDegree(TVertex v);

        TEdge AdjacentEdge(TVertex v, int index);

        IEnumerable<TEdge> AdjacentEdges(TVertex v);

        bool ContainsEdge(TVertex source, TVertex target);

        bool IsAdjacentEdgesEmpty(TVertex v);

        bool TryGetEdge(TVertex source, TVertex target, out TEdge edge);

        #endregion
    }
}