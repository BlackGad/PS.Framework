namespace PS.Graph
{
    /// <summary>
    ///     A mutable vertex list graph
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    public interface IMutableVertexListGraph<TVertex, TEdge> : IMutableIncidenceGraph<TVertex, TEdge>,
                                                               IMutableVertexSet<TVertex>
        where TEdge : IEdge<TVertex>
    {
    }
}