using System.Collections;

namespace PS.Graph
{
    public interface IClusteredGraph
    {
        #region Properties

        IEnumerable Clusters { get; }

        int ClustersCount { get; }

        bool Collapsed { get; set; }

        #endregion

        #region Members

        IClusteredGraph AddCluster();

        void RemoveCluster(IClusteredGraph g);

        #endregion
    }
}