using System;
using System.Diagnostics;

namespace PS.Graph
{
    /// <summary>
    ///     An equatable edge implementation
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    [Serializable]
    [DebuggerDisplay("{Source}->{Target}")]
    public class EquatableEdge<TVertex> : Edge<TVertex>,
                                          IEquatable<EquatableEdge<TVertex>>
    {
        #region Constructors

        public EquatableEdge(TVertex source, TVertex target)
            : base(source, target)
        {
        }

        #endregion

        #region Override members

        public override bool Equals(object obj)
        {
            return Equals(obj as EquatableEdge<TVertex>);
        }

        public override int GetHashCode()
        {
            return
                HashCodeHelper.Combine(Source.GetHashCode(), Target.GetHashCode());
        }

        #endregion

        #region IEquatable<EquatableEdge<TVertex>> Members

        public bool Equals(EquatableEdge<TVertex> other)
        {
            return
                other != null &&
                Source.Equals(other.Source) &&
                Target.Equals(other.Target);
        }

        #endregion
    }
}