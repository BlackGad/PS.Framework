namespace PS.Graph
{
    public delegate bool EdgeEqualityComparer<in TVertex, in TEdge>(TEdge edge, TVertex source, TVertex target)
        where TEdge : IEdge<TVertex>;
}