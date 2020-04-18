namespace PS.Graph
{
    /// <summary>
    ///     An edge factory
    /// </summary>
    public delegate TEdge EdgeFactory<in TVertex, out TEdge>(TVertex source, TVertex target)
        where TEdge : IEdge<TVertex>;
}