using System;
using System.Diagnostics;

namespace PS.Graph
{
    /// <summary>
    ///     An equatable, tagged, edge
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TTag"></typeparam>
    [Serializable]
    [DebuggerDisplay("{Source}->{Target}:{Tag}")]
    public class TaggedEquatableEdge<TVertex, TTag> : EquatableEdge<TVertex>,
                                                      ITagged<TTag>
    {
        private TTag _tag;

        #region Constructors

        public TaggedEquatableEdge(TVertex source, TVertex target, TTag tag)
            : base(source, target)
        {
            _tag = tag;
        }

        #endregion

        #region ITagged<TTag> Members

        public event EventHandler TagChanged;

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

        protected virtual void OnTagChanged(EventArgs e)
        {
            var eh = TagChanged;
            eh?.Invoke(this, e);
        }

        #endregion
    }
}