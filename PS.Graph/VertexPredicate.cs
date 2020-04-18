using System.Diagnostics.Contracts;

namespace PS.Graph
{
    [Pure]
    public delegate bool VertexPredicate<in TVertex>(TVertex v);
}