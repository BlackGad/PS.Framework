namespace PS.Graph
{
    /// <summary>
    ///     A factory of identifiable edges.
    /// </summary>
    public delegate TEdge IdentifiableEdgeFactory<in TVertex, out TEdge>(TVertex source, TVertex target, string id)
        where TEdge : IEdge<TVertex>;
}