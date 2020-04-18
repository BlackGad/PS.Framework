using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace PS.Graph
{
    [Serializable]
    [DebuggerDisplay("VertexCount = {VertexCount}, EdgeCount = {EdgeCount}")]
    public class BidirectionalMatrixGraph<TEdge> : IBidirectionalGraph<int, TEdge>,
                                                   IMutableEdgeListGraph<int, TEdge>,
                                                   ICloneable
        where TEdge : IEdge<int>
    {
        private readonly TEdge[,] _edges;

        #region Constructors

        public BidirectionalMatrixGraph(int vertexCount)
        {
            Contract.Requires(vertexCount > 0);

            VertexCount = vertexCount;
            EdgeCount = 0;
            _edges = new TEdge[vertexCount, vertexCount];
        }

        private BidirectionalMatrixGraph(
            int vertexCount,
            int edgeCount,
            TEdge[,] edges)
        {
            Contract.Requires(vertexCount > 0);
            Contract.Requires(edgeCount >= 0);
            Contract.Requires(edges != null);
            Contract.Requires(vertexCount == edges.GetLength(0));
            Contract.Requires(vertexCount == edges.GetLength(1));

            VertexCount = vertexCount;
            EdgeCount = edgeCount;
            _edges = edges;
        }

        #endregion

        #region IBidirectionalGraph<int,TEdge> Members

        public bool AllowParallelEdges
        {
            get { return false; }
        }

        public bool IsDirected
        {
            get { return true; }
        }

        public int VertexCount { get; }

        public bool IsVerticesEmpty
        {
            get { return VertexCount == 0; }
        }

        public int EdgeCount { get; private set; }

        public bool IsEdgesEmpty
        {
            get { return EdgeCount == 0; }
        }

        public IEnumerable<TEdge> Edges
        {
            get
            {
                for (var i = 0; i < VertexCount; ++i)
                {
                    for (var j = 0; j < VertexCount; ++j)
                    {
                        var e = _edges[i, j];
                        if (e != null)
                        {
                            yield return e;
                        }
                    }
                }
            }
        }

        [Pure]
        public bool IsInEdgesEmpty(int v)
        {
            for (var i = 0; i < VertexCount; ++i)
            {
                if (_edges[i, v] != null)
                {
                    return false;
                }
            }

            return true;
        }

        [Pure]
        public int InDegree(int v)
        {
            var count = 0;
            for (var i = 0; i < VertexCount; ++i)
            {
                if (_edges[i, v] != null)
                {
                    count++;
                }
            }

            return count;
        }

        [Pure]
        public IEnumerable<TEdge> InEdges(int v)
        {
            for (var i = 0; i < VertexCount; ++i)
            {
                var e = _edges[i, v];
                if (e != null)
                {
                    yield return e;
                }
            }
        }

        [Pure]
        public bool TryGetInEdges(int v, out IEnumerable<TEdge> edges)
        {
            Contract.Ensures(Contract.Result<bool>() == (v > VertexCount));
            Contract.Ensures(Contract.Result<bool>() == (Contract.ValueAtReturn(out edges) != null));

            if (v > -1 && v < VertexCount)
            {
                edges = InEdges(v);
                return true;
            }

            edges = null;
            return false;
        }

        [Pure]
        public TEdge InEdge(int v, int index)
        {
            var count = 0;
            for (var i = 0; i < VertexCount; ++i)
            {
                var e = _edges[i, v];
                if (e != null)
                {
                    if (count == index)
                    {
                        return e;
                    }

                    count++;
                }
            }

            throw new ArgumentOutOfRangeException(nameof(index));
        }

        [Pure]
        public int Degree(int v)
        {
            return InDegree(v) + OutDegree(v);
        }

        public bool ContainsEdge(int source, int target)
        {
            return _edges[source, target] != null;
        }

        public bool TryGetEdge(int source, int target, out TEdge edge)
        {
            edge = _edges[source, target];
            return edge != null;
        }

        public bool TryGetEdges(int source, int target, out IEnumerable<TEdge> edges)
        {
            if (TryGetEdge(source, target, out var edge))
            {
                edges = new[] { edge };
                return true;
            }

            edges = null;
            return false;
        }

        [Pure]
        public bool IsOutEdgesEmpty(int v)
        {
            for (var j = 0; j < VertexCount; ++j)
            {
                if (_edges[v, j] != null)
                {
                    return false;
                }
            }

            return true;
        }

        [Pure]
        public int OutDegree(int v)
        {
            var count = 0;
            for (var j = 0; j < VertexCount; ++j)
            {
                if (_edges[v, j] != null)
                {
                    count++;
                }
            }

            return count;
        }

        [Pure]
        public IEnumerable<TEdge> OutEdges(int v)
        {
            for (var j = 0; j < VertexCount; ++j)
            {
                var e = _edges[v, j];
                if (e != null)
                {
                    yield return e;
                }
            }
        }

        [Pure]
        public bool TryGetOutEdges(int v, out IEnumerable<TEdge> edges)
        {
            if (v > -1 && v < VertexCount)
            {
                edges = OutEdges(v);
                return true;
            }

            edges = null;
            return false;
        }

        [Pure]
        public TEdge OutEdge(int v, int index)
        {
            var count = 0;
            for (var j = 0; j < VertexCount; ++j)
            {
                var e = _edges[v, j];
                if (e != null)
                {
                    if (count == index)
                    {
                        return e;
                    }

                    count++;
                }
            }

            throw new ArgumentOutOfRangeException(nameof(index));
        }

        public IEnumerable<int> Vertices
        {
            get
            {
                for (var i = 0; i < VertexCount; ++i)
                {
                    yield return i;
                }
            }
        }

        [Pure]
        public bool ContainsVertex(int vertex)
        {
            return vertex >= 0 && vertex < VertexCount;
        }

        [Pure]
        public bool ContainsEdge(TEdge edge)
        {
            var e = _edges[edge.Source, edge.Target];
            return e != null &&
                   e.Equals(edge);
        }

        #endregion

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        #region IMutableEdgeListGraph<int,TEdge> Members

        public void Clear()
        {
            for (var i = 0; i < VertexCount; ++i)
            for (var j = 0; j < VertexCount; ++j)
            {
                _edges[i, j] = default;
            }

            EdgeCount = 0;
            OnCleared(EventArgs.Empty);
        }

        public event EventHandler Cleared;

        public bool AddEdge(TEdge edge)
        {
            if (_edges[edge.Source, edge.Target] != null)
            {
                throw new ParallelEdgeNotAllowedException();
            }

            _edges[edge.Source, edge.Target] = edge;
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

        public event EdgeAction<int, TEdge> EdgeAdded;

        public bool RemoveEdge(TEdge edge)
        {
            var e = _edges[edge.Source, edge.Target];
            _edges[edge.Source, edge.Target] = default;
            if (!e.Equals(default(TEdge)))
            {
                EdgeCount--;
                OnEdgeRemoved(edge);
                return true;
            }

            return false;
        }

        public event EdgeAction<int, TEdge> EdgeRemoved;

        public int RemoveEdgeIf(EdgePredicate<int, TEdge> predicate)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Members

        public void ClearEdges(int v)
        {
            Contract.Requires(0 <= v && v < VertexCount);

            ClearInEdges(v);
            ClearOutEdges(v);
        }

        public void ClearInEdges(int v)
        {
            Contract.Requires(0 <= v && v < VertexCount);

            for (var i = 0; i < VertexCount; ++i)
            {
                var e = _edges[i, v];
                if (e != null)
                {
                    RemoveEdge(e);
                }
            }
        }

        public void ClearOutEdges(int v)
        {
            Contract.Requires(0 <= v && v < VertexCount);

            for (var j = 0; j < VertexCount; ++j)
            {
                var e = _edges[v, j];
                if (e != null)
                {
                    RemoveEdge(e);
                }
            }
        }

        public BidirectionalMatrixGraph<TEdge> Clone()
        {
            return new BidirectionalMatrixGraph<TEdge>(
                VertexCount,
                EdgeCount,
                (TEdge[,])_edges.Clone()
            );
        }

        public int RemoveInEdgeIf(int v, EdgePredicate<int, TEdge> edgePredicate)
        {
            Contract.Requires(0 <= v && v < VertexCount);

            var count = 0;
            for (var i = 0; i < VertexCount; ++i)
            {
                var e = _edges[i, v];
                if (e != null && edgePredicate(e))
                {
                    RemoveEdge(e);
                    count++;
                }
            }

            return count;
        }

        public int RemoveOutEdgeIf(int v, EdgePredicate<int, TEdge> predicate)
        {
            Contract.Requires(0 <= v && v < VertexCount);

            var count = 0;
            for (var j = 0; j < VertexCount; ++j)
            {
                var e = _edges[v, j];
                if (e != null && predicate(e))
                {
                    RemoveEdge(e);
                    count++;
                }
            }

            return count;
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

        private void OnCleared(EventArgs e)
        {
            var eh = Cleared;
            eh?.Invoke(this, e);
        }

        #endregion
    }
}