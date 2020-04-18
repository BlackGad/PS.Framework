using System.Diagnostics.Contracts;

namespace PS.Graph
{
    [Pure]
    public delegate string VertexIdentity<in TVertex>(TVertex v);
}