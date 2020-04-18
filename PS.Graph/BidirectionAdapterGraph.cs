using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using PS.Graph.Collections;

namespace PS.Graph
{
    /// <summary>
    ///     Wraps a vertex list graph (out-edges only) and caches the in-edge dictionary.
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    [Serializable]
    [DebuggerDisplay("VertexCount = {VertexCount}, EdgeCount = {EdgeCount}")]
    public class BidirectionAdapterGraph<TVertex, TEdge> : IBidirectionalGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Constants

        private static readonly TEdge[] EmptyEdges = new TEdge[0];

        #endregion

        private readonly IVertexAndEdgeListGraph<TVertex, TEdge> _baseGraph;
        private readonly Dictionary<TVertex, EdgeList<TVertex, TEdge>> _inEdges;

        #region Constructors

        public BidirectionAdapterGraph(IVertexAndEdgeListGraph<TVertex, TEdge> baseGraph)
        {
            Contract.Requires(baseGraph != null);

            _baseGraph = baseGraph;
            _inEdges = new Dictionary<TVertex, EdgeList<TVertex, TEdge>>(_baseGraph.VertexCount);
            foreach (var edge in _baseGraph.Edges)
            {
                if (!_inEdges.TryGetValue(edge.Target, out var list))
                {
                    _inEdges.Add(edge.Target, list = new EdgeList<TVertex, TEdge>());
                }

                list.Add(edge);
            }
        }

        #endregion

        #region IBidirectionalGraph<TVertex,TEdge> Members

        [Pure]
        public bool IsInEdgesEmpty(TVertex v)
        {
            return InDegree(v) == 0;
        }

        [Pure]
        public int InDegree(TVertex v)
        {
            if (_inEdges.TryGetValue(v, out var edges))
            {
                return edges.Count;
            }

            return 0;
        }

        [Pure]
        public IEnumerable<TEdge> InEdges(TVertex v)
        {
            if (_inEdges.TryGetValue(v, out var edges))
            {
                return edges;
            }

            return EmptyEdges;
        }

        [Pure]
        public bool TryGetInEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            if (_inEdges.TryGetValue(v, out var es))
            {
                edges = es;
                return true;
            }

            edges = null;
            return false;
        }

        [Pure]
        public TEdge InEdge(TVertex v, int index)
        {
            return _inEdges[v][index];
        }

        [Pure]
        public int Degree(TVertex v)
        {
            return InDegree(v) + OutDegree(v);
        }

        [Pure]
        public bool ContainsEdge(TVertex source, TVertex target)
        {
            return _baseGraph.ContainsEdge(source, target);
        }

        [Pure]
        public bool TryGetEdges(TVertex source, TVertex target, out IEnumerable<TEdge> edges)
        {
            return _baseGraph.TryGetEdges(source, target, out edges);
        }

        [Pure]
        public bool TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            return _baseGraph.TryGetEdge(source, target, out edge);
        }

        [Pure] // InterfacePureBug
        public bool IsOutEdgesEmpty(TVertex v)
        {
            return _baseGraph.IsOutEdgesEmpty(v);
        }

        [Pure]
        public int OutDegree(TVertex v)
        {
            return _baseGraph.OutDegree(v);
        }

        [Pure]
        public IEnumerable<TEdge> OutEdges(TVertex v)
        {
            return _baseGraph.OutEdges(v);
        }

        [Pure]
        public bool TryGetOutEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            return _baseGraph.TryGetOutEdges(v, out edges);
        }

        [Pure]
        public TEdge OutEdge(TVertex v, int index)
        {
            return _baseGraph.OutEdge(v, index);
        }

        public bool IsDirected
        {
            get { return _baseGraph.IsDirected; }
        }

        public bool AllowParallelEdges
        {
            get { return _baseGraph.AllowParallelEdges; }
        }

        public bool IsVerticesEmpty
        {
            get { return _baseGraph.IsVerticesEmpty; }
        }

        public int VertexCount
        {
            get { return _baseGraph.VertexCount; }
        }

        public IEnumerable<TVertex> Vertices
        {
            get { return _baseGraph.Vertices; }
        }

        [Pure]
        public bool ContainsVertex(TVertex vertex)
        {
            return _baseGraph.ContainsVertex(vertex);
        }

        public bool IsEdgesEmpty
        {
            get { return _baseGraph.IsEdgesEmpty; }
        }

        public int EdgeCount
        {
            get { return _baseGraph.EdgeCount; }
        }

        public virtual IEnumerable<TEdge> Edges
        {
            get { return _baseGraph.Edges; }
        }

        [Pure]
        public bool ContainsEdge(TEdge edge)
        {
            return _baseGraph.ContainsEdge(edge);
        }

        #endregion
    }
}