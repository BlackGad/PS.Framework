using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace PS.Graph
{
    /// <summary>
    ///     An struct based <see cref="IEdge&lt;TVertex&gt;" /> implementation.    ///
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    [Serializable]
    [DebuggerDisplay(EdgeExtensions.DebuggerDisplayEdgeFormatString)]
    [StructLayout(LayoutKind.Auto)]
    public struct SEdge<TVertex>
        : IEdge<TVertex>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SEdge&lt;TVertex&gt;" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        public SEdge(TVertex source, TVertex target)
        {
            Contract.Ensures(Contract.ValueAtReturn(out this).Source.Equals(source));
            Contract.Ensures(Contract.ValueAtReturn(out this).Target.Equals(target));

            Source = source;
            Target = target;
        }

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

        /// <summary>
        ///     Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        /// </returns>
        public override string ToString()
        {
            return String.Format(
                EdgeExtensions.EdgeFormatString,
                Source,
                Target);
        }
    }
}