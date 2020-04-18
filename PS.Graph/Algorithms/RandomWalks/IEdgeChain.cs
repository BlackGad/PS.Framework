using System.Collections.Generic;

namespace PS.Graph.Algorithms.RandomWalks
{
    public interface IEdgeChain<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Members

        bool TryGetSuccessor(IImplicitGraph<TVertex, TEdge> g, TVertex u, out TEdge successor);
        bool TryGetSuccessor(IEnumerable<TEdge> edges, TVertex u, out TEdge successor);

        #endregion
    }
}