﻿using System;

namespace PS.Graph
{
    /// <summary>
    ///     An event involving an edge.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    [Serializable]
    public class EdgeEventArgs<TVertex, TEdge> : EventArgs
        where TEdge : IEdge<TVertex>
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EdgeEventArgs&lt;TVertex, TEdge&gt;" /> class.
        /// </summary>
        /// <param name="edge">The edge.</param>
        public EdgeEventArgs(TEdge edge)
        {
            Edge = edge;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the edge.
        /// </summary>
        /// <value>The edge.</value>
        public TEdge Edge { get; }

        #endregion
    }
}