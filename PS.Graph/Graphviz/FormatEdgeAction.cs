namespace PS.Graph.Graphviz
{
    public delegate void FormatEdgeAction<TVertex, TEdge>(
        object sender,
        FormatEdgeEventArgs<TVertex, TEdge> e)
        where TEdge : IEdge<TVertex>;
}