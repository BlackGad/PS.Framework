using System.Collections.Generic;

namespace PS.Graph.Algorithms
{
    public interface IEdgeColorizerAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Properties

        IDictionary<TEdge, GraphColor> EdgeColors { get; }

        #endregion
    }
}