using System.Collections.Generic;

namespace PS.Graph
{
    public interface IIncidenceGraph<in TVertex, TEdge> : IImplicitGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Members

        bool ContainsEdge(TVertex source, TVertex target);

        bool TryGetEdge(
            TVertex source,
            TVertex target,
            out TEdge edge);

        bool TryGetEdges(
            TVertex source,
            TVertex target,
            out IEnumerable<TEdge> edges);

        #endregion
    }
}