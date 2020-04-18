using System;

namespace PS.Graph
{
    public delegate void UndirectedEdgeAction<TVertex, TEdge>(
        Object sender,
        UndirectedEdgeEventArgs<TVertex, TEdge> e)
        where TEdge : IEdge<TVertex>;
}