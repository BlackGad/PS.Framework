using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

namespace PS.Graph
{
    /// <summary>
    ///     An immutable directed graph data structure efficient for large sparse
    ///     graph representation where out-edge need to be enumerated only.
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    [Serializable]
    [DebuggerDisplay("VertexCount = {VertexCount}, EdgeCount = {EdgeCount}")]
    public sealed class ArrayAdjacencyGraph<TVertex, TEdge> : IVertexAndEdgeListGraph<TVertex, TEdge>,
                                                              ICloneable
        where TEdge : IEdge<TVertex>
    {
        private readonly Dictionary<TVertex, TEdge[]> _vertexOutEdges;

        #region Constructors

        public ArrayAdjacencyGraph(
            IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph
        )
        {
            Contract.Requires(visitedGraph != null);
            _vertexOutEdges = new Dictionary<TVertex, TEdge[]>(visitedGraph.VertexCount);
            EdgeCount = visitedGraph.EdgeCount;
            foreach (var vertex in visitedGraph.Vertices)
            {
                var outEdges = new List<TEdge>(visitedGraph.OutEdges(vertex));
                _vertexOutEdges.Add(vertex, outEdges.ToArray());
            }
        }

        private ArrayAdjacencyGraph(
            Dictionary<TVertex, TEdge[]> vertexOutEdges,
            int edgeCount
        )
        {
            Contract.Requires(vertexOutEdges != null);
            Contract.Requires(edgeCount >= 0);
            Contract.Requires(edgeCount == vertexOutEdges.Sum(kv => kv.Value?.Length ?? 0));

            _vertexOutEdges = vertexOutEdges;
            EdgeCount = edgeCount;
        }

        #endregion

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        #region IVertexAndEdgeListGraph<TVertex,TEdge> Members

        public bool ContainsEdge(TVertex source, TVertex target)
        {
            return TryGetEdge(source, target, out _);
        }

        public bool TryGetEdges(TVertex source, TVertex target, out IEnumerable<TEdge> edges)
        {
            if (_vertexOutEdges.TryGetValue(source, out var es))
            {
                List<TEdge> edges1 = null;
                for (var i = 0; i < es.Length; i++)
                {
                    if (es[i].Target.Equals(target))
                    {
                        if (edges1 == null)
                        {
                            edges1 = new List<TEdge>(es.Length - i);
                        }

                        edges1.Add(es[i]);
                    }
                }

                edges = edges1;
                return edges != null;
            }

            edges = null;
            return false;
        }

        public bool TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            if (_vertexOutEdges.TryGetValue(source, out var edges) &&
                edges != null)
            {
                foreach (var e in edges)
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

        public bool IsOutEdgesEmpty(TVertex v)
        {
            return OutDegree(v) == 0;
        }

        public int OutDegree(TVertex v)
        {
            if (_vertexOutEdges.TryGetValue(v, out var edges) &&
                edges != null)
            {
                return edges.Length;
            }

            return 0;
        }

        public IEnumerable<TEdge> OutEdges(TVertex v)
        {
            if (_vertexOutEdges.TryGetValue(v, out var edges) &&
                edges != null)
            {
                return edges;
            }

            return Enumerable.Empty<TEdge>();
        }

        public bool TryGetOutEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            if (_vertexOutEdges.TryGetValue(v, out var aedges) &&
                aedges != null)
            {
                edges = aedges;
                return true;
            }

            edges = null;
            return false;
        }

        public TEdge OutEdge(TVertex v, int index)
        {
            return _vertexOutEdges[v][index];
        }

        public bool IsDirected
        {
            get { return true; }
        }

        public bool AllowParallelEdges
        {
            get { return true; }
        }

        public bool ContainsVertex(TVertex vertex)
        {
            return _vertexOutEdges.ContainsKey(vertex);
        }

        public bool IsVerticesEmpty
        {
            get { return _vertexOutEdges.Count == 0; }
        }

        public int VertexCount
        {
            get { return _vertexOutEdges.Count; }
        }

        public IEnumerable<TVertex> Vertices
        {
            get { return _vertexOutEdges.Keys; }
        }

        public bool IsEdgesEmpty
        {
            get { return EdgeCount == 0; }
        }

        public int EdgeCount { get; }

        public IEnumerable<TEdge> Edges
        {
            get
            {
                foreach (var edges in _vertexOutEdges.Values)
                {
                    if (edges != null)
                    {
                        foreach (var e in edges)
                        {
                            yield return e;
                        }
                    }
                }
            }
        }

        public bool ContainsEdge(TEdge edge)
        {
            if (_vertexOutEdges.TryGetValue(edge.Source, out var edges) &&
                edges != null)
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

        #endregion

        #region Members

        /// <summary>
        ///     Returns self since this class is immutable
        /// </summary>
        /// <returns></returns>
        public ArrayAdjacencyGraph<TVertex, TEdge> Clone()
        {
            return this;
        }

        #endregion
    }
}