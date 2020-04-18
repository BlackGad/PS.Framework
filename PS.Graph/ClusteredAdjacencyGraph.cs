using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PS.Graph.Collections;

namespace PS.Graph
{
    [Serializable]
    [DebuggerDisplay("VertexCount = {VertexCount}, EdgeCount = {EdgeCount}, ClusterCount = {ClusterCount}")]
    public class ClusteredAdjacencyGraph<TVertex, TEdge> : IClusteredAdjacencyGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Static members

        public static Type EdgeType
        {
            get { return typeof(TEdge); }
        }

        #endregion

        private List<IClusteredAdjacencyGraph<TVertex, TEdge>> _clusters;
        private bool _collapsed;

        #region Constructors

        public ClusteredAdjacencyGraph(AdjacencyGraph<TVertex, TEdge> wrapped)
        {
            Parent = null;
            Wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
            _clusters = new List<IClusteredAdjacencyGraph<TVertex, TEdge>>();
        }

        public ClusteredAdjacencyGraph(ClusteredAdjacencyGraph<TVertex, TEdge> parent)
        {
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
            Wrapped = new AdjacencyGraph<TVertex, TEdge>(parent.AllowParallelEdges);
            _clusters = new List<IClusteredAdjacencyGraph<TVertex, TEdge>>();
        }

        #endregion

        #region Properties

        protected AdjacencyGraph<TVertex, TEdge> Wrapped { get; }

        #endregion

        #region IClusteredAdjacencyGraph<TVertex,TEdge> Members

        public bool IsDirected
        {
            get { return Wrapped.IsDirected; }
        }

        public bool AllowParallelEdges
        {
            get { return Wrapped.AllowParallelEdges; }
        }

        public bool IsVerticesEmpty
        {
            get { return Wrapped.IsVerticesEmpty; }
        }

        public int VertexCount
        {
            get { return Wrapped.VertexCount; }
        }

        public virtual IEnumerable<TVertex> Vertices
        {
            get { return Wrapped.Vertices; }
        }

        public bool ContainsVertex(TVertex v)
        {
            return Wrapped.ContainsVertex(v);
        }

        public bool IsOutEdgesEmpty(TVertex v)
        {
            return Wrapped.IsOutEdgesEmpty(v);
        }

        public int OutDegree(TVertex v)
        {
            return Wrapped.OutDegree(v);
        }

        public virtual IEnumerable<TEdge> OutEdges(TVertex v)
        {
            return Wrapped.OutEdges(v);
        }

        public virtual bool TryGetOutEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            return Wrapped.TryGetOutEdges(v, out edges);
        }

        public TEdge OutEdge(TVertex v, int index)
        {
            return Wrapped.OutEdge(v, index);
        }

        public bool IsEdgesEmpty
        {
            get { return Wrapped.IsEdgesEmpty; }
        }

        public int EdgeCount
        {
            get { return Wrapped.EdgeCount; }
        }

        public virtual IEnumerable<TEdge> Edges
        {
            get { return Wrapped.Edges; }
        }

        public bool ContainsEdge(TVertex source, TVertex target)
        {
            return Wrapped.ContainsEdge(source, target);
        }

        public bool ContainsEdge(TEdge edge)
        {
            return Wrapped.ContainsEdge(edge);
        }

        public bool TryGetEdge(
            TVertex source,
            TVertex target,
            out TEdge edge)
        {
            return Wrapped.TryGetEdge(source, target, out edge);
        }

        public virtual bool TryGetEdges(
            TVertex source,
            TVertex target,
            out IEnumerable<TEdge> edges)
        {
            return Wrapped.TryGetEdges(source, target, out edges);
        }

        public bool Collapsed
        {
            get { return _collapsed; }
            set
            {
                if (_collapsed == value) return;
                _collapsed = value;
                CollapsedChanged?.Invoke();
            }
        }

        public IClusteredAdjacencyGraph<TVertex, TEdge> Parent { get; }

        public event Action CollapsedChanged;

        public bool IsClustersEmpty
        {
            get { return !_clusters.Any(); }
        }

        public int ClusterCount
        {
            get { return _clusters.Count; }
        }

        public IEnumerable<IClusteredAdjacencyGraph<TVertex, TEdge>> Clusters
        {
            get { return _clusters; }
        }

        public event ClusterAction<TVertex, TEdge> ClusterAdded;

        public event ClusterAction<TVertex, TEdge> ClusterRemoved;

        public IClusteredAdjacencyGraph<TVertex, TEdge> AddCluster()
        {
            var cluster = new ClusteredAdjacencyGraph<TVertex, TEdge>(this);
            _clusters.Add(cluster);
            ClusterAdded?.Invoke(cluster);
            return cluster;
        }

        public bool RemoveCluster(IClusteredAdjacencyGraph<TVertex, TEdge> cluster)
        {
            if (!_clusters.Remove(cluster)) return false;
            ClusterRemoved?.Invoke(cluster);
            return true;
        }

        public bool ContainsCluster(IClusteredAdjacencyGraph<TVertex, TEdge> cluster)
        {
            return _clusters.Contains(cluster);
        }

        public event EdgeAction<TVertex, TEdge> EdgeAdded
        {
            add { Wrapped.EdgeAdded += value; }
            remove { Wrapped.EdgeAdded -= value; }
        }

        public event EdgeAction<TVertex, TEdge> EdgeRemoved
        {
            add { Wrapped.EdgeRemoved += value; }
            remove { Wrapped.EdgeRemoved -= value; }
        }

        public virtual bool AddEdge(TEdge e)
        {
            if (!Wrapped.AddEdge(e)) return false;
            Parent?.AddEdge(e);
            return true;
        }

        public int AddEdgeRange(IEnumerable<TEdge> edges)
        {
            return edges.Count(AddEdge);
        }

        public event VertexAction<TVertex> VertexAdded
        {
            add { Wrapped.VertexAdded += value; }
            remove { Wrapped.VertexAdded -= value; }
        }

        public event VertexAction<TVertex> VertexRemoved
        {
            add { Wrapped.VertexRemoved += value; }
            remove { Wrapped.VertexRemoved -= value; }
        }

        public virtual bool AddVertex(TVertex v)
        {
            if (!Wrapped.AddVertex(v)) return false;
            Parent?.AddVertex(v);
            return true;
        }

        public virtual int AddVertexRange(IEnumerable<TVertex> vertices)
        {
            return vertices.Count(AddVertex);
        }

        public virtual bool AddVerticesAndEdge(TEdge e)
        {
            AddVertex(e.Source);
            AddVertex(e.Target);
            return AddEdge(e);
        }

        public int AddVerticesAndEdgeRange(IEnumerable<TEdge> edges)
        {
            return edges.Count(AddVerticesAndEdge);
        }

        public event EventHandler Cleared
        {
            add { Wrapped.Cleared += value; }
            remove { Wrapped.Cleared -= value; }
        }

        public void Clear()
        {
            foreach (var vertex in Vertices.ToList())
            {
                RemoveVertex(vertex);
            }
        }

        public void ClearOutEdges(TVertex v)
        {
            if (Wrapped.TryGetOutEdges(v, out var outEdges))
            {
                foreach (var outEdge in outEdges.ToList())
                {
                    RemoveEdge(outEdge);
                }
            }
        }

        public virtual bool RemoveEdge(TEdge e)
        {
            if (Wrapped.RemoveEdge(e))
            {
                foreach (var cluster in Clusters)
                {
                    cluster.RemoveEdge(e);
                }

                Parent?.RemoveEdge(e);

                return true;
            }

            return false;
        }

        public int RemoveEdgeIf(EdgePredicate<TVertex, TEdge> predicate)
        {
            var edges = new EdgeList<TVertex, TEdge>();
            foreach (var edge in Wrapped.Edges)
            {
                if (predicate(edge))
                {
                    edges.Add(edge);
                }
            }

            foreach (var edge in edges)
            {
                RemoveEdge(edge);
            }

            return edges.Count;
        }

        public int RemoveOutEdgeIf(TVertex v, EdgePredicate<TVertex, TEdge> predicate)
        {
            var edgeToRemoveCount = Wrapped.RemoveOutEdgeIf(v, predicate);
            Parent?.RemoveOutEdgeIf(v, predicate);

            return edgeToRemoveCount;
        }

        public void TrimEdgeExcess()
        {
            Wrapped.TrimEdgeExcess();
        }

        public virtual bool RemoveVertex(TVertex v)
        {
            if (Wrapped.RemoveVertex(v))
            {
                foreach (var cluster in Clusters)
                {
                    cluster.RemoveVertex(v);
                }

                Parent?.RemoveVertex(v);

                return true;
            }

            return false;
        }

        public int RemoveVertexIf(VertexPredicate<TVertex> predicate)
        {
            var vertices = new VertexList<TVertex>();
            foreach (var v in Wrapped.Vertices)
            {
                if (predicate(v))
                {
                    vertices.Add(v);
                }
            }

            foreach (var v in vertices)
            {
                RemoveVertex(v);
            }

            return vertices.Count;
        }

        #endregion
    }
}