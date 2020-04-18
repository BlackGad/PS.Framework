using System;
using System.Diagnostics.Contracts;

namespace PS.Graph.Contracts
{
    [ContractClassFor(typeof(ICloneableEdge<,>))]
    internal abstract class CloneableEdgeContract<TVertex, TEdge> : ICloneableEdge<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region ICloneableEdge<TVertex,TEdge> Members

        TEdge ICloneableEdge<TVertex, TEdge>.Clone(TVertex source, TVertex target)
        {
            Contract.Requires(source != null);
            Contract.Requires(target != null);
            Contract.Ensures(Contract.Result<TEdge>() != null);
            Contract.Ensures(Contract.Result<TEdge>().Source.Equals(source));
            Contract.Ensures(Contract.Result<TEdge>().Target.Equals(target));

            return default;
        }

        TVertex IEdge<TVertex>.Source
        {
            get { throw new NotImplementedException(); }
        }

        TVertex IEdge<TVertex>.Target
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}