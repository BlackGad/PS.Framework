using System;
using System.Collections.Generic;

namespace PS.Graph.Predicates
{
    [Serializable]
    public class FilteredIncidenceGraph<TVertex, TEdge, TGraph> : FilteredImplicitGraph<TVertex, TEdge, TGraph>,
                                                                  IIncidenceGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
        where TGraph : IIncidenceGraph<TVertex, TEdge>
    {
        #region Constructors

        public FilteredIncidenceGraph(
            TGraph baseGraph,
            VertexPredicate<TVertex> vertexPredicate,
            EdgePredicate<TVertex, TEdge> edgePredicate
        )
            : base(baseGraph, vertexPredicate, edgePredicate)
        {
        }

        #endregion

        #region IIncidenceGraph<TVertex,TEdge> Members

        public bool ContainsEdge(TVertex source, TVertex target)
        {
            if (!VertexPredicate(source))
            {
                return false;
            }

            if (!VertexPredicate(target))
            {
                return false;
            }

            foreach (var edge in BaseGraph.OutEdges(source))
            {
                if (edge.Target.Equals(target) && EdgePredicate(edge))
                {
                    return true;
                }
            }

            return false;
        }

        public bool TryGetEdge(
            TVertex source,
            TVertex target,
            out TEdge edge)
        {
            if (VertexPredicate(source) &&
                VertexPredicate(target) &&
                BaseGraph.TryGetEdges(source, target, out var unfilteredEdges))
            {
                foreach (var ufe in unfilteredEdges)
                {
                    if (EdgePredicate(ufe))
                    {
                        edge = ufe;
                        return true;
                    }
                }
            }

            edge = default;
            return false;
        }

        public bool TryGetEdges(
            TVertex source,
            TVertex target,
            out IEnumerable<TEdge> edges)
        {
            edges = null;
            if (!VertexPredicate(source))
            {
                return false;
            }

            if (!VertexPredicate(target))
            {
                return false;
            }

            if (BaseGraph.TryGetEdges(source, target, out var unfilteredEdges))
            {
                var filtered = new List<TEdge>();
                foreach (var edge in unfilteredEdges)
                {
                    if (EdgePredicate(edge))
                    {
                        filtered.Add(edge);
                    }
                }

                edges = filtered;
                return true;
            }

            return false;
        }

        #endregion
    }
}