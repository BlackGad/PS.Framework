using System.Collections.Generic;
using System.Diagnostics.Contracts;
using PS.Graph.Contracts;

namespace PS.Graph
{
    [ContractClass(typeof(IncidenceGraphContract<,>))]
    public interface IIncidenceGraph<TVertex, TEdge> : IImplicitGraph<TVertex, TEdge>
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