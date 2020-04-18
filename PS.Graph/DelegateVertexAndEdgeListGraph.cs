using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace PS.Graph
{
    /// <summary>
    ///     A delegate-based incidence graph
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    [Serializable]
    public class DelegateVertexAndEdgeListGraph<TVertex, TEdge> : DelegateIncidenceGraph<TVertex, TEdge>,
                                                                  IVertexAndEdgeListGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>, IEquatable<TEdge>
    {
        private readonly IEnumerable<TVertex> _vertices;
        private int _edgeCount = -1;
        private int _vertexCount = -1;

        #region Constructors

        public DelegateVertexAndEdgeListGraph(
            IEnumerable<TVertex> vertices,
            TryFunc<TVertex, IEnumerable<TEdge>> tryGetOutEdges)
            : base(tryGetOutEdges)
        {
            Contract.Requires(vertices != null);
            Contract.Requires(vertices.All(v => tryGetOutEdges(v, out var edges)));
            _vertices = vertices;
        }

        #endregion

        #region IVertexAndEdgeListGraph<TVertex,TEdge> Members

        public bool IsVerticesEmpty
        {
            get
            {
                // shortcut if count is already computed
                if (_vertexCount > -1)
                {
                    return _vertexCount == 0;
                }

                return !_vertices.Any();
            }
        }

        public int VertexCount
        {
            get
            {
                if (_vertexCount < 0)
                {
                    _vertexCount = _vertices.Count();
                }

                return _vertexCount;
            }
        }

        public virtual IEnumerable<TVertex> Vertices
        {
            get { return _vertices; }
        }

        public bool IsEdgesEmpty
        {
            get
            {
                // shortcut if edges is already computed
                if (_edgeCount > -1)
                {
                    return _edgeCount == 0;
                }

                return !_vertices.SelectMany(OutEdges).Any();
            }
        }

        public int EdgeCount
        {
            get
            {
                if (_edgeCount < 0)
                {
                    _edgeCount = Edges.Count();
                }

                return _edgeCount;
            }
        }

        public virtual IEnumerable<TEdge> Edges
        {
            get
            {
                foreach (var vertex in _vertices)
                foreach (var edge in OutEdges(vertex))
                {
                    yield return edge;
                }
            }
        }

        public bool ContainsEdge(TEdge edge)
        {
            if (TryGetOutEdges(edge.Source, out var edges))
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
    }
}