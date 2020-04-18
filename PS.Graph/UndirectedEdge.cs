using System;
using System.Diagnostics;

namespace PS.Graph
{
    /// <summary>
    ///     The default <see cref="IUndirectedEdge&lt;TVertex&gt;" /> implementation.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    [Serializable]
    [DebuggerDisplay(EdgeExtensions.DebuggerDisplayUndirectedEdgeFormatString)]
    public class UndirectedEdge<TVertex> : IUndirectedEdge<TVertex>
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Edge&lt;TVertex&gt;" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        public UndirectedEdge(TVertex source, TVertex target)
        {
            Source = source;
            Target = target;
        }

        #endregion

        #region Override members

        /// <summary>
        ///     Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        /// </returns>
        public override string ToString()
        {
            return String.Format(
                EdgeExtensions.UndirectedEdgeFormatString,
                Source,
                Target);
        }

        #endregion

        #region IUndirectedEdge<TVertex> Members

        /// <summary>
        ///     Gets the source vertex
        /// </summary>
        /// <value></value>
        public TVertex Source { get; }

        /// <summary>
        ///     Gets the target vertex
        /// </summary>
        /// <value></value>
        public TVertex Target { get; }

        #endregion
    }
}