using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PS.Graph
{
    [Serializable]
    [DebuggerDisplay("VertexCount = {VertexCount}, EdgeCount = {EdgeCount}, ClusterCount = {ClusterCount}")]
    public class ClusteredBidirectionalGraph<TVertex, TEdge> : IClusteredBidirectionalGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private List<IClusteredGraph<TVertex, TEdge>> _clusters;

        #region Constructors

        public ClusteredBidirectionalGraph(IGraph wrappedGraph)
            : this(wrappedGraph, null)
        {
        }

        public ClusteredBidirectionalGraph()
            : this(new BidirectionalGraph<TVertex, TEdge>(), null)
        {
        }

        private ClusteredBidirectionalGraph(IGraph wrappedGraph,
                                            IClusteredGraph<TVertex, TEdge> parentCluster)
        {
            WrappedGraph = wrappedGraph ?? throw new ArgumentNullException(nameof(wrappedGraph));
            ParentCluster = parentCluster;

            _clusters = new List<IClusteredGraph<TVertex, TEdge>>();

            if (wrappedGraph is IMutableVertexSet<TVertex> mutableVertexSet)
            {
                mutableVertexSet.VertexAdded += WrappedGraphOnVertexAdded;
                mutableVertexSet.VertexRemoved += WrappedGraphOnVertexRemoved;
            }

            if (wrappedGraph is IMutableEdgeListGraph<TVertex, TEdge> mutableEdgeListGraph)
            {
                mutableEdgeListGraph.EdgeAdded += WrappedGraphOnEdgeAdded;
                mutableEdgeListGraph.EdgeRemoved += WrappedGraphOnEdgeRemoved;
            }

            if (wrappedGraph is IMutableGraph<TVertex, TEdge> mutableGraph)
            {
                mutableGraph.Cleared += WrappedGraphOnCleared;
            }
        }

        #endregion

        #region IClusteredBidirectionalGraph<TVertex,TEdge> Members

        public int ClusterCount
        {
            get { return _clusters.Count; }
        }

        public virtual IEnumerable<IClusteredGraph<TVertex, TEdge>> Clusters
        {
            get { return _clusters; }
        }

        public bool IsClustersEmpty
        {
            get { return !_clusters.Any(); }
        }

        public IClusteredGraph<TVertex, TEdge> ParentCluster { get; }

        public IGraph WrappedGraph { get; }

        public event ClusterAction<TVertex, TEdge> ClusterAdded;
        public event ClusterAction<TVertex, TEdge> ClusterRemoved;

        public IClusteredGraph<TVertex, TEdge> AddCluster(IGraph graph)
        {
            var cluster = CreateCluster(graph ?? new BidirectionalGraph<TVertex, TEdge>());

            foreach (var vertex in cluster.Vertices)
            {
                if(ContainsVertex(vertex)) continue;
                AddVertex(vertex);
            }

            foreach (var edge in cluster.Edges)
            {
                if(ContainsEdge(edge)) continue;
                AddEdge(edge);
            }

            _clusters.Add(cluster);
            OnClusterAdded(cluster);
            return cluster;
        }

        public IReadOnlyList<IClusteredGraph<TVertex, TEdge>> AddClusterRange(IEnumerable<IEdgeListGraph<TVertex, TEdge>> graphs)
        {
            graphs = graphs ?? Enumerable.Empty<IEdgeListGraph<TVertex, TEdge>>();
            var result = new List<IClusteredGraph<TVertex, TEdge>>();
            foreach (var graph in graphs)
            {
                result.Add(AddCluster(graph));
            }

            return result;
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

        public bool AllowParallelEdges
        {
            get
            {
                if (ParentCluster?.AllowParallelEdges == false) return false;
                return WrappedGraph.AllowParallelEdges;
            }
        }

        public bool IsDirected
        {
            get
            {
                if (ParentCluster?.IsDirected == false) return false;
                return WrappedGraph.IsDirected;
            }
        }

        public event ClearAction<TVertex, TEdge> Cleared
        {
            add { GetWrappedGraph<IMutableGraph<TVertex, TEdge>>().Cleared += value; }
            remove { GetWrappedGraph<IMutableGraph<TVertex, TEdge>>().Cleared -= value; }
        }

        public void Clear()
        {
            GetWrappedGraph<IMutableGraph<TVertex, TEdge>>().Clear();
        }

        public bool ContainsVertex(TVertex vertex)
        {
            return GetWrappedGraph<IImplicitVertexSet<TVertex>>().ContainsVertex(vertex);
        }

        public bool IsOutEdgesEmpty(TVertex v)
        {
            return GetWrappedGraph<IImplicitGraph<TVertex, TEdge>>().IsOutEdgesEmpty(v);
        }

        public int OutDegree(TVertex v)
        {
            return GetWrappedGraph<IImplicitGraph<TVertex, TEdge>>().OutDegree(v);
        }

        public TEdge OutEdge(TVertex v, int index)
        {
            return GetWrappedGraph<IImplicitGraph<TVertex, TEdge>>().OutEdge(v, index);
        }

        public IEnumerable<TEdge> OutEdges(TVertex v)
        {
            return GetWrappedGraph<IImplicitGraph<TVertex, TEdge>>().OutEdges(v);
        }

        public bool TryGetOutEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            return GetWrappedGraph<IImplicitGraph<TVertex, TEdge>>().TryGetOutEdges(v, out edges);
        }

        public bool ContainsEdge(TVertex source, TVertex target)
        {
            return GetWrappedGraph<IIncidenceGraph<TVertex, TEdge>>().ContainsEdge(source, target);
        }

        public bool TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            return GetWrappedGraph<IIncidenceGraph<TVertex, TEdge>>().TryGetEdge(source, target, out edge);
        }

        public bool TryGetEdges(TVertex source, TVertex target, out IEnumerable<TEdge> edges)
        {
            return GetWrappedGraph<IIncidenceGraph<TVertex, TEdge>>().TryGetEdges(source, target, out edges);
        }

        public void ClearOutEdges(TVertex v)
        {
            GetWrappedGraph<IMutableIncidenceGraph<TVertex, TEdge>>().ClearOutEdges(v);
        }

        public int RemoveOutEdgeIf(TVertex v, EdgePredicate<TVertex, TEdge> predicate)
        {
            return GetWrappedGraph<IMutableIncidenceGraph<TVertex, TEdge>>().RemoveOutEdgeIf(v, predicate);
        }

        public void TrimEdgeExcess()
        {
            GetWrappedGraph<IMutableIncidenceGraph<TVertex, TEdge>>().TrimEdgeExcess();
        }

        public bool IsVerticesEmpty
        {
            get { return GetWrappedGraph<IVertexSet<TVertex>>().IsVerticesEmpty; }
        }

        public int VertexCount
        {
            get { return GetWrappedGraph<IVertexSet<TVertex>>().VertexCount; }
        }

        public IEnumerable<TVertex> Vertices
        {
            get { return GetWrappedGraph<IVertexSet<TVertex>>().Vertices; }
        }

        public event VertexAction<TVertex> VertexAdded
        {
            add { GetWrappedGraph<IMutableVertexSet<TVertex>>().VertexAdded += value; }
            remove { GetWrappedGraph<IMutableVertexSet<TVertex>>().VertexAdded -= value; }
        }

        public event VertexAction<TVertex> VertexRemoved
        {
            add { GetWrappedGraph<IMutableVertexSet<TVertex>>().VertexRemoved += value; }
            remove { GetWrappedGraph<IMutableVertexSet<TVertex>>().VertexRemoved -= value; }
        }

        public bool AddVertex(TVertex v)
        {
            return GetWrappedGraph<IMutableVertexSet<TVertex>>().AddVertex(v);
        }

        public int AddVertexRange(IEnumerable<TVertex> vertices)
        {
            return GetWrappedGraph<IMutableVertexSet<TVertex>>().AddVertexRange(vertices);
        }

        public bool RemoveVertex(TVertex v)
        {
            return GetWrappedGraph<IMutableVertexSet<TVertex>>().RemoveVertex(v);
        }

        public int RemoveVertexIf(VertexPredicate<TVertex> predicate)
        {
            return GetWrappedGraph<IMutableVertexSet<TVertex>>().RemoveVertexIf(predicate);
        }

        public int EdgeCount
        {
            get { return GetWrappedGraph<IEdgeSet<TVertex, TEdge>>().EdgeCount; }
        }

        public IEnumerable<TEdge> Edges
        {
            get { return GetWrappedGraph<IEdgeSet<TVertex, TEdge>>().Edges; }
        }

        public bool IsEdgesEmpty
        {
            get { return GetWrappedGraph<IEdgeSet<TVertex, TEdge>>().IsEdgesEmpty; }
        }

        public bool ContainsEdge(TEdge edge)
        {
            return GetWrappedGraph<IEdgeSet<TVertex, TEdge>>().ContainsEdge(edge);
        }

        public event EdgeAction<TVertex, TEdge> EdgeAdded
        {
            add { GetWrappedGraph<IMutableEdgeListGraph<TVertex, TEdge>>().EdgeAdded += value; }
            remove { GetWrappedGraph<IMutableEdgeListGraph<TVertex, TEdge>>().EdgeAdded -= value; }
        }

        public event EdgeAction<TVertex, TEdge> EdgeRemoved
        {
            add { GetWrappedGraph<IMutableEdgeListGraph<TVertex, TEdge>>().EdgeRemoved += value; }
            remove { GetWrappedGraph<IMutableEdgeListGraph<TVertex, TEdge>>().EdgeRemoved -= value; }
        }

        public bool AddEdge(TEdge edge)
        {
            return GetWrappedGraph<IMutableEdgeListGraph<TVertex, TEdge>>().AddEdge(edge);
        }

        public int AddEdgeRange(IEnumerable<TEdge> edges)
        {
            return GetWrappedGraph<IMutableEdgeListGraph<TVertex, TEdge>>().AddEdgeRange(edges);
        }

        public bool RemoveEdge(TEdge edge)
        {
            return GetWrappedGraph<IMutableEdgeListGraph<TVertex, TEdge>>().RemoveEdge(edge);
        }

        public int RemoveEdgeIf(EdgePredicate<TVertex, TEdge> predicate)
        {
            return GetWrappedGraph<IMutableEdgeListGraph<TVertex, TEdge>>().RemoveEdgeIf(predicate);
        }

        public bool AddVerticesAndEdge(TEdge edge)
        {
            return GetWrappedGraph<IMutableVertexAndEdgeSet<TVertex, TEdge>>().AddVerticesAndEdge(edge);
        }

        public int AddVerticesAndEdgeRange(IEnumerable<TEdge> edges)
        {
            return GetWrappedGraph<IMutableVertexAndEdgeSet<TVertex, TEdge>>().AddVerticesAndEdgeRange(edges);
        }

        public int Degree(TVertex v)
        {
            return GetWrappedGraph<IBidirectionalIncidenceGraph<TVertex, TEdge>>().Degree(v);
        }

        public int InDegree(TVertex v)
        {
            return GetWrappedGraph<IBidirectionalIncidenceGraph<TVertex, TEdge>>().InDegree(v);
        }

        public TEdge InEdge(TVertex v, int index)
        {
            return GetWrappedGraph<IBidirectionalIncidenceGraph<TVertex, TEdge>>().InEdge(v, index);
        }

        public IEnumerable<TEdge> InEdges(TVertex v)
        {
            return GetWrappedGraph<IBidirectionalIncidenceGraph<TVertex, TEdge>>().InEdges(v);
        }

        public bool IsInEdgesEmpty(TVertex v)
        {
            return GetWrappedGraph<IBidirectionalIncidenceGraph<TVertex, TEdge>>().IsInEdgesEmpty(v);
        }

        public bool TryGetInEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            return GetWrappedGraph<IBidirectionalIncidenceGraph<TVertex, TEdge>>().TryGetInEdges(v, out edges);
        }

        public void ClearEdges(TVertex v)
        {
            GetWrappedGraph<IMutableBidirectionalGraph<TVertex, TEdge>>().ClearEdges(v);
        }

        public void ClearInEdges(TVertex v)
        {
            GetWrappedGraph<IMutableBidirectionalGraph<TVertex, TEdge>>().ClearInEdges(v);
        }

        public int RemoveInEdgeIf(TVertex v, EdgePredicate<TVertex, TEdge> edgePredicate)
        {
            return GetWrappedGraph<IMutableBidirectionalGraph<TVertex, TEdge>>().RemoveInEdgeIf(v, edgePredicate);
        }

        #endregion

        #region Event handlers

        private void WrappedGraphOnCleared(IReadOnlyList<TVertex> obsoleteVertices, IReadOnlyList<TEdge> obsoleteEdges)
        {
            OnCleared(obsoleteVertices, obsoleteEdges);
        }

        private void WrappedGraphOnEdgeAdded(TEdge e)
        {
            OnEdgeAdded(e);
        }

        private void WrappedGraphOnEdgeRemoved(TEdge e)
        {
            OnEdgeRemoved(e);
        }

        private void WrappedGraphOnVertexAdded(TVertex vertex)
        {
            OnVertexAdded(vertex);
        }

        private void WrappedGraphOnVertexRemoved(TVertex vertex)
        {
            OnVertexRemoved(vertex);
        }

        #endregion

        #region Members

        protected virtual IClusteredGraph<TVertex, TEdge> CreateCluster(IGraph graph)
        {
            return new ClusteredBidirectionalGraph<TVertex, TEdge>(graph, this);
        }

        protected virtual void OnCleared(IReadOnlyList<TVertex> obsoleteVertices, IReadOnlyList<TEdge> obsoleteEdges)
        {
            if (ParentCluster != null)
            {
                foreach (var edge in obsoleteEdges)
                {
                    ParentCluster.RemoveEdge(edge);
                }

                foreach (var vertex in obsoleteVertices)
                {
                    ParentCluster.RemoveVertex(vertex);
                }
            }

            foreach (var cluster in Clusters)
            {
                cluster.Clear();
            }

            _clusters.Clear();
        }

        protected virtual void OnClusterAdded(IClusteredGraph<TVertex, TEdge> args)
        {
            ClusterAdded?.Invoke(args);
        }

        protected virtual void OnClusterRemoved(IClusteredGraph<TVertex, TEdge> args)
        {
            ClusterRemoved?.Invoke(args);
        }

        protected virtual void OnEdgeAdded(TEdge args)
        {
            ParentCluster?.AddEdge(args);
        }


        protected virtual void OnEdgeRemoved(TEdge args)
        {
            foreach (var cluster in Clusters)
            {
                if (cluster.ContainsEdge(args)) cluster.RemoveEdge(args);
            }

            ParentCluster?.RemoveEdge(args);
        }

        protected virtual void OnVertexAdded(TVertex args)
        {
            ParentCluster?.AddVertex(args);
        }

        protected virtual void OnVertexRemoved(TVertex args)
        {
            foreach (var cluster in Clusters)
            {
                if (cluster.ContainsVertex(args)) cluster.RemoveVertex(args);
                cluster.RemoveVertex(args);
            }

            ParentCluster?.RemoveVertex(args);
        }

        private T GetWrappedGraph<T>()
        {
            if (WrappedGraph is T graph)
            {
                return graph;
            }

            throw new ClusteredGraphNotSupportFunctionalityException(typeof(T));
        }

        #endregion
    }
}