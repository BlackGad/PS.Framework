using System;

namespace PS.Graph.Graphviz
{
    public delegate void FormatVertexEventHandler<TVertex>(
        Object sender,
        FormatVertexEventArgs<TVertex> e);
}