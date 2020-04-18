using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using PS.Graph.Collections;

namespace PS.Graph
{
    [Serializable]
    [DebuggerDisplay("VertexCount = {VertexCount}, EdgeCount = {EdgeCount}")]
    public class ClusteredAdjacencyGraph<TVertex, TEdge> : IVertexAndEdgeListGraph<TVertex, TEdge>,
                                                           IEdgeListAndIncidenceGraph<TVertex, TEdge>,
                                                           IClusteredGraph
        where TEdge : IEdge<TVertex>
    {
        #region Static members

        public static Type EdgeType
        {
            get { return typeof(TEdge); }
        }

        #endregion

        private ArrayList _clusters;
        private bool _collapsed;

        #region Constructors

        public ClusteredAdjacencyGraph(AdjacencyGraph<TVertex, TEdge> wrapped)
        {
            Parent = null;
            Wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
            _clusters = new ArrayList();
            _collapsed = false;
        }

        public ClusteredAdjacencyGraph(ClusteredAdjacencyGraph<TVertex, TEdge> parent)
        {
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
            Wrapped = new AdjacencyGraph<TVertex, TEdge>(parent.AllowParallelEdges);
            _clusters = new ArrayList();
        }

        #endregion

        #region Properties

        public int EdgeCapacity
        {
            get { return Wrapped.EdgeCapacity; }
            set { Wrapped.EdgeCapacity = value; }
        }

        public ClusteredAdjacencyGraph<TVertex, TEdge> Parent { get; }

        protected AdjacencyGraph<TVertex, TEdge> Wrapped { get; }

        #endregion

        #region IClusteredGraph Members

        public bool Collapsed
        {
            get { return _collapsed; }
            set { _collapsed = value; }
        }

        public IEnumerable Clusters
        {
            get { return _clusters; }
        }

        public int ClustersCount
        {
            get { return _clusters.Count; }
        }

        IClusteredGraph IClusteredGraph.AddCluster()
        {
            return AddCluster();
        }

        public void RemoveCluster(IClusteredGraph cluster)
        {
            if (cluster == null)
            {
                throw new ArgumentNullException(nameof(cluster));
            }

            _clusters.Remove(cluster);
        }

        #endregion

        #region IVertexAndEdgeListGraph<TVertex,TEdge> Members

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

        #endregion

        #region Members

        public ClusteredAdjacencyGraph<TVertex, TEdge> AddCluster()
        {
            var cluster = new ClusteredAdjacencyGraph<TVertex, TEdge>(this);
            _clusters.Add(cluster);
            return cluster;
        }

        public virtual bool AddEdge(TEdge e)
        {
            Wrapped.AddEdge(e);
            if (Parent != null && !Parent.ContainsEdge(e))
            {
                Parent.AddEdge(e);
            }

            return true;
        }

        public int AddEdgeRange(IEnumerable<TEdge> edges)
        {
            var count = 0;
            foreach (var edge in edges)
            {
                if (AddEdge(edge))
                {
                    count++;
                }
            }

            return count;
        }

        public virtual bool AddVertex(TVertex v)
        {
            if (!(Parent == null || Parent.ContainsVertex(v)))
            {
                Parent.AddVertex(v);
                return Wrapped.AddVertex(v);
            }

            return Wrapped.AddVertex(v);
        }

        public virtual int AddVertexRange(IEnumerable<TVertex> vertices)
        {
            var count = 0;
            foreach (var v in vertices)
            {
                if (AddVertex(v))
                {
                    count++;
                }
            }

            return count;
        }

        public virtual bool AddVerticesAndEdge(TEdge e)
        {
            AddVertex(e.Source);
            AddVertex(e.Target);
            return AddEdge(e);
        }

        public int AddVerticesAndEdgeRange(IEnumerable<TEdge> edges)
        {
            var count = 0;
            foreach (var edge in edges)
            {
                if (AddVerticesAndEdge(edge))
                {
                    count++;
                }
            }

            return count;
        }

        public void Clear()
        {
            Wrapped.Clear();
            _clusters.Clear();
        }

        public void ClearOutEdges(TVertex v)
        {
            Wrapped.ClearOutEdges(v);
        }

        public virtual bool RemoveEdge(TEdge e)
        {
            if (!Wrapped.ContainsEdge(e))
            {
                return false;
            }

            RemoveChildEdge(e);
            Wrapped.RemoveEdge(e);
            Parent?.RemoveEdge(e);

            return true;
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

        public virtual bool RemoveVertex(TVertex v)
        {
            if (!Wrapped.ContainsVertex(v))
            {
                return false;
            }

            RemoveChildVertex(v);
            Wrapped.RemoveVertex(v);
            Parent?.RemoveVertex(v);

            return true;
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

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(Wrapped.EdgeCount >= 0);
        }

        private void RemoveChildEdge(TEdge e)
        {
            foreach (ClusteredAdjacencyGraph<TVertex, TEdge> el in Clusters)
            {
                if (el.ContainsEdge(e))
                {
                    el.Wrapped.RemoveEdge(e);
                    el.RemoveChildEdge(e);
                    break;
                }
            }
        }

        private void RemoveChildVertex(TVertex v)
        {
            foreach (ClusteredAdjacencyGraph<TVertex, TEdge> el in Clusters)
            {
                if (el.ContainsVertex(v))
                {
                    el.Wrapped.RemoveVertex(v);
                    el.RemoveChildVertex(v);
                    break;
                }
            }
        }

        #endregion
    }
}