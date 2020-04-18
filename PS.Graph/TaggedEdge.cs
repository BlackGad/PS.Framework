using System;
using System.Diagnostics.Contracts;

namespace PS.Graph
{
    [Serializable]
    public class TaggedEdge<TVertex, TTag> : Edge<TVertex>,
                                             ITagged<TTag>
    {
        private TTag _tag;

        #region Constructors

        public TaggedEdge(TVertex source, TVertex target, TTag tag)
            : base(source, target)
        {
            Contract.Ensures(Equals(Tag, tag));

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