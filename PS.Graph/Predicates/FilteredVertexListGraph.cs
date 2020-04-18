using System;
using System.Collections.Generic;

namespace PS.Graph.Predicates
{
    [Serializable]
    public class FilteredVertexListGraph<TVertex, TEdge, TGraph> : FilteredIncidenceGraph<TVertex, TEdge, TGraph>,
                                                                   IVertexListGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
        where TGraph : IVertexListGraph<TVertex, TEdge>
    {
        #region Constructors

        public FilteredVertexListGraph(
            TGraph baseGraph,
            VertexPredicate<TVertex> vertexPredicate,
            EdgePredicate<TVertex, TEdge> edgePredicate
        )
            : base(baseGraph, vertexPredicate, edgePredicate)
        {
        }

        #endregion

        #region IVertexListGraph<TVertex,TEdge> Members

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

        #endregion
    }
}