using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace PS.Graph.Contracts
{
    [ContractClassFor(typeof(IUndirectedEdge<>))]
    internal abstract class UndirectedEdgeContract<TVertex> : IUndirectedEdge<TVertex>
    {
        #region IUndirectedEdge<TVertex> Members

        public TVertex Source
        {
            get { throw new NotImplementedException(); }
        }

        public TVertex Target
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region Members

        [ContractInvariantMethod]
        private void UndirectedEdgeInvariant()
        {
            IUndirectedEdge<TVertex> iThis = this;
            Contract.Invariant(Comparer<TVertex>.Default.Compare(iThis.Source, iThis.Target) <= 0);
        }

        #endregion
    }
}