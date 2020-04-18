using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using PS.Graph.Collections;

namespace PS.Graph
{
    /// <summary>
    ///     A mutable directed graph data structure efficient for sparse
    ///     graph representation where out-edge need to be enumerated only.
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    [Serializable]
    [DebuggerDisplay("VertexCount = {VertexCount}, EdgeCount = {EdgeCount}")]
    public class AdjacencyGraph<TVertex, TEdge> : IEdgeListAndIncidenceGraph<TVertex, TEdge>,
                                                  IMutableVertexAndEdgeListGraph<TVertex, TEdge>,
                                                  ICloneable
        where TEdge : IEdge<TVertex>
    {
        #region Static members

        public static Type EdgeType
        {
            get { return typeof(TEdge); }
        }

        #endregion

        private readonly IVertexEdgeDictionary<TVertex, TEdge> _vertexEdges;
        private int _edgeCapacity;

        #region Constructors

        public AdjacencyGraph()
            : this(true)
        {
        }

        public AdjacencyGraph(bool allowParallelEdges)
            : this(allowParallelEdges, -1)
        {
        }

        public AdjacencyGraph(bool allowParallelEdges, int vertexCapacity)
            : this(allowParallelEdges, vertexCapacity, -1)
        {
        }

        public AdjacencyGraph(bool allowParallelEdges, int vertexCapacity, int edgeCapacity)
            : this(allowParallelEdges, vertexCapacity, edgeCapacity, EqualityComparer<TVertex>.Default)
        {
        }

        public AdjacencyGraph(bool allowParallelEdges, int vertexCapacity, int edgeCapacity, IEqualityComparer<TVertex> vertexComparer)
        {
            AllowParallelEdges = allowParallelEdges;
            if (vertexCapacity > -1)
            {
                _vertexEdges = new VertexEdgeDictionary<TVertex, TEdge>(vertexCapacity, vertexComparer);
            }
            else
            {
                _vertexEdges = new VertexEdgeDictionary<TVertex, TEdge>(vertexComparer);
            }

            _edgeCapacity = edgeCapacity;
        }

        public AdjacencyGraph(
            bool allowParallelEdges,
            int capacity,
            int edgeCapacity,
            Func<int, IVertexEdgeDictionary<TVertex, TEdge>> vertexEdgesDictionaryFactory)
        {
            AllowParallelEdges = allowParallelEdges;
            _vertexEdges = vertexEdgesDictionaryFactory(capacity);
            _edgeCapacity = edgeCapacity;
        }

        private AdjacencyGraph(
            IVertexEdgeDictionary<TVertex, TEdge> vertexEdges,
            int edgeCount,
            int edgeCapacity,
            bool allowParallelEdges
        )
        {
            _vertexEdges = vertexEdges;
            EdgeCount = edgeCount;
            _edgeCapacity = edgeCapacity;
            AllowParallelEdges = allowParallelEdges;
        }

        #endregion

        #region Properties

        public int EdgeCapacity
        {
            get { return _edgeCapacity; }
            set { _edgeCapacity = value; }
        }

        #endregion

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        #region IEdgeListAndIncidenceGraph<TVertex,TEdge> Members

        public bool IsDirected { get; } = true;

        public bool AllowParallelEdges { [Pure] get; }

        public bool IsVerticesEmpty
        {
            get { return _vertexEdges.Count == 0; }
        }

        public int VertexCount
        {
            get { return _vertexEdges.Count; }
        }

        public virtual IEnumerable<TVertex> Vertices
        {
            get { return _vertexEdges.Keys; }
        }

        [Pure]
        public bool ContainsVertex(TVertex v)
        {
            return _vertexEdges.ContainsKey(v);
        }

        public bool IsOutEdgesEmpty(TVertex v)
        {
            return _vertexEdges[v].Count == 0;
        }

        public int OutDegree(TVertex v)
        {
            return _vertexEdges[v].Count;
        }

        public virtual IEnumerable<TEdge> OutEdges(TVertex v)
        {
            return _vertexEdges[v];
        }

        public virtual bool TryGetOutEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            if (_vertexEdges.TryGetValue(v, out var list))
            {
                edges = list;
                return true;
            }

            edges = null;
            return false;
        }

        public TEdge OutEdge(TVertex v, int index)
        {
            return _vertexEdges[v][index];
        }

        /// <summary>
        ///     Gets a value indicating whether this instance is edges empty.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is edges empty; otherwise, <c>false</c>.
        /// </value>
        public bool IsEdgesEmpty
        {
            [Pure] get { return EdgeCount == 0; }
        }

        /// <summary>
        ///     Gets the edge count.
        /// </summary>
        /// <value>The edge count.</value>
        public int EdgeCount { get; private set; }

        /// <summary>
        ///     Gets the edges.
        /// </summary>
        /// <value>The edges.</value>
        public virtual IEnumerable<TEdge> Edges
        {
            [Pure]
            get
            {
                foreach (var edges in _vertexEdges.Values)
                foreach (var edge in edges)
                {
                    yield return edge;
                }
            }
        }

        [Pure]
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

        [Pure]
        public bool ContainsEdge(TEdge edge)
        {
            return
                _vertexEdges.TryGetValue(edge.Source, out var edges) &&
                edges.Contains(edge);
        }

        [Pure]
        public bool TryGetEdge(
            TVertex source,
            TVertex target,
            out TEdge edge)
        {
            if (_vertexEdges.TryGetValue(source, out var edgeList) &&
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

        [Pure]
        public virtual bool TryGetEdges(
            TVertex source,
            TVertex target,
            out IEnumerable<TEdge> edges)
        {
            if (_vertexEdges.TryGetValue(source, out var outEdges))
            {
                var list = new List<TEdge>(outEdges.Count);
                foreach (var edge in outEdges)
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

        #endregion

        #region IMutableVertexAndEdgeListGraph<TVertex,TEdge> Members

        /// <summary>
        ///     Adds the edge to the graph
        /// </summary>
        /// <param name="e">the edge to add</param>
        /// <returns>true if the edge was added; false if it was already part of the graph</returns>
        public virtual bool AddEdge(TEdge e)
        {
            if (!AllowParallelEdges)
            {
                if (ContainsEdge(e.Source, e.Target))
                {
                    return false;
                }
            }

            _vertexEdges[e.Source].Add(e);
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

        public event EdgeAction<TVertex, TEdge> EdgeAdded;

        public virtual bool RemoveEdge(TEdge e)
        {
            if (_vertexEdges.TryGetValue(e.Source, out var edges) &&
                edges.Remove(e))
            {
                EdgeCount--;
                Contract.Assert(EdgeCount >= 0);
                OnEdgeRemoved(e);
                return true;
            }

            return false;
        }

        public event EdgeAction<TVertex, TEdge> EdgeRemoved;

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

        public void Clear()
        {
            _vertexEdges.Clear();
            EdgeCount = 0;
            OnCleared(EventArgs.Empty);
        }

        public event EventHandler Cleared;

        public void ClearOutEdges(TVertex v)
        {
            var edges = _vertexEdges[v];
            var count = edges.Count;
            if (EdgeRemoved != null) // call only if someone is listening
            {
                foreach (var edge in edges)
                {
                    OnEdgeRemoved(edge);
                }
            }

            edges.Clear();
            EdgeCount -= count;
        }

        public int RemoveOutEdgeIf(TVertex v, EdgePredicate<TVertex, TEdge> predicate)
        {
            var edges = _vertexEdges[v];
            var edgeToRemove = new EdgeList<TVertex, TEdge>(edges.Count);
            foreach (var edge in edges)
            {
                if (predicate(edge))
                {
                    edgeToRemove.Add(edge);
                }
            }

            foreach (var edge in edgeToRemove)
            {
                edges.Remove(edge);
                OnEdgeRemoved(edge);
            }

            EdgeCount -= edgeToRemove.Count;

            return edgeToRemove.Count;
        }

        public void TrimEdgeExcess()
        {
            foreach (var edges in _vertexEdges.Values)
            {
                edges.TrimExcess();
            }
        }

        public virtual bool AddVerticesAndEdge(TEdge e)
        {
            AddVertex(e.Source);
            AddVertex(e.Target);
            return AddEdge(e);
        }

        /// <summary>
        ///     Adds a range of edges to the graph
        /// </summary>
        /// <param name="edges"></param>
        /// <returns>the count edges that were added</returns>
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

        public virtual bool AddVertex(TVertex v)
        {
            if (ContainsVertex(v))
            {
                return false;
            }

            if (EdgeCapacity > 0)
            {
                _vertexEdges.Add(v, new EdgeList<TVertex, TEdge>(EdgeCapacity));
            }
            else
            {
                _vertexEdges.Add(v, new EdgeList<TVertex, TEdge>());
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

        public virtual bool RemoveVertex(TVertex v)
        {
            if (!ContainsVertex(v))
            {
                return false;
            }

            // remove outEdges
            {
                var edges = _vertexEdges[v];
                if (EdgeRemoved != null) // lazily notify
                {
                    foreach (var edge in edges)
                    {
                        OnEdgeRemoved(edge);
                    }
                }

                EdgeCount -= edges.Count;
                edges.Clear();
            }

            // iterate over edges and remove each edge touching the vertex
            var edgeToRemove = new EdgeList<TVertex, TEdge>();
            foreach (var kv in _vertexEdges)
            {
                if (kv.Key.Equals(v)) continue; // we've already 
                // collect edge to remove
                foreach (var edge in kv.Value)
                {
                    if (edge.Target.Equals(v))
                    {
                        edgeToRemove.Add(edge);
                    }
                }

                // remove edges
                foreach (var edge in edgeToRemove)
                {
                    kv.Value.Remove(edge);
                    OnEdgeRemoved(edge);
                }

                // update count
                EdgeCount -= edgeToRemove.Count;
                edgeToRemove.Clear();
            }

            Contract.Assert(EdgeCount >= 0);
            _vertexEdges.Remove(v);
            OnVertexRemoved(v);

            return true;
        }

        public event VertexAction<TVertex> VertexRemoved;

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

        #endregion

        #region Members

        [Pure]
        public AdjacencyGraph<TVertex, TEdge> Clone()
        {
            return new AdjacencyGraph<TVertex, TEdge>(
                _vertexEdges.Clone(),
                EdgeCount,
                _edgeCapacity,
                AllowParallelEdges
            );
        }

        protected virtual void OnEdgeAdded(TEdge args)
        {
            var eh = EdgeAdded;
            eh?.Invoke(args);
        }

        protected virtual void OnEdgeRemoved(TEdge args)
        {
            var eh = EdgeRemoved;
            eh?.Invoke(args);
        }

        protected virtual void OnVertexAdded(TVertex args)
        {
            var eh = VertexAdded;
            eh?.Invoke(args);
        }

        protected virtual void OnVertexRemoved(TVertex args)
        {
            var eh = VertexRemoved;
            eh?.Invoke(args);
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(EdgeCount >= 0);
        }

        private void OnCleared(EventArgs e)
        {
            var eh = Cleared;
            eh?.Invoke(this, e);
        }

        #endregion
    }
}