using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PS.Graph
{
    /// <summary>
    ///     An immutable undirected graph data structure based on arrays.
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    [Serializable]
    [DebuggerDisplay("VertexCount = {VertexCount}, EdgeCount = {EdgeCount}")]
    public sealed class ArrayUndirectedGraph<TVertex, TEdge> : IUndirectedGraph<TVertex, TEdge>,
                                                               ICloneable
        where TEdge : IEdge<TVertex>
    {
        private readonly Dictionary<TVertex, TEdge[]> _vertexEdges;

        #region Constructors

        public ArrayUndirectedGraph(
            IUndirectedGraph<TVertex, TEdge> graph)
        {
            EdgeEqualityComparer = graph.EdgeEqualityComparer;
            EdgeCount = graph.EdgeCount;
            _vertexEdges = new Dictionary<TVertex, TEdge[]>(graph.VertexCount);
            foreach (var v in graph.Vertices)
            {
                var edges = graph.AdjacentEdges(v).ToArray();
                _vertexEdges.Add(v, edges);
            }
        }

        #endregion

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return this;
        }

        #endregion

        #region IUndirectedGraph<TVertex,TEdge> Members

        public EdgeEqualityComparer<TVertex, TEdge> EdgeEqualityComparer { get; }

        public IEnumerable<TEdge> AdjacentEdges(TVertex v)
        {
            var edges = _vertexEdges[v];
            return edges ?? Enumerable.Empty<TEdge>();
        }

        public int AdjacentDegree(TVertex v)
        {
            var edges = _vertexEdges[v];
            return edges?.Length ?? 0;
        }

        public bool IsAdjacentEdgesEmpty(TVertex v)
        {
            return _vertexEdges[v] != null;
        }

        public TEdge AdjacentEdge(TVertex v, int index)
        {
            return _vertexEdges[v][index];
        }

        public bool TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            var edges = _vertexEdges[source];
            if (edges != null)
            {
                foreach (var e in edges)
                {
                    if (EdgeEqualityComparer(e, source, target))
                    {
                        edge = e;
                        return true;
                    }
                }
            }

            edge = default;
            return false;
        }

        public bool ContainsEdge(TVertex source, TVertex target)
        {
            return TryGetEdge(source, target, out _);
        }

        public bool ContainsVertex(TVertex vertex)
        {
            return _vertexEdges.ContainsKey(vertex);
        }

        public bool IsDirected
        {
            get { return false; }
        }

        public bool AllowParallelEdges
        {
            get { return true; }
        }

        public bool IsEdgesEmpty
        {
            get { return EdgeCount > 0; }
        }

        public int EdgeCount { get; }

        public IEnumerable<TEdge> Edges
        {
            get
            {
                foreach (var edges in _vertexEdges.Values)
                {
                    if (edges == null) continue;

                    foreach (var e in edges)
                    {
                        yield return e;
                    }
                }
            }
        }

        public bool ContainsEdge(TEdge edge)
        {
            var source = edge.Source;
            if (_vertexEdges.TryGetValue(source, out var edges))
            {
                foreach (var e in edges)
                {
                    if (e.Equals(edge))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsVerticesEmpty
        {
            get { return _vertexEdges.Count == 0; }
        }

        public int VertexCount
        {
            get { return _vertexEdges.Count; }
        }

        public IEnumerable<TVertex> Vertices
        {
            get { return _vertexEdges.Keys; }
        }

        #endregion

        #region Members

        /// <summary>
        ///     Returns self
        /// </summary>
        /// <returns></returns>
        public ArrayUndirectedGraph<TVertex, TEdge> Clone()
        {
            return this;
        }

        #endregion
    }
}