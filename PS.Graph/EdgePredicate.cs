namespace PS.Graph
{
    public delegate bool EdgePredicate<TVertex, in TEdge>(TEdge e)
        where TEdge : IEdge<TVertex>;
}