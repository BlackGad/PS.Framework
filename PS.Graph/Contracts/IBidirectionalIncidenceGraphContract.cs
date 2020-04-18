using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace PS.Graph.Contracts
{
    [ContractClassFor(typeof(IBidirectionalIncidenceGraph<,>))]
    internal abstract class BidirectionalIncidenceGraphContract<TVertex, TEdge> : IBidirectionalIncidenceGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region IBidirectionalIncidenceGraph<TVertex,TEdge> Members

        [Pure]
        bool IBidirectionalIncidenceGraph<TVertex, TEdge>.IsInEdgesEmpty(TVertex v)
        {
            IBidirectionalIncidenceGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(v != null);
            Contract.Requires(iThis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<bool>() == (iThis.InDegree(v) == 0));

            return default;
        }

        [Pure]
        int IBidirectionalIncidenceGraph<TVertex, TEdge>.InDegree(TVertex v)
        {
            IBidirectionalIncidenceGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(v != null);
            Contract.Requires(iThis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<int>() == iThis.InEdges(v).Count());

            return default;
        }

        [Pure]
        IEnumerable<TEdge> IBidirectionalIncidenceGraph<TVertex, TEdge>.InEdges(TVertex v)
        {
            IBidirectionalIncidenceGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(v != null);
            Contract.Requires(iThis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<IEnumerable<TEdge>>() != null);
            Contract.Ensures(Contract.Result<IEnumerable<TEdge>>().All(edge => edge != null && edge.Target.Equals(v)
                             )
            );

            return default;
        }

        [Pure]
        bool IBidirectionalIncidenceGraph<TVertex, TEdge>.TryGetInEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            IBidirectionalIncidenceGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(v != null);
            Contract.Requires(iThis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<bool>() == iThis.ContainsVertex(v));
            Contract.Ensures(!Contract.Result<bool>() || Contract.ValueAtReturn(out edges) != null);
            Contract.Ensures(!Contract.Result<bool>() ||
                             Contract.ValueAtReturn(out edges).All(edge => edge != null && edge.Target.Equals(v)
                             )
            );

            edges = null;
            return default;
        }

        [Pure]
        TEdge IBidirectionalIncidenceGraph<TVertex, TEdge>.InEdge(TVertex v, int index)
        {
            IBidirectionalIncidenceGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(v != null);
            Contract.Requires(iThis.ContainsVertex(v));
            Contract.Requires(index >= 0 && index < iThis.InDegree(v));
            Contract.Ensures(Contract.Result<TEdge>().Equals(iThis.InEdges(v).ElementAt(index)));

            return default;
        }

        [Pure]
        int IBidirectionalIncidenceGraph<TVertex, TEdge>.Degree(TVertex v)
        {
            IBidirectionalIncidenceGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(v != null);
            Contract.Requires(iThis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<int>() == iThis.InDegree(v) + iThis.OutDegree(v));

            return default;
        }

        bool IImplicitGraph<TVertex, TEdge>.IsOutEdgesEmpty(TVertex v)
        {
            throw new NotImplementedException();
        }

        int IImplicitGraph<TVertex, TEdge>.OutDegree(TVertex v)
        {
            throw new NotImplementedException();
        }

        IEnumerable<TEdge> IImplicitGraph<TVertex, TEdge>.OutEdges(TVertex v)
        {
            throw new NotImplementedException();
        }

        bool IImplicitGraph<TVertex, TEdge>.TryGetOutEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            throw new NotImplementedException();
        }

        TEdge IImplicitGraph<TVertex, TEdge>.OutEdge(TVertex v, int index)
        {
            throw new NotImplementedException();
        }

        bool IGraph.IsDirected
        {
            get { throw new NotImplementedException(); }
        }

        bool IGraph.AllowParallelEdges
        {
            get { throw new NotImplementedException(); }
        }

        bool IImplicitVertexSet<TVertex>.ContainsVertex(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        bool IIncidenceGraph<TVertex, TEdge>.ContainsEdge(TVertex source, TVertex target)
        {
            throw new NotImplementedException();
        }

        bool IIncidenceGraph<TVertex, TEdge>.TryGetEdges(TVertex source, TVertex target, out IEnumerable<TEdge> edges)
        {
            throw new NotImplementedException();
        }

        bool IIncidenceGraph<TVertex, TEdge>.TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}