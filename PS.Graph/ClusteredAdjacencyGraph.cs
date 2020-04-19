using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PS.Graph
{
    [Serializable]
    [DebuggerDisplay("VertexCount = {VertexCount}, EdgeCount = {EdgeCount}, ClusterCount = {ClusterCount}")]
    public class ClusteredAdjacencyGraph<TVertex, TEdge> : BaseClusteredGraph<TVertex, TEdge>,
                                                           IClusteredAdjacencyGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private IList<IClusteredGraph<TVertex, TEdge>> _children;
        private IMutableVertexAndEdgeListGraph<TVertex, TEdge> _graph;
        private IClusteredGraph<TVertex, TEdge> _parent;

        #region Constructors

        public ClusteredAdjacencyGraph(IMutableVertexAndEdgeListGraph<TVertex, TEdge> wrapped)
            : this()
        {
            _graph = wrapped;
        }

        private ClusteredAdjacencyGraph()
        {
            _children = new List<IClusteredGraph<TVertex, TEdge>>();
        }

        private ClusteredAdjacencyGraph(IClusteredGraph<TVertex, TEdge> parent)
            : this()
        {
            _parent = parent;
            _graph = new AdjacencyGraph<TVertex, TEdge>();
        }

        #endregion

        #region Override members

        protected override IClusteredGraph<TVertex, TEdge> CreateChildCluster()
        {
            return new ClusteredAdjacencyGraph<TVertex, TEdge>(this);
        }

        protected override IList<IClusteredGraph<TVertex, TEdge>> GetClusterChildren()
        {
            return _children;
        }

        protected override IMutableVertexAndEdgeListGraph<TVertex, TEdge> GetClusterGraph()
        {
            return _graph;
        }

        protected override IClusteredGraph<TVertex, TEdge> GetClusterParent()
        {
            return _parent;
        }

        #endregion
    }
}