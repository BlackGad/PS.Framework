using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace PS.Graph.Contracts
{
    [ContractClassFor(typeof(IImplicitUndirectedGraph<,>))]
    internal abstract class ImplicitUndirectedGraphContract<TVertex, TEdge> : IImplicitUndirectedGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region IImplicitUndirectedGraph<TVertex,TEdge> Members

        [Pure]
        EdgeEqualityComparer<TVertex, TEdge> IImplicitUndirectedGraph<TVertex, TEdge>.EdgeEqualityComparer
        {
            get
            {
                Contract.Ensures(Contract.Result<EdgeEqualityComparer<TVertex, TEdge>>() != null);
                return null;
            }
        }

        [Pure]
        IEnumerable<TEdge> IImplicitUndirectedGraph<TVertex, TEdge>.AdjacentEdges(TVertex v)
        {
            IImplicitUndirectedGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(v != null);
            Contract.Requires(iThis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<IEnumerable<TEdge>>() != null);
            Contract.Ensures(
                Contract.Result<IEnumerable<TEdge>>().All(edge =>
                                                              edge != null &&
                                                              iThis.ContainsEdge(edge.Source, edge.Target) &&
                                                              (edge.Source.Equals(v) || edge.Target.Equals(v))
                )
            );

            return default;
        }

        [Pure]
        int IImplicitUndirectedGraph<TVertex, TEdge>.AdjacentDegree(TVertex v)
        {
            IImplicitUndirectedGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(v != null);
            Contract.Requires(iThis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<int>() == iThis.AdjacentEdges(v).Count());

            return default;
        }

        [Pure]
        bool IImplicitUndirectedGraph<TVertex, TEdge>.IsAdjacentEdgesEmpty(TVertex v)
        {
            IImplicitUndirectedGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(v != null);
            Contract.Requires(iThis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<bool>() == (iThis.AdjacentDegree(v) == 0));

            return default;
        }

        [Pure]
        TEdge IImplicitUndirectedGraph<TVertex, TEdge>.AdjacentEdge(TVertex v, int index)
        {
            IImplicitUndirectedGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(v != null);
            Contract.Requires(iThis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<TEdge>() != null);
            Contract.Ensures(
                Contract.Result<TEdge>().Source.Equals(v)
                || Contract.Result<TEdge>().Target.Equals(v));

            return default;
        }

        [Pure]
        bool IImplicitUndirectedGraph<TVertex, TEdge>.TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            IImplicitUndirectedGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(source != null);
            Contract.Requires(target != null);

            edge = default;
            return default;
        }

        [Pure]
        bool IImplicitUndirectedGraph<TVertex, TEdge>.ContainsEdge(TVertex source, TVertex target)
        {
            IImplicitUndirectedGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(source != null);
            Contract.Requires(target != null);
            Contract.Ensures(Contract.Result<bool>() == iThis.AdjacentEdges(source).Any(e => e.Target.Equals(target) || e.Source.Equals(target)));

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