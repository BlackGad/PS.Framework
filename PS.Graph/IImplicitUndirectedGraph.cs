using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace PS.Graph
{
    public interface IImplicitUndirectedGraph<in TVertex, TEdge> : IImplicitVertexSet<TVertex>,
                                                                   IGraph
        where TEdge : IEdge<TVertex>
    {
        #region Properties

        [Pure]
        EdgeEqualityComparer<TVertex, TEdge> EdgeEqualityComparer { get; }

        #endregion

        #region Members

        [Pure]
        int AdjacentDegree(TVertex v);

        [Pure]
        TEdge AdjacentEdge(TVertex v, int index);

        [Pure]
        IEnumerable<TEdge> AdjacentEdges(TVertex v);

        [Pure]
        bool ContainsEdge(TVertex source, TVertex target);

        [Pure]
        bool IsAdjacentEdgesEmpty(TVertex v);

        [Pure]
        bool TryGetEdge(TVertex source, TVertex target, out TEdge edge);

        #endregion
    }
}