using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace PS.Graph.Contracts
{
    [ContractClassFor(typeof(IVertexSet<>))]
    internal abstract class VertexSetContract<TVertex> : IVertexSet<TVertex>
    {
        #region IVertexSet<TVertex> Members

        bool IVertexSet<TVertex>.IsVerticesEmpty
        {
            get
            {
                IVertexSet<TVertex> iThis = this;
                Contract.Ensures(Contract.Result<bool>() == (iThis.VertexCount == 0));

                return default;
            }
        }

        int IVertexSet<TVertex>.VertexCount
        {
            get
            {
                IVertexSet<TVertex> iThis = this;
                Contract.Ensures(Contract.Result<int>() == iThis.Vertices.Count());

                return default;
            }
        }

        IEnumerable<TVertex> IVertexSet<TVertex>.Vertices
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<TVertex>>() != null);

                return default;
            }
        }

        public bool ContainsVertex(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}