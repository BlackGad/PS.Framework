using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace PS.Graph.Contracts
{
    [ContractClassFor(typeof(IMutableIncidenceGraph<,>))]
    internal abstract class MutableIncidenceGraphContract<TVertex, TEdge> : IMutableIncidenceGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region IMutableIncidenceGraph<TVertex,TEdge> Members

        int IMutableIncidenceGraph<TVertex, TEdge>.RemoveOutEdgeIf(
            TVertex v,
            EdgePredicate<TVertex, TEdge> predicate)
        {
            IMutableIncidenceGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(v != null);
            Contract.Requires(iThis.ContainsVertex(v));
            Contract.Requires(predicate != null);
            Contract.Ensures(Contract.Result<int>() == Contract.OldValue(iThis.OutEdges(v).Count(ve => predicate(ve))));
            Contract.Ensures(iThis.OutEdges(v).All(ve => !predicate(ve)));

            return default;
        }

        void IMutableIncidenceGraph<TVertex, TEdge>.ClearOutEdges(TVertex v)
        {
            IMutableIncidenceGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(v != null);
            Contract.Requires(iThis.ContainsVertex(v));
            Contract.Ensures(iThis.OutDegree(v) == 0);
        }

        void IMutableIncidenceGraph<TVertex, TEdge>.TrimEdgeExcess()
        {
        }

        void IMutableGraph.Clear()
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

        bool IImplicitVertexSet<TVertex>.ContainsVertex(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        event EventHandler IMutableGraph.Cleared
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        #endregion
    }
}