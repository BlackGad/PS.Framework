using System.Diagnostics.Contracts;
using PS.Graph.Contracts;

namespace PS.Graph
{
    /// <summary>
    ///     A directed graph data structure that is efficient
    ///     to traverse both in and out edges.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    [ContractClass(typeof(BidirectionalGraphContract<,>))]
    public interface IBidirectionalGraph<TVertex, TEdge> : IVertexAndEdgeListGraph<TVertex, TEdge>,
                                                           IBidirectionalIncidenceGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
    }
}