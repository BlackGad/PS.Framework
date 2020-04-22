using System.Collections.Generic;

namespace PS.Graph
{
    /// <summary>
    ///     The handler for clear method
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    public delegate void ClearAction<TVertex, TEdge>(IReadOnlyList<TVertex> obsoleteVertices, IReadOnlyList<TEdge> obsoleteEdges)
        where TEdge : IEdge<TVertex>;
}