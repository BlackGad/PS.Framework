using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace PS.Graph.Contracts
{
    [ContractClassFor(typeof(IEdgeSet<,>))]
    internal abstract class EdgeSetContract<TVertex, TEdge> : IEdgeSet<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region IEdgeSet<TVertex,TEdge> Members

        bool IEdgeSet<TVertex, TEdge>.IsEdgesEmpty
        {
            get
            {
                IEdgeSet<TVertex, TEdge> iThis = this;
                Contract.Ensures(Contract.Result<bool>() == (iThis.EdgeCount == 0));

                return default;
            }
        }

        int IEdgeSet<TVertex, TEdge>.EdgeCount
        {
            get
            {
                IEdgeSet<TVertex, TEdge> iThis = this;
                Contract.Ensures(Contract.Result<int>() == iThis.Edges.Count());

                return default;
            }
        }

        IEnumerable<TEdge> IEdgeSet<TVertex, TEdge>.Edges
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<TEdge>>() != null);
                Contract.Ensures(Contract.Result<IEnumerable<TEdge>>().All(e => e != null));

                return default;
            }
        }

        [Pure]
        bool IEdgeSet<TVertex, TEdge>.ContainsEdge(TEdge edge)
        {
            IEdgeSet<TVertex, TEdge> iThis = this;
            Contract.Requires(edge != null);
            Contract.Ensures(Contract.Result<bool>() == Contract.Exists(iThis.Edges, e => e.Equals(edge)));

            return default;
        }

        #endregion
    }
}