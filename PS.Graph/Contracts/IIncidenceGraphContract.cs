using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace PS.Graph.Contracts
{
    [ContractClassFor(typeof(IIncidenceGraph<,>))]
    internal abstract class IncidenceGraphContract<TVertex, TEdge> : IIncidenceGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region IIncidenceGraph<TVertex,TEdge> Members

        bool IIncidenceGraph<TVertex, TEdge>.ContainsEdge(TVertex source, TVertex target)
        {
            IIncidenceGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(source != null);
            Contract.Requires(target != null);
            Contract.Requires(iThis.ContainsVertex(source));
            Contract.Requires(iThis.ContainsVertex(target));

            return default;
        }

        bool IIncidenceGraph<TVertex, TEdge>.TryGetEdges(
            TVertex source,
            TVertex target,
            out IEnumerable<TEdge> edges)
        {
            IIncidenceGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(source != null);
            Contract.Requires(target != null);
            Contract.Requires(iThis.ContainsVertex(source));
            Contract.Requires(iThis.ContainsVertex(target));

            edges = null;
            return default;
        }

        bool IIncidenceGraph<TVertex, TEdge>.TryGetEdge(
            TVertex source,
            TVertex target,
            out TEdge edge)
        {
            IIncidenceGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(source != null);
            Contract.Requires(target != null);
            Contract.Requires(iThis.ContainsVertex(source));
            Contract.Requires(iThis.ContainsVertex(target));

            edge = default;
            return default;
        }

        public bool IsOutEdgesEmpty(TVertex v)
        {
            throw new NotImplementedException();
        }

        public int OutDegree(TVertex v)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEdge> OutEdges(TVertex v)
        {
            throw new NotImplementedException();
        }

        public bool TryGetOutEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            throw new NotImplementedException();
        }

        public TEdge OutEdge(TVertex v, int index)
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

        public bool ContainsVertex(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}