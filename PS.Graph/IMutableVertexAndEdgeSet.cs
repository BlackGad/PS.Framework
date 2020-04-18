using System.Collections.Generic;
using System.Diagnostics.Contracts;
using PS.Graph.Contracts;

namespace PS.Graph
{
    /// <summary>
    ///     A mutable vertex and edge set
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    [ContractClass(typeof(MutableVertexAndEdgeSetContract<,>))]
    public interface IMutableVertexAndEdgeSet<TVertex, TEdge> : IMutableVertexSet<TVertex>,
                                                                IMutableEdgeListGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Members

        /// <summary>
        ///     Adds the vertices and edge to the graph.
        /// </summary>
        /// <param name="edge"></param>
        /// <returns>true if the edge was added, otherwise false.</returns>
        bool AddVerticesAndEdge(TEdge edge);

        /// <summary>
        ///     Adds a set of edges (and it's vertices if necessary)
        /// </summary>
        /// <param name="edges"></param>
        /// <returns>the number of edges added.</returns>
        int AddVerticesAndEdgeRange(IEnumerable<TEdge> edges);

        #endregion
    }
}