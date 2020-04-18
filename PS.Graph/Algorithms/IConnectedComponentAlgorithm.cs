using System.Collections.Generic;

namespace PS.Graph.Algorithms
{
    public interface IConnectedComponentAlgorithm<TVertex, out TGraph> : IAlgorithm<TGraph>
        where TGraph : IGraph
    {
        #region Properties

        int ComponentCount { get; }
        IDictionary<TVertex, int> Components { get; }

        #endregion
    }
}