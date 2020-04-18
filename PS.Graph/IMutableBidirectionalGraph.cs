using System.Diagnostics.Contracts;
using PS.Graph.Contracts;

namespace PS.Graph
{
    /// <summary>
    ///     A mutable bidirectional directed graph
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    [ContractClass(typeof(MutableBidirectionalGraphContract<,>))]
    public interface IMutableBidirectionalGraph<TVertex, TEdge> : IMutableVertexAndEdgeListGraph<TVertex, TEdge>,
                                                                  IBidirectionalGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Members

        /// <summary>
        ///     Clears in-edges and out-edges of <paramref name="v" />
        /// </summary>
        /// <param name="v"></param>
        void ClearEdges(TVertex v);

        /// <summary>
        ///     Clears in-edges of <paramref name="v" />
        /// </summary>
        /// <param name="v"></param>
        void ClearInEdges(TVertex v);

        /// <summary>
        ///     Removes in-edges of <paramref name="v" /> that match
        ///     predicate <paramref name="edgePredicate" />.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="edgePredicate"></param>
        /// <returns>Number of edges removed</returns>
        int RemoveInEdgeIf(TVertex v, EdgePredicate<TVertex, TEdge> edgePredicate);

        #endregion
    }
}