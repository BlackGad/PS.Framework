namespace PS.Graph
{
    /// <summary>
    ///     A graph with vertices and edges
    /// </summary>
    public interface IGraph
    {
        #region Properties

        /// <summary>
        ///     Gets a value indicating if the graph allows parallel edges
        /// </summary>
        bool AllowParallelEdges { get; }

        /// <summary>
        ///     Gets a value indicating if the graph is directed
        /// </summary>
        bool IsDirected { get; }

        #endregion
    }
}