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
    [DebuggerDisplay("{Source}->{Target}:{Tag}")]
    public struct STaggedEdge<TVertex, TTag> : IEdge<TVertex>,
                                               ITagged<TTag>
    {
        private TTag tag;

        public STaggedEdge(TVertex source, TVertex target, TTag tag)
        {
            Source = source;
            Target = target;
            this.tag = tag;
            TagChanged = null;
        }

        public TVertex Source { get; }

        public TVertex Target { get; }

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
            return $"{Source}->{Target}:{Tag}";
        }
    }
}