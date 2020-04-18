using System.Diagnostics.Contracts;

namespace PS.Graph.Contracts
{
    [ContractClassFor(typeof(IImplicitVertexSet<>))]
    internal abstract class ImplicitVertexSetContract<TVertex> : IImplicitVertexSet<TVertex>
    {
        #region IImplicitVertexSet<TVertex> Members

        [Pure]
        bool IImplicitVertexSet<TVertex>.ContainsVertex(TVertex vertex)
        {
            Contract.Requires(vertex != null);

            return default;
        }

        #endregion
    }
}