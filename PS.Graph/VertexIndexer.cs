using System.Diagnostics.Contracts;

namespace PS.Graph
{
    [Pure]
    public delegate int VertexIndexer<in TVertex>(TVertex v);
}