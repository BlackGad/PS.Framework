using System;
using System.Diagnostics;

namespace PS.Graph
{
    /// <summary>
    ///     A tagged undirected edge.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TTag">Type type of the tag</typeparam>
    [Serializable]
    [DebuggerDisplay(EdgeExtensions.DebuggerDisplayTaggedUndirectedEdgeFormatString)]
    public class TaggedUndirectedEdge<TVertex, TTag> : UndirectedEdge<TVertex>,
                                                       ITagged<TTag>
    {
        private TTag _tag;

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="TaggedUndirectedEdge&lt;TVertex, TTag&gt;" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="tag">the tag</param>
        public TaggedUndirectedEdge(TVertex source, TVertex target, TTag tag)
            : base(source, target)
        {
            _tag = tag;
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
                EdgeExtensions.TaggedUndirectedEdgeFormatString,
                Source,
                Target,
                Tag);
        }

        #endregion

        #region ITagged<TTag> Members

        /// <summary>
        ///     Raised when the tag is changed
        /// </summary>
        public event EventHandler TagChanged;

        /// <summary>
        ///     Gets or sets the tag
        /// </summary>
        public TTag Tag
        {
            get { return _tag; }
            set
            {
                if (!Equals(_tag, value))
                {
                    _tag = value;
                    OnTagChanged(EventArgs.Empty);
                }
            }
        }

        #endregion

        #region Members

        private void OnTagChanged(EventArgs e)
        {
            var eh = TagChanged;
            eh?.Invoke(this, e);
        }

        #endregion
    }
}