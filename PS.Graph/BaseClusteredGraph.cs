using System;
using System.Collections.Generic;
using System.Linq;
using PS.Graph.Collections;

namespace PS.Graph
{
    [Serializable]
    public abstract class BaseClusteredGraph<TVertex, TEdge> : IClusteredGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private bool _collapsed;

        #region IClusteredGraph<TVertex,TEdge> Members

        IClusteredGraph<TVertex, TEdge> IClusteredGraph<TVertex, TEdge>.Parent
        {
            get { return GetClusterParent(); }
        }

        public bool IsDirected
        {
            get { return GetClusterGraph().IsDirected; }
        }

        public bool AllowParallelEdges
        {
            get { return GetClusterGraph().AllowParallelEdges; }
        }

        public bool IsVerticesEmpty
        {
            get { return GetClusterGraph().IsVerticesEmpty; }
        }

        public int VertexCount
        {
            get { return GetClusterGraph().VertexCount; }
        }

        public virtual IEnumerable<TVertex> Vertices
        {
            get { return GetClusterGraph().Vertices; }
        }

        public bool ContainsVertex(TVertex v)
        {
            return GetClusterGraph().ContainsVertex(v);
        }

        public bool IsOutEdgesEmpty(TVertex v)
        {
            return GetClusterGraph().IsOutEdgesEmpty(v);
        }

        public int OutDegree(TVertex v)
        {
            return GetClusterGraph().OutDegree(v);
        }

        public virtual IEnumerable<TEdge> OutEdges(TVertex v)
        {
            return GetClusterGraph().OutEdges(v);
        }

        public virtual bool TryGetOutEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            return GetClusterGraph().TryGetOutEdges(v, out edges);
        }

        public TEdge OutEdge(TVertex v, int index)
        {
            return GetClusterGraph().OutEdge(v, index);
        }

        public bool IsEdgesEmpty
        {
            get { return GetClusterGraph().IsEdgesEmpty; }
        }

        public int EdgeCount
        {
            get { return GetClusterGraph().EdgeCount; }
        }

        public virtual IEnumerable<TEdge> Edges
        {
            get { return GetClusterGraph().Edges; }
        }

        public bool ContainsEdge(TVertex source, TVertex target)
        {
            return GetClusterGraph().ContainsEdge(source, target);
        }

        public bool ContainsEdge(TEdge edge)
        {
            return GetClusterGraph().ContainsEdge(edge);
        }

        public bool TryGetEdge(
            TVertex source,
            TVertex target,
            out TEdge edge)
        {
            return GetClusterGraph().TryGetEdge(source, target, out edge);
        }

        public virtual bool TryGetEdges(
            TVertex source,
            TVertex target,
            out IEnumerable<TEdge> edges)
        {
            return GetClusterGraph().TryGetEdges(source, target, out edges);
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

        public event Action CollapsedChanged;

        public bool IsClustersEmpty
        {
            get { return !GetClusterChildren().Any(); }
        }

        public int ClusterCount
        {
            get { return GetClusterChildren().Count; }
        }

        public IEnumerable<IClusteredGraph<TVertex, TEdge>> Clusters
        {
            get { return GetClusterChildren(); }
        }

        public event ClusterAction<TVertex, TEdge> ClusterAdded;

        public event ClusterAction<TVertex, TEdge> ClusterRemoved;

        public event EdgeAction<TVertex, TEdge> EdgeAdded
        {
            add { GetClusterGraph().EdgeAdded += value; }
            remove { GetClusterGraph().EdgeAdded -= value; }
        }

        public event EdgeAction<TVertex, TEdge> EdgeRemoved
        {
            add { GetClusterGraph().EdgeRemoved += value; }
            remove { GetClusterGraph().EdgeRemoved -= value; }
        }

        public virtual bool AddEdge(TEdge e)
        {
            if (!GetClusterGraph().AddEdge(e)) return false;
            GetClusterParent()?.AddEdge(e);
            return true;
        }

        public int AddEdgeRange(IEnumerable<TEdge> edges)
        {
            return edges.Count(AddEdge);
        }

        public event VertexAction<TVertex> VertexAdded
        {
            add { GetClusterGraph().VertexAdded += value; }
            remove { GetClusterGraph().VertexAdded -= value; }
        }

        public event VertexAction<TVertex> VertexRemoved
        {
            add { GetClusterGraph().VertexRemoved += value; }
            remove { GetClusterGraph().VertexRemoved -= value; }
        }

        public virtual bool AddVertex(TVertex v)
        {
            if (!GetClusterGraph().AddVertex(v)) return false;
            GetClusterParent()?.AddVertex(v);
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
            add { GetClusterGraph().Cleared += value; }
            remove { GetClusterGraph().Cleared -= value; }
        }

        public void Clear()
        {
            var vertices = Vertices.ToList();
            GetClusterGraph().Clear();

            foreach (var vertex in vertices)
            {
                GetClusterParent()?.RemoveVertex(vertex);
            }

            foreach (var cluster in Clusters)
            {
                cluster.Clear();
            }

            GetClusterChildren().Clear();
        }

        public void ClearOutEdges(TVertex v)
        {
            if (GetClusterGraph().TryGetOutEdges(v, out var outEdges))
            {
                foreach (var outEdge in outEdges.ToList())
                {
                    RemoveEdge(outEdge);
                }
            }
        }

        public virtual bool RemoveEdge(TEdge e)
        {
            if (GetClusterGraph().RemoveEdge(e))
            {
                foreach (var cluster in Clusters)
                {
                    cluster.RemoveEdge(e);
                }

                GetClusterParent()?.RemoveEdge(e);

                return true;
            }

            return false;
        }

        public int RemoveEdgeIf(EdgePredicate<TVertex, TEdge> predicate)
        {
            var edges = new EdgeList<TVertex, TEdge>();
            foreach (var edge in GetClusterGraph().Edges)
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
            var edgeToRemoveCount = GetClusterGraph().RemoveOutEdgeIf(v, predicate);
            GetClusterParent()?.RemoveOutEdgeIf(v, predicate);

            return edgeToRemoveCount;
        }

        public void TrimEdgeExcess()
        {
            GetClusterGraph().TrimEdgeExcess();
        }

        public virtual bool RemoveVertex(TVertex v)
        {
            if (GetClusterGraph().RemoveVertex(v))
            {
                foreach (var cluster in Clusters)
                {
                    cluster.RemoveVertex(v);
                }

                GetClusterParent()?.RemoveVertex(v);

                return true;
            }

            return false;
        }

        public int RemoveVertexIf(VertexPredicate<TVertex> predicate)
        {
            var vertices = new VertexList<TVertex>();
            foreach (var v in GetClusterGraph().Vertices)
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

        public IClusteredGraph<TVertex, TEdge> AddCluster()
        {
            var cluster = CreateChildCluster();
            GetClusterChildren().Add(cluster);
            ClusterAdded?.Invoke(cluster);
            return cluster;
        }

        public bool ContainsCluster(IClusteredGraph<TVertex, TEdge> cluster)
        {
            return GetClusterChildren().Contains(cluster);
        }

        public bool RemoveCluster(IClusteredGraph<TVertex, TEdge> cluster)
        {
            if (!GetClusterChildren().Remove(cluster)) return false;
            ClusterRemoved?.Invoke(cluster);
            return true;
        }

        #endregion

        #region Members

        protected abstract IClusteredGraph<TVertex, TEdge> CreateChildCluster();
        protected abstract IList<IClusteredGraph<TVertex, TEdge>> GetClusterChildren();
        protected abstract IMutableVertexAndEdgeListGraph<TVertex, TEdge> GetClusterGraph();
        protected abstract IClusteredGraph<TVertex, TEdge> GetClusterParent();

        #endregion
    }
}