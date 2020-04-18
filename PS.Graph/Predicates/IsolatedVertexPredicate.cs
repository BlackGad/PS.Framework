using System.Diagnostics.Contracts;

namespace PS.Graph.Predicates
{
    /// <summary>
    ///     A vertex predicate that detects vertex with no in or out edges.
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    public sealed class IsolatedVertexPredicate<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private readonly IBidirectionalGraph<TVertex, TEdge> _visitedGraph;

        #region Constructors

        public IsolatedVertexPredicate(IBidirectionalGraph<TVertex, TEdge> visitedGraph)
        {
            _visitedGraph = visitedGraph;
        }

        #endregion

        #region Members

        [Pure]
        public bool Test(TVertex v)
        {
            return _visitedGraph.IsInEdgesEmpty(v)
                   && _visitedGraph.IsOutEdgesEmpty(v);
        }

        #endregion
    }
}