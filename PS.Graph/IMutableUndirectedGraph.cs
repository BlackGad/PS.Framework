namespace PS.Graph
{
    /// <summary>
    ///     A mutable indirect graph
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    public interface IMutableUndirectedGraph<TVertex, TEdge> : IUndirectedGraph<TVertex, TEdge>,
                                                               IMutableVertexAndEdgeSet<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Members

        void ClearAdjacentEdges(TVertex vertex);
        int RemoveAdjacentEdgeIf(TVertex vertex, EdgePredicate<TVertex, TEdge> predicate);

        #endregion
    }
}