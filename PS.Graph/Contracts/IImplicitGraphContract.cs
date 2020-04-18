using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace PS.Graph.Contracts
{
    [ContractClassFor(typeof(IImplicitGraph<,>))]
    internal abstract class ImplicitGraphContract<TVertex, TEdge> : IImplicitGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region IImplicitGraph<TVertex,TEdge> Members

        [Pure]
        bool IImplicitGraph<TVertex, TEdge>.IsOutEdgesEmpty(TVertex v)
        {
            IImplicitGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(v != null);
            Contract.Requires(iThis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<bool>() == (iThis.OutDegree(v) == 0));

            return default;
        }

        [Pure]
        int IImplicitGraph<TVertex, TEdge>.OutDegree(TVertex v)
        {
            IImplicitGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(v != null);
            Contract.Requires(iThis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<int>() == iThis.OutEdges(v).Count());

            return default;
        }

        [Pure]
        IEnumerable<TEdge> IImplicitGraph<TVertex, TEdge>.OutEdges(TVertex v)
        {
            IImplicitGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(v != null);
            Contract.Requires(iThis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<IEnumerable<TEdge>>() != null);
            Contract.Ensures(Contract.Result<IEnumerable<TEdge>>().All(e => e.Source.Equals(v)));

            return default;
        }

        [Pure]
        bool IImplicitGraph<TVertex, TEdge>.TryGetOutEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            IImplicitGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(v != null);
            Contract.Ensures(!Contract.Result<bool>() ||
                             Contract.ValueAtReturn(out edges) != null &&
                             Contract.ValueAtReturn(out edges).All(e => e.Source.Equals(v))
            );

            edges = null;
            return default;
        }

        [Pure]
        TEdge IImplicitGraph<TVertex, TEdge>.OutEdge(TVertex v, int index)
        {
            IImplicitGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(v != null);
            Contract.Requires(iThis.ContainsVertex(v));
            Contract.Requires(index >= 0 && index < iThis.OutDegree(v));
            Contract.Ensures(
                iThis.OutEdges(v).ElementAt(index).Equals(Contract.Result<TEdge>()));

            return default;
        }

        bool IImplicitVertexSet<TVertex>.ContainsVertex(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        public bool IsDirected
        {
            get { throw new NotImplementedException(); }
        }

        public bool AllowParallelEdges
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}