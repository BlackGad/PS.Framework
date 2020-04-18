using System;

namespace PS.Graph.Predicates
{
    [Serializable]
    public sealed class SinkVertexPredicate<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private readonly IIncidenceGraph<TVertex, TEdge> _visitedGraph;

        #region Constructors

        public SinkVertexPredicate(IIncidenceGraph<TVertex, TEdge> visitedGraph)
        {
            _visitedGraph = visitedGraph;
        }

        #endregion

        #region Members

        public bool Test(TVertex v)
        {
            return _visitedGraph.IsOutEdgesEmpty(v);
        }

        #endregion
    }
}