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
    public sealed class ArrayBidirectionalGraph<TVertex, TEdge> : IBidirectionalGraph<TVertex, TEdge>,
                                                                  ICloneable
        where TEdge : IEdge<TVertex>
    {
        private readonly Dictionary<TVertex, InOutEdges> _vertexEdges;

        #region Constructors

        /// <summary>
        ///     Constructs a new ArrayBidirectionalGraph instance from a
        ///     IBidirectionalGraph instance
        /// </summary>
        /// <param name="visitedGraph"></param>
        public ArrayBidirectionalGraph(
            IBidirectionalGraph<TVertex, TEdge> visitedGraph
        )
        {
            Contract.Requires(visitedGraph != null);

            _vertexEdges = new Dictionary<TVertex, InOutEdges>(visitedGraph.VertexCount);
            EdgeCount = visitedGraph.EdgeCount;
            foreach (var vertex in visitedGraph.Vertices)
            {
                var outEdges = visitedGraph.OutEdges(vertex).ToArray();
                var inEdges = visitedGraph.InEdges(vertex).ToArray();
                _vertexEdges.Add(vertex, new InOutEdges(outEdges, inEdges));
            }
        }

        private ArrayBidirectionalGraph(
            Dictionary<TVertex, InOutEdges> vertexEdges,
            int edgeCount
        )
        {
            Contract.Requires(vertexEdges != null);
            Contract.Requires(edgeCount >= 0);

            _vertexEdges = vertexEdges;
            EdgeCount = edgeCount;
        }

        #endregion

        #region IBidirectionalGraph<TVertex,TEdge> Members

        public bool ContainsEdge(TVertex source, TVertex target)
        {
            return TryGetEdge(source, target, out _);
        }

        public bool TryGetEdges(TVertex source, TVertex target, out IEnumerable<TEdge> edges)
        {
            if (_vertexEdges.TryGetValue(source, out var es))
            {
                List<TEdge> edges1 = null;
                if (es.TryGetOutEdges(out var outEdges))
                {
                    for (var i = 0; i < outEdges.Length; i++)
                    {
                        if (!outEdges[i].Target.Equals(target)) continue;
                        if (edges1 == null)
                        {
                            edges1 = new List<TEdge>(outEdges.Length - i);
                        }

                        edges1.Add(outEdges[i]);
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
            if (_vertexEdges.TryGetValue(source, out var io) && io.TryGetOutEdges(out var edges))
            {
                foreach (var e in edges)
                {
                    if (!e.Target.Equals(target)) continue;
                    edge = e;
                    return true;
                }
            }

            edge = default;
            return false;
        }

        public bool IsOutEdgesEmpty(TVertex v)
        {
            if (_vertexEdges.TryGetValue(v, out var io) && io.TryGetOutEdges(out _))
            {
                return false;
            }

            return true;
        }

        public int OutDegree(TVertex v)
        {
            if (_vertexEdges.TryGetValue(v, out var io) &&
                io.TryGetOutEdges(out var edges))
            {
                return edges.Length;
            }

            return 0;
        }

        public IEnumerable<TEdge> OutEdges(TVertex v)
        {
            if (TryGetInEdges(v, out var result))
            {
                return result;
            }

            return Enumerable.Empty<TEdge>();
        }

        public bool TryGetOutEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            if (_vertexEdges.TryGetValue(v, out var io) && io.TryGetOutEdges(out var resultEdges))
            {
                edges = resultEdges;
                return true;
            }

            edges = null;
            return false;
        }

        public TEdge OutEdge(TVertex v, int index)
        {
            var io = _vertexEdges[v];

            if (!io.TryGetOutEdges(out var edges))
            {
                Contract.Assert(false);
            }

            return edges[index];
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
            return _vertexEdges.ContainsKey(vertex);
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

        public bool IsEdgesEmpty
        {
            get { return EdgeCount == 0; }
        }

        public int EdgeCount { get; }

        public IEnumerable<TEdge> Edges
        {
            get
            {
                foreach (var io in _vertexEdges.Values)
                {
                    if (!io.TryGetOutEdges(out var edges)) continue;
                    foreach (var e in edges)
                    {
                        yield return e;
                    }
                }
            }
        }

        public bool ContainsEdge(TEdge edge)
        {
            if (_vertexEdges.TryGetValue(edge.Source, out var io) && io.TryGetOutEdges(out var edges))
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

        public bool IsInEdgesEmpty(TVertex v)
        {
            if (_vertexEdges.TryGetValue(v, out var io) && io.TryGetInEdges(out _))
            {
                return false;
            }

            return true;
        }

        public int InDegree(TVertex v)
        {
            if (_vertexEdges.TryGetValue(v, out var io) &&
                io.TryGetInEdges(out var edges))
            {
                return edges.Length;
            }

            return 0;
        }

        public IEnumerable<TEdge> InEdges(TVertex v)
        {
            if (TryGetInEdges(v, out var result))
            {
                return result;
            }

            return Enumerable.Empty<TEdge>();
        }

        public bool TryGetInEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            if (_vertexEdges.TryGetValue(v, out var io) &&
                io.TryGetInEdges(out var aedges))
            {
                edges = aedges;
                return true;
            }

            edges = null;
            return false;
        }

        public TEdge InEdge(TVertex v, int index)
        {
            var io = _vertexEdges[v];

            if (!io.TryGetOutEdges(out var edges))
            {
                Contract.Assert(false);
            }

            return edges[index];
        }

        public int Degree(TVertex v)
        {
            return InDegree(v) + OutDegree(v);
        }

        #endregion

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        #region Members

        /// <summary>
        ///     Returns self since this class is immutable        ///
        /// </summary>
        /// <returns></returns>
        public ArrayBidirectionalGraph<TVertex, TEdge> Clone()
        {
            return this;
        }

        #endregion

        #region Nested type: InOutEdges

        [Serializable]
        private struct InOutEdges
        {
            private readonly TEdge[] _inEdges;
            private readonly TEdge[] _outEdges;

            #region Constructors

            public InOutEdges(TEdge[] outEdges, TEdge[] inEdges)
            {
                _outEdges = outEdges != null && outEdges.Length > 0 ? outEdges : null;
                _inEdges = inEdges != null && inEdges.Length > 0 ? inEdges : null;
            }

            #endregion

            #region Members

            public bool TryGetInEdges(out TEdge[] edges)
            {
                edges = _inEdges;
                return edges != null;
            }

            public bool TryGetOutEdges(out TEdge[] edges)
            {
                edges = _outEdges;
                return edges != null;
            }

            #endregion
        }

        #endregion
    }
}