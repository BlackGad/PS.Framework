using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace PS.Graph.Contracts
{
    [ContractClassFor(typeof(IMutableVertexSet<>))]
    internal abstract class MutableVertexSetContract<TVertex> : IMutableVertexSet<TVertex>
    {
        #region IMutableVertexSet<TVertex> Members

        event VertexAction<TVertex> IMutableVertexSet<TVertex>.VertexAdded
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        bool IMutableVertexSet<TVertex>.AddVertex(TVertex v)
        {
            IMutableVertexSet<TVertex> iThis = this;
            Contract.Requires(v != null);
            Contract.Ensures(Contract.Result<bool>() == Contract.OldValue(!iThis.ContainsVertex(v)));
            Contract.Ensures(iThis.ContainsVertex(v));
            Contract.Ensures(iThis.VertexCount == Contract.OldValue(iThis.VertexCount) + (Contract.Result<bool>() ? 1 : 0));

            return default;
        }

        int IMutableVertexSet<TVertex>.AddVertexRange(IEnumerable<TVertex> vertices)
        {
            IMutableVertexSet<TVertex> iThis = this;
            Contract.Requires(vertices != null);
            Contract.Requires(vertices.All(v => v != null));
            Contract.Ensures(vertices.All(v => iThis.ContainsVertex(v)));
            Contract.Ensures(iThis.VertexCount == Contract.OldValue(iThis.VertexCount) + Contract.Result<int>());

            return default;
        }

        event VertexAction<TVertex> IMutableVertexSet<TVertex>.VertexRemoved
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        bool IMutableVertexSet<TVertex>.RemoveVertex(TVertex v)
        {
            IMutableVertexSet<TVertex> iThis = this;
            Contract.Requires(v != null);
            Contract.Ensures(Contract.Result<bool>() == Contract.OldValue(iThis.ContainsVertex(v)));
            Contract.Ensures(!iThis.ContainsVertex(v));
            Contract.Ensures(iThis.VertexCount == Contract.OldValue(iThis.VertexCount) - (Contract.Result<bool>() ? 1 : 0));

            return default;
        }

        int IMutableVertexSet<TVertex>.RemoveVertexIf(VertexPredicate<TVertex> predicate)
        {
            IMutableVertexSet<TVertex> iThis = this;
            Contract.Requires(predicate != null);
            Contract.Ensures(Contract.Result<int>() == Contract.OldValue(iThis.Vertices.Count(v => predicate(v))));
            Contract.Ensures(iThis.Vertices.All(v => !predicate(v)));
            Contract.Ensures(iThis.VertexCount == Contract.OldValue(iThis.VertexCount) - Contract.Result<int>());

            return default;
        }

        public bool IsVerticesEmpty
        {
            get { throw new NotImplementedException(); }
        }

        public int VertexCount
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<TVertex> Vertices
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