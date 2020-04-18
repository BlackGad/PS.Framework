using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace PS.Graph
{
    /// <summary>
    ///     The default <see cref="IEdge&lt;TVertex&gt;" /> implementation.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    [Serializable]
    [DebuggerDisplay("{Source}->{Target}")]
    public class Edge<TVertex> : IEdge<TVertex>
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Edge&lt;TVertex&gt;" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        public Edge(TVertex source, TVertex target)
        {
            Contract.Ensures(Source.Equals(source));
            Contract.Ensures(Target.Equals(target));

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
            return Source + "->" + Target;
        }

        #endregion

        #region IEdge<TVertex> Members

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