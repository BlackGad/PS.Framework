using System.Diagnostics.Contracts;
using PS.Graph.Contracts;

namespace PS.Graph
{
    /// <summary>
    ///     A graph whose edges can be enumerated
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    [ContractClass(typeof(EdgeListGraphContract<,>))]
    public interface IEdgeListGraph<TVertex, TEdge> : IGraph,
                                                      IEdgeSet<TVertex, TEdge>,
                                                      IVertexSet<TVertex>
        where TEdge : IEdge<TVertex>
    {
    }
}