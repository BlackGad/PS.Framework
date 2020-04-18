using System.Diagnostics.Contracts;

namespace PS.Graph
{
    [Pure]
    public delegate bool EdgePredicate<TVertex, in TEdge>(TEdge e)
        where TEdge : IEdge<TVertex>;
}