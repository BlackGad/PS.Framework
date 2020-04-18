using System.Collections.Generic;

namespace PS.Graph
{
    public interface IHyperEdge<out TVertex>
    {
        #region Properties

        int EndPointCount { get; }
        IEnumerable<TVertex> EndPoints { get; }

        #endregion
    }
}