using System.Diagnostics.Contracts;

namespace PS.Graph.Contracts
{
    [ContractClassFor(typeof(IEdge<>))]
    internal abstract class EdgeContract<TVertex> : IEdge<TVertex>
    {
        #region IEdge<TVertex> Members

        TVertex IEdge<TVertex>.Source
        {
            get
            {
                Contract.Ensures(Contract.Result<TVertex>() != null);
                return default;
            }
        }

        TVertex IEdge<TVertex>.Target
        {
            get
            {
                Contract.Ensures(Contract.Result<TVertex>() != null);
                return default;
            }
        }

        #endregion

        #region Members

        [ContractInvariantMethod]
        private void EdgeInvariant()
        {
            IEdge<TVertex> iThis = this;
            Contract.Invariant(iThis.Source != null);
            Contract.Invariant(iThis.Target != null);
        }

        #endregion
    }
}