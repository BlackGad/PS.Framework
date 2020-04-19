using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PS.Graph
{
    [Serializable]
    [DebuggerDisplay("VertexCount = {VertexCount}, EdgeCount = {EdgeCount}, ClusterCount = {ClusterCount}")]
    public class ClusteredAdjacencyGraph<TVertex, TEdge> : AdjacencyGraph<TVertex, TEdge>,
                                                           IClusteredAdjacencyGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private List<IClusteredGraph<TVertex, TEdge>> _clusters;

        #region Constructors

        public ClusteredAdjacencyGraph()
            : this(true)
        {
        }

        public ClusteredAdjacencyGraph(bool allowParallelEdges)
            : base(allowParallelEdges)
        {
            _clusters = new List<IClusteredGraph<TVertex, TEdge>>();
        }

        #endregion

        #region Override members

        protected override void OnCleared(IReadOnlyList<TVertex> obsoleteVertices, IReadOnlyList<TEdge> obsoleteEdges)
        {
            foreach (var vertex in obsoleteVertices)
            {
                Parent?.RemoveVertex(vertex);
            }

            foreach (var cluster in Clusters)
            {
                cluster.Clear();
            }

            _clusters.Clear();

            base.OnCleared(obsoleteVertices, obsoleteEdges);
        }

        protected override void OnEdgeAdded(TEdge args)
        {
            Parent?.AddEdge(args);
            base.OnEdgeAdded(args);
        }

        protected override void OnEdgeRemoved(TEdge args)
        {
            foreach (var cluster in Clusters)
            {
                cluster.RemoveEdge(args);
            }

            Parent?.RemoveEdge(args);

            base.OnEdgeRemoved(args);
        }

        protected override void OnVertexAdded(TVertex args)
        {
            Parent?.AddVertex(args);
            base.OnVertexAdded(args);
        }

        protected override void OnVertexRemoved(TVertex args)
        {
            foreach (var cluster in Clusters)
            {
                cluster.RemoveVertex(args);
            }

            Parent?.RemoveVertex(args);

            base.OnVertexRemoved(args);
        }

        public override AdjacencyGraph<TVertex, TEdge> Clone()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IClusteredAdjacencyGraph<TVertex,TEdge> Members

        public int ClusterCount
        {
            get { return _clusters.Count; }
        }

        public virtual IEnumerable<IClusteredGraph<TVertex, TEdge>> Clusters
        {
            get { return _clusters; }
        }

        public virtual bool Collapsed { get; set; }

        public bool IsClustersEmpty
        {
            get { return !_clusters.Any(); }
        }

        public IClusteredGraph<TVertex, TEdge> Parent { get; private set; }
        public event ClusterAction<TVertex, TEdge> ClusterAdded;
        public event ClusterAction<TVertex, TEdge> ClusterRemoved;

        public IClusteredGraph<TVertex, TEdge> AddCluster()
        {
            var cluster = CreateCluster();
            _clusters.Add(cluster);
            OnClusterAdded(cluster);
            return cluster;
        }

        public bool ContainsCluster(IClusteredGraph<TVertex, TEdge> cluster)
        {
            return _clusters.Contains(cluster);
        }

        public bool RemoveCluster(IClusteredGraph<TVertex, TEdge> cluster)
        {
            if (_clusters.Remove(cluster))
            {
                OnClusterRemoved(cluster);
                return true;
            }

            return false;
        }

        #endregion

        #region Members

        protected virtual IClusteredGraph<TVertex, TEdge> CreateCluster()
        {
            return new ClusteredAdjacencyGraph<TVertex, TEdge>
            {
                Parent = this
            };
        }

        protected virtual void OnClusterAdded(IClusteredGraph<TVertex, TEdge> args)
        {
            ClusterAdded?.Invoke(args);
        }

        protected virtual void OnClusterRemoved(IClusteredGraph<TVertex, TEdge> args)
        {
            ClusterRemoved?.Invoke(args);
        }

        #endregion
    }
}