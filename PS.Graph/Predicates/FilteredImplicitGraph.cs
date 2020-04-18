using System;
using System.Collections.Generic;

namespace PS.Graph.Predicates
{
    [Serializable]
    public class FilteredImplicitGraph<TVertex, TEdge, TGraph> : FilteredImplicitVertexSet<TVertex, TEdge, TGraph>,
                                                                 IImplicitGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
        where TGraph : IImplicitGraph<TVertex, TEdge>
    {
        #region Constructors

        public FilteredImplicitGraph(
            TGraph baseGraph,
            VertexPredicate<TVertex> vertexPredicate,
            EdgePredicate<TVertex, TEdge> edgePredicate
        )
            : base(baseGraph, vertexPredicate, edgePredicate)
        {
        }

        #endregion

        #region IImplicitGraph<TVertex,TEdge> Members

        public bool IsOutEdgesEmpty(TVertex v)
        {
            return OutDegree(v) == 0;
        }

        public int OutDegree(TVertex v)
        {
            var count = 0;
            foreach (var edge in BaseGraph.OutEdges(v))
            {
                if (TestEdge(edge))
                {
                    count++;
                }
            }

            return count;
        }

        public IEnumerable<TEdge> OutEdges(TVertex v)
        {
            foreach (var edge in BaseGraph.OutEdges(v))
            {
                if (TestEdge(edge))
                {
                    yield return edge;
                }
            }
        }

        public bool TryGetOutEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            if (!BaseGraph.TryGetOutEdges(v, out _))
            {
                edges = null;
                return false;
            }

            edges = OutEdges(v);
            return true;
        }

        public TEdge OutEdge(TVertex v, int index)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}