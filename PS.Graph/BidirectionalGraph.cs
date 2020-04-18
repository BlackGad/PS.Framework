using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using PS.Graph.Collections;

namespace PS.Graph
{
    /// <summary>
    ///     A mutable directed graph data structure efficient for sparse
    ///     graph representation where out-edge and in-edges need to be enumerated. Requires
    ///     twice as much memory as the adjacency graph.
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    [Serializable]
    [DebuggerDisplay("VertexCount = {VertexCount}, EdgeCount = {EdgeCount}")]
    public class BidirectionalGraph<TVertex, TEdge> : IEdgeListAndIncidenceGraph<TVertex, TEdge>,
                                                      IMutableBidirectionalGraph<TVertex, TEdge>,
                                                      ICloneable
        where TEdge : IEdge<TVertex>
    {
        private readonly IVertexEdgeDictionary<TVertex, TEdge> _vertexOutEdges;
        private readonly IVertexEdgeDictionary<TVertex, TEdge> _vertexInEdges;
        private int _edgeCapacity = -1;

        public BidirectionalGraph()
            : this(true)
        {
        }

        public BidirectionalGraph(bool allowParallelEdges)
            : this(allowParallelEdges, -1)
        {
        }

        public BidirectionalGraph(bool allowParallelEdges, int vertexCapacity)
            : this(allowParallelEdges, vertexCapacity, -1)
        {
        }

        public BidirectionalGraph(bool allowParallelEdges, int vertexCapacity, int edgeCapacity)
            : this(allowParallelEdges, vertexCapacity, edgeCapacity, EqualityComparer<TVertex>.Default)
        {
        }

        public BidirectionalGraph(bool allowParallelEdges, int vertexCapacity, int edgeCapacity, IEqualityComparer<TVertex> vertexComparer)
        {
            AllowParallelEdges = allowParallelEdges;
            if (vertexCapacity > -1)
            {
                _vertexInEdges = new VertexEdgeDictionary<TVertex, TEdge>(vertexCapacity, vertexComparer);
                _vertexOutEdges = new VertexEdgeDictionary<TVertex, TEdge>(vertexCapacity, vertexComparer);
            }
            else
            {
                _vertexInEdges = new VertexEdgeDictionary<TVertex, TEdge>(vertexComparer);
                _vertexOutEdges = new VertexEdgeDictionary<TVertex, TEdge>(vertexComparer);
            }

            _edgeCapacity = edgeCapacity;
        }

        public BidirectionalGraph(
            bool allowParallelEdges,
            int capacity,
            Func<int, IVertexEdgeDictionary<TVertex, TEdge>> vertexEdgesDictionaryFactory)
        {
            AllowParallelEdges = allowParallelEdges;
            _vertexInEdges = vertexEdgesDictionaryFactory(capacity);
            _vertexOutEdges = vertexEdgesDictionaryFactory(capacity);
        }

        public static Type EdgeType
        {
            get { return typeof(TEdge); }
        }

        public int EdgeCapacity
        {
            get { return _edgeCapacity; }
            set { _edgeCapacity = value; }
        }

        public bool IsDirected { get; } = true;

        public bool AllowParallelEdges { get; }

        public bool IsVerticesEmpty
        {
            get { return _vertexOutEdges.Count == 0; }
        }

        public int VertexCount
        {
            get { return _vertexOutEdges.Count; }
        }

        public virtual IEnumerable<TVertex> Vertices
        {
            get { return _vertexOutEdges.Keys; }
        }

        public bool ContainsVertex(TVertex v)
        {
            return _vertexOutEdges.ContainsKey(v);
        }

        public bool IsOutEdgesEmpty(TVertex v)
        {
            return _vertexOutEdges[v].Count == 0;
        }

        public int OutDegree(TVertex v)
        {
            return _vertexOutEdges[v].Count;
        }

        public IEnumerable<TEdge> OutEdges(TVertex v)
        {
            return _vertexOutEdges[v];
        }

        public bool TryGetInEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            if (_vertexInEdges.TryGetValue(v, out var list))
            {
                edges = list;
                return true;
            }

            edges = null;
            return false;
        }

        public bool TryGetOutEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            if (_vertexOutEdges.TryGetValue(v, out var list))
            {
                edges = list;
                return true;
            }

            edges = null;
            return false;
        }

        public TEdge OutEdge(TVertex v, int index)
        {
            return _vertexOutEdges[v][index];
        }

        public bool IsInEdgesEmpty(TVertex v)
        {
            return _vertexInEdges[v].Count == 0;
        }

        public int InDegree(TVertex v)
        {
            return _vertexInEdges[v].Count;
        }

        public IEnumerable<TEdge> InEdges(TVertex v)
        {
            return _vertexInEdges[v];
        }

        public TEdge InEdge(TVertex v, int index)
        {
            return _vertexInEdges[v][index];
        }

        public int Degree(TVertex v)
        {
            return OutDegree(v) + InDegree(v);
        }

        public bool IsEdgesEmpty
        {
            get { return EdgeCount == 0; }
        }

        public int EdgeCount { get; private set; }

        public virtual IEnumerable<TEdge> Edges
        {
            get
            {
                foreach (var edges in _vertexOutEdges.Values)
                foreach (var edge in edges)
                {
                    yield return edge;
                }
            }
        }

        public bool ContainsEdge(TVertex source, TVertex target)
        {
            if (!TryGetOutEdges(source, out var outEdges))
            {
                return false;
            }

            foreach (var outEdge in outEdges)
            {
                if (outEdge.Target.Equals(target))
                {
                    return true;
                }
            }

            return false;
        }

        public bool TryGetEdge(
            TVertex source,
            TVertex target,
            out TEdge edge)
        {
            if (_vertexOutEdges.TryGetValue(source, out var edgeList) &&
                edgeList.Count > 0)
            {
                foreach (var e in edgeList)
                {
                    if (e.Target.Equals(target))
                    {
                        edge = e;
                        return true;
                    }
                }
            }

            edge = default;
            return false;
        }

        public bool TryGetEdges(
            TVertex source,
            TVertex target,
            out IEnumerable<TEdge> edges)
        {
            if (_vertexOutEdges.TryGetValue(source, out var edgeList))
            {
                var list = new List<TEdge>(edgeList.Count);
                foreach (var edge in edgeList)
                {
                    if (edge.Target.Equals(target))
                    {
                        list.Add(edge);
                    }
                }

                edges = list;
                return true;
            }

            edges = null;
            return false;
        }

        public bool ContainsEdge(TEdge edge)
        {
            return _vertexOutEdges.TryGetValue(edge.Source, out var outEdges) &&
                   outEdges.Contains(edge);
        }

        public virtual bool AddVertex(TVertex v)
        {
            if (ContainsVertex(v))
            {
                return false;
            }

            if (EdgeCapacity > 0)
            {
                _vertexOutEdges.Add(v, new EdgeList<TVertex, TEdge>(EdgeCapacity));
                _vertexInEdges.Add(v, new EdgeList<TVertex, TEdge>(EdgeCapacity));
            }
            else
            {
                _vertexOutEdges.Add(v, new EdgeList<TVertex, TEdge>());
                _vertexInEdges.Add(v, new EdgeList<TVertex, TEdge>());
            }

            OnVertexAdded(v);
            return true;
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

        public event VertexAction<TVertex> VertexAdded;

        protected virtual void OnVertexAdded(TVertex args)
        {
            var eh = VertexAdded;
            eh?.Invoke(args);
        }

        public virtual bool RemoveVertex(TVertex v)
        {
            if (!ContainsVertex(v))
            {
                return false;
            }

            // collect edges to remove
            var edgesToRemove = new EdgeList<TVertex, TEdge>();
            foreach (var outEdge in OutEdges(v))
            {
                _vertexInEdges[outEdge.Target].Remove(outEdge);
                edgesToRemove.Add(outEdge);
            }

            foreach (var inEdge in InEdges(v))
            {
                // might already have been removed
                if (_vertexOutEdges[inEdge.Source].Remove(inEdge))
                {
                    edgesToRemove.Add(inEdge);
                }
            }

            // notify users
            if (EdgeRemoved != null)
            {
                foreach (var edge in edgesToRemove)
                {
                    OnEdgeRemoved(edge);
                }
            }

            _vertexOutEdges.Remove(v);
            _vertexInEdges.Remove(v);
            EdgeCount -= edgesToRemove.Count;
            OnVertexRemoved(v);

            return true;
        }

        public event VertexAction<TVertex> VertexRemoved;

        protected virtual void OnVertexRemoved(TVertex args)
        {
            var eh = VertexRemoved;
            eh?.Invoke(args);
        }

        public int RemoveVertexIf(VertexPredicate<TVertex> predicate)
        {
            var vertices = new VertexList<TVertex>();
            foreach (var v in Vertices)
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

        public virtual bool AddEdge(TEdge e)
        {
            if (!AllowParallelEdges)
            {
                if (ContainsEdge(e.Source, e.Target))
                {
                    return false;
                }
            }

            _vertexOutEdges[e.Source].Add(e);
            _vertexInEdges[e.Target].Add(e);
            EdgeCount++;

            OnEdgeAdded(e);

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

        public event EdgeAction<TVertex, TEdge> EdgeAdded;

        protected virtual void OnEdgeAdded(TEdge args)
        {
            var eh = EdgeAdded;
            eh?.Invoke(args);
        }

        public virtual bool RemoveEdge(TEdge e)
        {
            if (_vertexOutEdges[e.Source].Remove(e))
            {
                _vertexInEdges[e.Target].Remove(e);
                EdgeCount--;
                Contract.Assert(EdgeCount >= 0);

                OnEdgeRemoved(e);
                return true;
            }

            return false;
        }

        public event EdgeAction<TVertex, TEdge> EdgeRemoved;

        protected virtual void OnEdgeRemoved(TEdge args)
        {
            var eh = EdgeRemoved;
            eh?.Invoke(args);
        }

        public int RemoveEdgeIf(EdgePredicate<TVertex, TEdge> predicate)
        {
            var edges = new EdgeList<TVertex, TEdge>();
            foreach (var edge in Edges)
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
            var edges = new EdgeList<TVertex, TEdge>();
            foreach (var edge in OutEdges(v))
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

        public int RemoveInEdgeIf(TVertex v, EdgePredicate<TVertex, TEdge> predicate)
        {
            var edges = new EdgeList<TVertex, TEdge>();
            foreach (var edge in InEdges(v))
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

        public void ClearOutEdges(TVertex v)
        {
            var outEdges = _vertexOutEdges[v];
            foreach (var edge in outEdges)
            {
                _vertexInEdges[edge.Target].Remove(edge);
                OnEdgeRemoved(edge);
            }

            EdgeCount -= outEdges.Count;
            outEdges.Clear();
        }

        #if DEEP_INVARIANT
        [ContractInvariantMethod]
        void ObjectInvariant()
        {
            Contract.Invariant(this.edgeCount >= 0);
            Contract.Invariant(Enumerable.Sum(this.vertexInEdges.Values, ie => ie.Count) == this.edgeCount);
            Contract.Invariant(this.vertexInEdges.Count == this.vertexOutEdges.Count);
            Contract.Invariant(Enumerable.All(this.vertexInEdges, kv => this.vertexOutEdges.ContainsKey(kv.Key)));
            Contract.Invariant(Enumerable.All(this.vertexOutEdges, kv => this.vertexInEdges.ContainsKey(kv.Key)));
        }
        #endif

        public void ClearInEdges(TVertex v)
        {
            var inEdges = _vertexInEdges[v];
            foreach (var edge in inEdges)
            {
                _vertexOutEdges[edge.Source].Remove(edge);
                OnEdgeRemoved(edge);
            }

            EdgeCount -= inEdges.Count;
            inEdges.Clear();
        }

        public void ClearEdges(TVertex v)
        {
            ClearOutEdges(v);
            ClearInEdges(v);
        }

        public void TrimEdgeExcess()
        {
            foreach (var edges in _vertexInEdges.Values)
            {
                edges.TrimExcess();
            }

            foreach (var edges in _vertexOutEdges.Values)
            {
                edges.TrimExcess();
            }
        }

        public void Clear()
        {
            _vertexOutEdges.Clear();
            _vertexInEdges.Clear();
            EdgeCount = 0;
            OnCleared(EventArgs.Empty);
        }

        public event EventHandler Cleared;

        private void OnCleared(EventArgs e)
        {
            var eh = Cleared;
            eh?.Invoke(this, e);
        }

        public void MergeVertex(TVertex v, EdgeFactory<TVertex, TEdge> edgeFactory)
        {
            // storing edges in local array
            var inEdges = _vertexInEdges[v];
            var outEdges = _vertexOutEdges[v];

            // remove vertex
            RemoveVertex(v);

            // add edges from each source to each target
            foreach (var source in inEdges)
            {
                //is it a self edge
                if (source.Source.Equals(v))
                {
                    continue;
                }

                foreach (var target in outEdges)
                {
                    if (v.Equals(target.Target))
                    {
                        continue;
                    }

                    // we add an new edge
                    AddEdge(edgeFactory(source.Source, target.Target));
                }
            }
        }

        public void MergeVertexIf(VertexPredicate<TVertex> vertexPredicate, EdgeFactory<TVertex, TEdge> edgeFactory)
        {
            // storing vertices to merge
            var mergeVertices = new VertexList<TVertex>(VertexCount / 4);
            foreach (var v in Vertices)
            {
                if (vertexPredicate(v))
                {
                    mergeVertices.Add(v);
                }
            }

            // applying merge recursively
            foreach (var v in mergeVertices)
            {
                MergeVertex(v, edgeFactory);
            }
        }

        #region ICloneable Members

        private BidirectionalGraph(
            IVertexEdgeDictionary<TVertex, TEdge> vertexInEdges,
            IVertexEdgeDictionary<TVertex, TEdge> vertexOutEdges,
            int edgeCount,
            int edgeCapacity,
            bool allowParallelEdges
        )
        {
            _vertexInEdges = vertexInEdges;
            _vertexOutEdges = vertexOutEdges;
            EdgeCount = edgeCount;
            _edgeCapacity = edgeCapacity;
            AllowParallelEdges = allowParallelEdges;
        }

        public BidirectionalGraph<TVertex, TEdge> Clone()
        {
            return new BidirectionalGraph<TVertex, TEdge>(
                _vertexInEdges.Clone(),
                _vertexOutEdges.Clone(),
                EdgeCount,
                _edgeCapacity,
                AllowParallelEdges
            );
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion
    }
}