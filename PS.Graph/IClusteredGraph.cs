using System.Collections.Generic;

namespace PS.Graph
{
    public interface IClusteredGraph<TVertex, TEdge> : IEdgeListAndIncidenceGraph<TVertex, TEdge>,
                                                       IMutableVertexAndEdgeListGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Properties

        /// <summary>
        ///     Gets the cluster count.
        /// </summary>
        /// <value>The cluster count.</value>
        int ClusterCount { get; }

        /// <summary>
        ///     Gets the clusters.
        /// </summary>
        /// <value>The cluster.</value>
        IEnumerable<IClusteredGraph<TVertex, TEdge>> Clusters { get; }

        /// <summary>
        ///     Gets a value indicating whether there are no children clusters in this graph.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the cluster set is empty; otherwise, <c>false</c>.
        /// </value>
        bool IsClustersEmpty { get; }

        /// <summary>
        ///     Parent cluster graph
        /// </summary>
        IClusteredGraph<TVertex, TEdge> Parent { get; }

        #endregion

        #region Events

        event ClusterAction<TVertex, TEdge> ClusterAdded;

        event ClusterAction<TVertex, TEdge> ClusterRemoved;

        #endregion

        #region Members

        /// <summary>
        ///     Adds child cluster graph to this cluster graph
        /// </summary>
        /// <returns>Child cluster graph</returns>
        IClusteredGraph<TVertex, TEdge> AddCluster();
        //IClusteredGraph<TVertex, TEdge> AddCluster(IMutableVertexAndEdgeListGraph<TVertex, TEdge> cluster);

        /// <summary>
        ///     Determines whether the specified cluster set contains cluster.
        /// </summary>
        /// <param name="cluster">The cluster.</param>
        /// <returns>
        ///     <c>true</c> if the specified cluster set contains cluster; otherwise, <c>false</c>.
        /// </returns>
        bool ContainsCluster(IClusteredGraph<TVertex, TEdge> cluster);

        bool RemoveCluster(IClusteredGraph<TVertex, TEdge> cluster);

        #endregion
    }
}