﻿using System.Collections.Generic;
using System.Diagnostics.Contracts;
using PS.Graph.Contracts;

namespace PS.Graph
{
    /// <summary>
    ///     A mutable edge list graph.
    /// </summary>
    /// <typeparam name="TVertex">the vertex type</typeparam>
    /// <typeparam name="TEdge">the edge type</typeparam>
    [ContractClass(typeof(MutableEdgeListGraphContract<,>))]
    public interface IMutableEdgeListGraph<TVertex, TEdge> : IMutableGraph,
                                                             IEdgeListGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Events

        /// <summary>
        ///     Raised when an edge is added to the graph.
        /// </summary>
        event EdgeAction<TVertex, TEdge> EdgeAdded;

        /// <summary>
        ///     Raised when an edge has been removed from the graph.
        /// </summary>
        event EdgeAction<TVertex, TEdge> EdgeRemoved;

        #endregion

        #region Members

        /// <summary>
        ///     Adds the edge to the graph
        /// </summary>
        /// <param name="edge"></param>
        /// <returns>true if the edge was added, otherwise false.</returns>
        bool AddEdge(TEdge edge);

        /// <summary>
        ///     Adds a set of edges to the graph.
        /// </summary>
        /// <param name="edges"></param>
        /// <returns>the number of edges successfully added to the graph.</returns>
        int AddEdgeRange(IEnumerable<TEdge> edges);

        /// <summary>
        ///     Removes <paramref name="edge" /> from the graph
        /// </summary>
        /// <param name="edge"></param>
        /// <returns>true if <paramref name="edge" /> was successfully removed; otherwise false.</returns>
        bool RemoveEdge(TEdge edge);

        /// <summary>
        ///     Removes all edges that match <paramref name="predicate" />.
        /// </summary>
        /// <param name="predicate">
        ///     A pure delegate that takes an <typeparamref name="TEdge" /> and returns true if the edge should
        ///     be removed.
        /// </param>
        /// <returns>the number of edges removed.</returns>
        int RemoveEdgeIf(EdgePredicate<TVertex, TEdge> predicate);

        #endregion
    }
}