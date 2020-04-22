namespace PS.Graph
{
    /// <summary>
    ///     A mutable graph instance
    /// </summary>
    public interface IMutableGraph<TVertex, TEdge> : IGraph
        where TEdge : IEdge<TVertex>
    {
        #region Events

        /// <summary>
        ///     Called when the graph vertices and edges have been cleared.
        /// </summary>
        event ClearAction<TVertex, TEdge> Cleared;

        #endregion

        #region Members

        /// <summary>
        ///     Clears the vertex and edges
        /// </summary>
        void Clear();

        #endregion
    }
}