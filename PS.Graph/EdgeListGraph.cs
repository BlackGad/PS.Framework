using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using PS.Graph.Collections;

namespace PS.Graph
{
    [Serializable]
    [DebuggerDisplay("EdgeCount = {" + nameof(EdgeCount) + "}")]
    public class EdgeListGraph<TVertex, TEdge> : IMutableEdgeListGraph<TVertex, TEdge>,
                                                 ICloneable
        where TEdge : IEdge<TVertex>
    {
        private readonly EdgeEdgeDictionary<TVertex, TEdge> _edges = new EdgeEdgeDictionary<TVertex, TEdge>();

        #region Constructors

        public EdgeListGraph()
        {
        }

        public EdgeListGraph(bool isDirected, bool allowParallelEdges)
        {
            IsDirected = isDirected;
            AllowParallelEdges = allowParallelEdges;
        }

        private EdgeListGraph(
            bool isDirected,
            bool allowParallelEdges,
            EdgeEdgeDictionary<TVertex, TEdge> edges)
        {
            IsDirected = isDirected;
            AllowParallelEdges = allowParallelEdges;
            _edges = edges;
        }

        #endregion

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        #region IMutableEdgeListGraph<TVertex,TEdge> Members

        public bool IsEdgesEmpty
        {
            get { return _edges.Count == 0; }
        }

        public int EdgeCount
        {
            get { return _edges.Count; }
        }

        public IEnumerable<TEdge> Edges
        {
            get { return _edges.Keys; }
        }

        [Pure]
        public bool ContainsEdge(TEdge edge)
        {
            return _edges.ContainsKey(edge);
        }

        public bool IsDirected { get; } = true;

        public bool AllowParallelEdges { get; } = true;

        [Pure]
        public bool IsVerticesEmpty
        {
            get { return _edges.Count == 0; }
        }

        [Pure]
        public int VertexCount
        {
            get { return GetVertexCounts().Count; }
        }

        [Pure]
        public IEnumerable<TVertex> Vertices
        {
            get { return GetVertexCounts().Keys; }
        }

        [Pure]
        public bool ContainsVertex(TVertex vertex)
        {
            foreach (var e in Edges)
            {
                if (e.Source.Equals(vertex) ||
                    e.Target.Equals(vertex))
                {
                    return true;
                }
            }

            return false;
        }

        public bool AddEdge(TEdge edge)
        {
            if (ContainsEdge(edge))
            {
                return false;
            }

            _edges.Add(edge, edge);
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
            if (_edges.Remove(edge))
            {
                OnEdgeRemoved(edge);
                return true;
            }

            return false;
        }

        public event EdgeAction<TVertex, TEdge> EdgeRemoved;

        public int RemoveEdgeIf(EdgePredicate<TVertex, TEdge> predicate)
        {
            var edgesToRemove = new List<TEdge>();
            foreach (var edge in Edges)
            {
                if (predicate(edge))
                {
                    edgesToRemove.Add(edge);
                }
            }

            foreach (var edge in edgesToRemove)
            {
                _edges.Remove(edge);
            }

            return edgesToRemove.Count;
        }

        public void Clear()
        {
            var edges = _edges.Clone();
            _edges.Clear();
            foreach (var edge in edges.Keys)
            {
                OnEdgeRemoved(edge);
            }

            OnCleared(EventArgs.Empty);
        }

        public event EventHandler Cleared;

        #endregion

        #region Members

        public bool AddVerticesAndEdge(TEdge edge)
        {
            return AddEdge(edge);
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

        public EdgeListGraph<TVertex, TEdge> Clone()
        {
            return new EdgeListGraph<TVertex, TEdge>(
                IsDirected,
                AllowParallelEdges,
                _edges.Clone()
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

        private Dictionary<TVertex, int> GetVertexCounts()
        {
            var vertices = new Dictionary<TVertex, int>(EdgeCount * 2);
            foreach (var e in Edges)
            {
                vertices[e.Source]++;
                vertices[e.Target]++;
            }

            return vertices;
        }

        private void OnCleared(EventArgs e)
        {
            var eh = Cleared;
            eh?.Invoke(this, e);
        }

        #endregion
    }
}