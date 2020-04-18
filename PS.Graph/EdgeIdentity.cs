namespace PS.Graph
{
    public delegate string EdgeIdentity<TVertex, in TEdge>(TEdge edge)
        where TEdge : IEdge<TVertex>;
}