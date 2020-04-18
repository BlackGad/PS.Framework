using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using PS.Graph.Collections;

namespace PS.Graph
{
    public delegate bool EdgeEqualityComparer<in TVertex, in TEdge>(TEdge edge, TVertex source, TVertex target)
        where TEdge : IEdge<TVertex>;

    [Serializable]
    [DebuggerDisplay("VertexCount = {VertexCount}, EdgeCount = {EdgeCount}")]
    public class UndirectedGraph<TVertex, TEdge> : IMutableUndirectedGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private readonly VertexEdgeDictionary<TVertex, TEdge> _adjacentEdges;
        private readonly EdgeEqualityComparer<TVertex, TEdge> _edgeEqualityComparer;
        private int _edgeCapacity = 4;

        #region Constructors

        public UndirectedGraph()
            : this(true)
        {
        }

        public UndirectedGraph(bool allowParallelEdges)
            : this(allowParallelEdges, EdgeExtensions.GetUndirectedVertexEquality<TVertex, TEdge>())
        {
            AllowParallelEdges = allowParallelEdges;
        }

        public UndirectedGraph(bool allowParallelEdges, EdgeEqualityComparer<TVertex, TEdge> edgeEqualityComparer)
            : this(allowParallelEdges, edgeEqualityComparer, -1)
        {
        }

        public UndirectedGraph(bool allowParallelEdges, EdgeEqualityComparer<TVertex, TEdge> edgeEqualityComparer, int vertexCapacity)
            : this(allowParallelEdges, edgeEqualityComparer, vertexCapacity, EqualityComparer<TVertex>.Default)
        {
        }

        public UndirectedGraph(bool allowParallelEdges,
                               EdgeEqualityComparer<TVertex, TEdge> edgeEqualityComparer,
                               int vertexCapacity,
                               IEqualityComparer<TVertex> vertexComparer)
        {
            AllowParallelEdges = allowParallelEdges;
            _edgeEqualityComparer = edgeEqualityComparer;
            if (vertexCapacity > -1)
            {
                _adjacentEdges = new VertexEdgeDictionary<TVertex, TEdge>(vertexCapacity, vertexComparer);
            }
            else
            {
                _adjacentEdges = new VertexEdgeDictionary<TVertex, TEdge>(vertexComparer);
            }
        }

        #endregion

        #region Properties

        public int EdgeCapacity
        {
            get { return _edgeCapacity; }
            set { _edgeCapacity = value; }
        }

        #endregion

        #region IMutableUndirectedGraph<TVertex,TEdge> Members

        public EdgeEqualityComparer<TVertex, TEdge> EdgeEqualityComparer
        {
            get
            {
                Contract.Ensures(Contract.Result<EdgeEqualityComparer<TVertex, TEdge>>() != null);
                return _edgeEqualityComparer;
            }
        }

        public bool IsDirected
        {
            get { return false; }
        }

        public bool AllowParallelEdges { get; }

        public event VertexAction<TVertex> VertexAdded;

        public int AddVertexRange(IEnumerable<TVertex> vertices)
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

        public bool AddVertex(TVertex v)
        {
            if (ContainsVertex(v))
            {
                return false;
            }

            var edges = EdgeCapacity < 0
                ? new EdgeList<TVertex, TEdge>()
                : new EdgeList<TVertex, TEdge>(EdgeCapacity);
            _adjacentEdges.Add(v, edges);
            OnVertexAdded(v);
            return true;
        }

        public event VertexAction<TVertex> VertexRemoved;

        public bool RemoveVertex(TVertex v)
        {
            ClearAdjacentEdges(v);
            var result = _adjacentEdges.Remove(v);

            if (result)
            {
                OnVertexRemoved(v);
            }

            return result;
        }

        public int RemoveVertexIf(VertexPredicate<TVertex> predicate)
        {
            var vertices = new List<TVertex>();
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

        public int RemoveAdjacentEdgeIf(TVertex v, EdgePredicate<TVertex, TEdge> predicate)
        {
            var outEdges = _adjacentEdges[v];
            var edges = new List<TEdge>(outEdges.Count);
            foreach (var edge in outEdges)
            {
                if (predicate(edge))
                {
                    edges.Add(edge);
                }
            }

            RemoveEdges(edges);
            return edges.Count;
        }

        public void ClearAdjacentEdges(TVertex v)
        {
            var edges = _adjacentEdges[v].Clone();
            EdgeCount -= edges.Count;

            foreach (var edge in edges)
            {
                if (_adjacentEdges.TryGetValue(edge.Target, out var aEdges))
                {
                    aEdges.Remove(edge);
                }

                if (_adjacentEdges.TryGetValue(edge.Source, out aEdges))
                {
                    aEdges.Remove(edge);
                }
            }
        }

        public void Clear()
        {
            _adjacentEdges.Clear();
            EdgeCount = 0;
            OnCleared(EventArgs.Empty);
        }

        public event EventHandler Cleared;

        public bool TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            foreach (var e in AdjacentEdges(source))
            {
                if (_edgeEqualityComparer(e, source, target))
                {
                    edge = e;
                    return true;
                }
            }

            edge = default;
            return false;
        }

        public bool ContainsEdge(TVertex source, TVertex target)
        {
            return TryGetEdge(source, target, out _);
        }

        public TEdge AdjacentEdge(TVertex v, int index)
        {
            return _adjacentEdges[v][index];
        }

        public bool IsVerticesEmpty
        {
            get { return _adjacentEdges.Count == 0; }
        }

        public int VertexCount
        {
            get { return _adjacentEdges.Count; }
        }

        public IEnumerable<TVertex> Vertices
        {
            get { return _adjacentEdges.Keys; }
        }

        [Pure]
        public bool ContainsVertex(TVertex vertex)
        {
            return _adjacentEdges.ContainsKey(vertex);
        }

        public bool AddVerticesAndEdge(TEdge edge)
        {
            var sourceEdges = AddAndReturnEdges(edge.Source);
            var targetEdges = AddAndReturnEdges(edge.Target);

            if (!AllowParallelEdges)
            {
                if (ContainsEdgeBetweenVertices(sourceEdges, edge))
                {
                    return false;
                }
            }

            sourceEdges.Add(edge);
            if (!edge.IsSelfEdge<TVertex, TEdge>())
            {
                targetEdges.Add(edge);
            }

            EdgeCount++;
            OnEdgeAdded(edge);

            return true;
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

        public bool AddEdge(TEdge edge)
        {
            var sourceEdges = _adjacentEdges[edge.Source];
            if (!AllowParallelEdges)
            {
                if (ContainsEdgeBetweenVertices(sourceEdges, edge))
                {
                    return false;
                }
            }

            sourceEdges.Add(edge);
            if (!edge.IsSelfEdge<TVertex, TEdge>())
            {
                var targetEdges = _adjacentEdges[edge.Target];
                targetEdges.Add(edge);
            }

            EdgeCount++;
            OnEdgeAdded(edge);

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

        public bool RemoveEdge(TEdge edge)
        {
            var removed = _adjacentEdges[edge.Source].Remove(edge);
            if (removed)
            {
                if (!edge.IsSelfEdge<TVertex, TEdge>())
                {
                    _adjacentEdges[edge.Target].Remove(edge);
                }

                EdgeCount--;
                Contract.Assert(EdgeCount >= 0);
                OnEdgeRemoved(edge);
                return true;
            }

            return false;
        }

        public event EdgeAction<TVertex, TEdge> EdgeRemoved;

        public int RemoveEdgeIf(EdgePredicate<TVertex, TEdge> predicate)
        {
            var edges = new List<TEdge>();
            foreach (var edge in Edges)
            {
                if (predicate(edge))
                {
                    edges.Add(edge);
                }
            }

            return RemoveEdges(edges);
        }

        public bool IsEdgesEmpty
        {
            get { return EdgeCount == 0; }
        }

        public int EdgeCount { get; private set; }

        public IEnumerable<TEdge> Edges
        {
            get
            {
                var edgeColors = new Dictionary<TEdge, GraphColor>(EdgeCount);
                foreach (var edges in _adjacentEdges.Values)
                {
                    foreach (var edge in edges)
                    {
                        if (edgeColors.TryGetValue(edge, out _))
                        {
                            continue;
                        }

                        edgeColors.Add(edge, GraphColor.Black);
                        yield return edge;
                    }
                }
            }
        }

        public bool ContainsEdge(TEdge edge)
        {
            foreach (var e in AdjacentEdges(edge.Source))
            {
                if (e.Equals(edge))
                {
                    return true;
                }
            }

            return false;
        }

        public IEnumerable<TEdge> AdjacentEdges(TVertex v)
        {
            return _adjacentEdges[v];
        }

        public int AdjacentDegree(TVertex v)
        {
            return _adjacentEdges[v].Count;
        }

        public bool IsAdjacentEdgesEmpty(TVertex v)
        {
            return _adjacentEdges[v].Count == 0;
        }

        #endregion

        #region Members

        public int RemoveEdges(IEnumerable<TEdge> edges)
        {
            var count = 0;
            foreach (var edge in edges)
            {
                if (RemoveEdge(edge))
                {
                    count++;
                }
            }

            return count;
        }

        public void TrimEdgeExcess()
        {
            foreach (var edges in _adjacentEdges.Values)
            {
                edges.TrimExcess();
            }
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

        private IEdgeList<TVertex, TEdge> AddAndReturnEdges(TVertex v)
        {
            if (!_adjacentEdges.TryGetValue(v, out var edges))
            {
                _adjacentEdges[v] = edges = EdgeCapacity < 0
                    ? new EdgeList<TVertex, TEdge>()
                    : new EdgeList<TVertex, TEdge>(EdgeCapacity);
            }

            return edges;
        }

        private bool ContainsEdgeBetweenVertices(IEnumerable<TEdge> edges, TEdge edge)
        {
            var source = edge.Source;
            var target = edge.Target;
            foreach (var e in edges)
            {
                if (EdgeEqualityComparer(e, source, target))
                {
                    return true;
                }
            }

            return false;
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