namespace PS.Graph.Algorithms
{
    /// <summary>
    ///     An algorithm that exposes events to compute a distance map between vertices
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    public interface IDistanceRecorderAlgorithm<out TVertex>
    {
        #region Events

        event VertexAction<TVertex> DiscoverVertex;
        event VertexAction<TVertex> InitializeVertex;

        #endregion
    }
}