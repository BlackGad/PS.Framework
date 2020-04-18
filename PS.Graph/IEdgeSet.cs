using System.Collections.Generic;

namespace PS.Graph
{
    /// <summary>
    ///     A set of edges
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    public interface IEdgeSet<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Properties

        /// <summary>
        ///     Gets the edge count.
        /// </summary>
        /// <value>The edge count.</value>
        int EdgeCount { get; }

        /// <summary>
        ///     Gets the edges.
        /// </summary>
        /// <value>The edges.</value>
        IEnumerable<TEdge> Edges { get; }

        /// <summary>
        ///     Gets a value indicating whether there are no edges in this set.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this set is empty; otherwise, <c>false</c>.
        /// </value>
        bool IsEdgesEmpty { get; }

        #endregion

        #region Members

        /// <summary>
        ///     Determines whether the specified edge contains edge.
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <returns>
        ///     <c>true</c> if the specified edge contains edge; otherwise, <c>false</c>.
        /// </returns>
        bool ContainsEdge(TEdge edge);

        #endregion
    }
}