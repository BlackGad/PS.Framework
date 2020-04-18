using System.Diagnostics.Contracts;
using PS.Graph.Collections;

namespace PS.Graph.Contracts.Collections
{
    [ContractClassFor(typeof(IDisjointSet<>))]
    internal abstract class DisjointSetContract<T> : IDisjointSet<T>
    {
        #region IDisjointSet<T> Members

        int IDisjointSet<T>.SetCount
        {
            get { return default; }
        }

        int IDisjointSet<T>.ElementCount
        {
            get { return default; }
        }

        void IDisjointSet<T>.MakeSet(T value)
        {
            IDisjointSet<T> iThis = this;
            Contract.Requires(value != null);
            Contract.Requires(!iThis.Contains(value));
            Contract.Ensures(iThis.Contains(value));
            Contract.Ensures(iThis.SetCount == Contract.OldValue(iThis.SetCount) + 1);
            Contract.Ensures(iThis.ElementCount == Contract.OldValue(iThis.ElementCount) + 1);
        }

        T IDisjointSet<T>.FindSet(T value)
        {
            IDisjointSet<T> iThis = this;
            Contract.Requires(value != null);
            Contract.Requires(iThis.Contains(value));

            return default;
        }

        bool IDisjointSet<T>.Union(T left, T right)
        {
            IDisjointSet<T> iThis = this;
            Contract.Requires(left != null);
            Contract.Requires(iThis.Contains(left));
            Contract.Requires(right != null);
            Contract.Requires(iThis.Contains(right));

            return default;
        }

        [Pure]
        bool IDisjointSet<T>.Contains(T value)
        {
            return default;
        }

        bool IDisjointSet<T>.AreInSameSet(T left, T right)
        {
            IDisjointSet<T> iThis = this;
            Contract.Requires(left != null);
            Contract.Requires(right != null);
            Contract.Requires(iThis.Contains(left));
            Contract.Requires(iThis.Contains(right));

            return default;
        }

        #endregion

        #region Members

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            IDisjointSet<T> iThis = this;
            Contract.Invariant(0 <= iThis.SetCount);
            Contract.Invariant(iThis.SetCount <= iThis.ElementCount);
        }

        #endregion
    }
}