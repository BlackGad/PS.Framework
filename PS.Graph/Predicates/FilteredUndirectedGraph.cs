using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace PS.Graph.Predicates
{
    [Serializable]
    public sealed class FilteredUndirectedGraph<TVertex, TEdge, TGraph> : FilteredGraph<TVertex, TEdge, TGraph>,
                                                                          IUndirectedGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
        where TGraph : IUndirectedGraph<TVertex, TEdge>
    {
        #region Constructors

        public FilteredUndirectedGraph(
            TGraph baseGraph,
            VertexPredicate<TVertex> vertexPredicate,
            EdgePredicate<TVertex, TEdge> edgePredicate
        )
            : base(baseGraph, vertexPredicate, edgePredicate)
        {
        }

        #endregion

        #region IUndirectedGraph<TVertex,TEdge> Members

        public EdgeEqualityComparer<TVertex, TEdge> EdgeEqualityComparer { get; } = EdgeExtensions.GetUndirectedVertexEquality<TVertex, TEdge>();

        [Pure]
        public IEnumerable<TEdge> AdjacentEdges(TVertex v)
        {
            if (VertexPredicate(v))
            {
                foreach (var edge in BaseGraph.AdjacentEdges(v))
                {
                    if (TestEdge(edge))
                    {
                        yield return edge;
                    }
                }
            }
        }

        [Pure]
        public int AdjacentDegree(TVertex v)
        {
            return AdjacentEdges(v).Count();
        }

        [Pure]
        public bool IsAdjacentEdgesEmpty(TVertex v)
        {
            return !AdjacentEdges(v).Any();
        }

        [Pure]
        public TEdge AdjacentEdge(TVertex v, int index)
        {
            if (VertexPredicate(v))
            {
                var count = 0;
                foreach (var edge in AdjacentEdges(v))
                {
                    if (count == index)
                    {
                        return edge;
                    }

                    count++;
                }
            }

            throw new IndexOutOfRangeException();
        }

        public bool TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            if (VertexPredicate(source) &&
                VertexPredicate(target))
            {
                // we need to find the edge
                foreach (var e in Edges)
                {
                    if (EdgeEqualityComparer(e, source, target)
                        && EdgePredicate(e))
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
        public bool ContainsEdge(TVertex source, TVertex target)
        {
            return TryGetEdge(source, target, out _);
        }

        public bool IsEdgesEmpty
        {
            get { return !Edges.Any(); }
        }

        public int EdgeCount
        {
            get { return Edges.Count(); }
        }

        public IEnumerable<TEdge> Edges
        {
            get
            {
                foreach (var edge in BaseGraph.Edges)
                {
                    if (TestEdge(edge))
                    {
                        yield return edge;
                    }
                }
            }
        }

        [Pure]
        public bool ContainsEdge(TEdge edge)
        {
            if (!TestEdge(edge))
            {
                return false;
            }

            return BaseGraph.ContainsEdge(edge);
        }

        public bool IsVerticesEmpty
        {
            get { return !Vertices.Any(); }
        }

        public int VertexCount
        {
            get { return Vertices.Count(); }
        }

        public IEnumerable<TVertex> Vertices
        {
            get
            {
                foreach (var vertex in BaseGraph.Vertices)
                {
                    if (VertexPredicate(vertex))
                    {
                        yield return vertex;
                    }
                }
            }
        }

        [Pure]
        public bool ContainsVertex(TVertex vertex)
        {
            if (!VertexPredicate(vertex))
            {
                return false;
            }

            return BaseGraph.ContainsVertex(vertex);
        }

        #endregion
    }
}