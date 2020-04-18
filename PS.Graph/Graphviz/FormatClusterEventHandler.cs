using System;

namespace PS.Graph.Graphviz
{
    public delegate void FormatClusterEventHandler<TVertex, TEdge>(
        Object sender,
        FormatClusterEventArgs<TVertex, TEdge> e)
        where TEdge : IEdge<TVertex>;
}