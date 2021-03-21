﻿using System.Collections.Generic;

namespace PS.Graph
{
    /// <summary>
    ///     A implicit directed graph data structure
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    public interface IImplicitGraph<in TVertex, TEdge> : IGraph,
                                                         IImplicitVertexSet<TVertex>
        where TEdge : IEdge<TVertex>
    {
        #region Members

        /// <summary>
        ///     Determines whether there are out-edges associated to <paramref name="v" />.
        /// </summary>
        /// <param name="v">The vertex.</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="v" /> has no out-edges; otherwise, <c>false</c>.
        /// </returns>
        bool IsOutEdgesEmpty(TVertex v);

        /// <summary>
        ///     Gets the count of out-edges of <paramref name="v" />
        /// </summary>
        /// <param name="v">The vertex.</param>
        /// <returns>The count of out-edges of <paramref name="v" /></returns>
        int OutDegree(TVertex v);

        /// <summary>
        ///     Gets the out-edge of <paramref name="v" /> at position <paramref name="index" />.
        /// </summary>
        /// <param name="v">The vertex.</param>
        /// <param name="index">The index.</param>
        /// <returns>The out-edge at position <paramref name="index" /></returns>
        TEdge OutEdge(TVertex v, int index);

        /// <summary>
        ///     Gets the out-edges of <paramref name="v" />.
        /// </summary>
        /// <param name="v">The vertex.</param>
        /// <returns>An enumeration of the out-edges of <paramref name="v" />.</returns>
        IEnumerable<TEdge> OutEdges(TVertex v);

        /// <summary>
        ///     Tries to get the out-edges of <paramref name="v" />
        /// </summary>
        /// <param name="v"></param>
        /// <param name="edges"></param>
        /// <returns></returns>
        bool TryGetOutEdges(TVertex v, out IEnumerable<TEdge> edges);

        #endregion
    }
}