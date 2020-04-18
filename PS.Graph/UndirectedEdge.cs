using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;

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
        private readonly TVertex _source;
        private readonly TVertex _target;

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Edge&lt;TVertex&gt;" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        public UndirectedEdge(TVertex source, TVertex target)
        {
            Contract.Requires(source != null);
            Contract.Requires(target != null);
            Contract.Requires(Comparer<TVertex>.Default.Compare(source, target) <= 0);
            Contract.Ensures(_source.Equals(source));
            Contract.Ensures(_target.Equals(target));

            _source = source;
            _target = target;
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
        public TVertex Source
        {
            get { return _source; }
        }

        /// <summary>
        ///     Gets the target vertex
        /// </summary>
        /// <value></value>
        public TVertex Target
        {
            get { return _target; }
        }

        #endregion
    }
}