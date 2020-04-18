using System;

namespace PS.Graph.Algorithms.RandomWalks
{
    public interface IMarkovEdgeChain<TVertex, TEdge> : IEdgeChain<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Properties

        Random Rand { get; set; }

        #endregion
    }
}