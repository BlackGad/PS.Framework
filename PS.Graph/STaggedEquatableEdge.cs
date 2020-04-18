using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace PS.Graph
{
    /// <summary>
    ///     A tagged edge as value type.
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TTag"></typeparam>
    [Serializable]
    [StructLayout(LayoutKind.Auto)]
    [DebuggerDisplay(EdgeExtensions.DebuggerDisplayTaggedEdgeFormatString)]
    public struct STaggedEquatableEdge<TVertex, TTag> : IEdge<TVertex>,
                                                        IEquatable<STaggedEquatableEdge<TVertex, TTag>>,
                                                        ITagged<TTag>
    {
        private readonly TVertex source;
        private readonly TVertex target;
        private TTag tag;

        public STaggedEquatableEdge(TVertex source, TVertex target, TTag tag)
        {
            this.source = source;
            this.target = target;
            this.tag = tag;
            TagChanged = null;
        }

        public TVertex Source
        {
            get { return source; }
        }

        public TVertex Target
        {
            get { return target; }
        }

        public event EventHandler TagChanged;

        private void OnTagChanged(EventArgs e)
        {
            var eh = TagChanged;
            eh?.Invoke(this, e);
        }

        public TTag Tag
        {
            get { return tag; }
            set
            {
                if (!Equals(tag, value))
                {
                    tag = value;
                    OnTagChanged(EventArgs.Empty);
                }
            }
        }

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

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///     true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(STaggedEquatableEdge<TVertex, TTag> other)
        {
            return
                source.Equals(other.source) &&
                target.Equals(other.target);
        }

        /// <summary>
        ///     Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        ///     true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is STaggedEquatableEdge<TVertex, TTag> edge && Equals(edge);
        }

        /// <summary>
        ///     Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return HashCodeHelper.Combine(
                source.GetHashCode(),
                target.GetHashCode());
        }
    }
}