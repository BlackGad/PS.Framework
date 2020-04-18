using System;
using System.Collections.Generic;

namespace PS.Graph.Predicates
{
    [Serializable]
    public sealed class FilteredEdgeListGraph<TVertex, TEdge, TGraph> : FilteredImplicitVertexSet<TVertex, TEdge, TGraph>,
                                                                        IEdgeListGraph<TVertex, TEdge>
        where TGraph : IEdgeListGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Constructors

        public FilteredEdgeListGraph(
            TGraph baseGraph,
            VertexPredicate<TVertex> vertexPredicate,
            EdgePredicate<TVertex, TEdge> edgePredicate
        )
            : base(baseGraph, vertexPredicate, edgePredicate)
        {
        }

        #endregion

        #region IEdgeListGraph<TVertex,TEdge> Members

        public bool IsVerticesEmpty
        {
            get
            {
                foreach (var v in BaseGraph.Vertices)
                {
                    if (VertexPredicate(v))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public int VertexCount
        {
            get
            {
                var count = 0;
                foreach (var v in BaseGraph.Vertices)
                {
                    if (VertexPredicate(v))
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public IEnumerable<TVertex> Vertices
        {
            get
            {
                foreach (var v in BaseGraph.Vertices)
                {
                    if (VertexPredicate(v))
                    {
                        yield return v;
                    }
                }
            }
        }

        public bool IsEdgesEmpty
        {
            get
            {
                foreach (var edge in BaseGraph.Edges)
                {
                    if (FilterEdge(edge))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public int EdgeCount
        {
            get
            {
                var count = 0;
                foreach (var edge in BaseGraph.Edges)
                {
                    if (FilterEdge(edge))
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public IEnumerable<TEdge> Edges
        {
            get
            {
                foreach (var edge in BaseGraph.Edges)
                {
                    if (FilterEdge(edge))
                    {
                        yield return edge;
                    }
                }
            }
        }

        public bool ContainsEdge(TEdge edge)
        {
            return
                FilterEdge(edge) &&
                BaseGraph.ContainsEdge(edge);
        }

        #endregion

        #region Members

        private bool FilterEdge(TEdge edge)
        {
            return VertexPredicate(edge.Source)
                   && VertexPredicate(edge.Target)
                   && EdgePredicate(edge);
        }

        #endregion
    }
}