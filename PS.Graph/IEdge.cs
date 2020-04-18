namespace PS.Graph
{
    /// <summary>
    ///     A directed edge
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    public interface IEdge<out TVertex>
    {
        #region Properties

        /// <summary>
        ///     Gets the source vertex
        /// </summary>
        TVertex Source { get; }

        /// <summary>
        ///     Gets the target vertex
        /// </summary>
        TVertex Target { get; }

        #endregion
    }
}