using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace PS.Graph.Contracts
{
    [ContractClassFor(typeof(IMutableEdgeListGraph<,>))]
    internal abstract class MutableEdgeListGraphContract<TVertex, TEdge> : IMutableEdgeListGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region IMutableEdgeListGraph<TVertex,TEdge> Members

        bool IMutableEdgeListGraph<TVertex, TEdge>.AddEdge(TEdge e)
        {
            IMutableEdgeListGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(e != null);
            Contract.Requires(iThis.ContainsVertex(e.Source));
            Contract.Requires(iThis.ContainsVertex(e.Target));
            Contract.Ensures(iThis.ContainsEdge(e));
            Contract.Ensures(iThis.AllowParallelEdges || Contract.Result<bool>() == Contract.OldValue(!iThis.ContainsEdge(e)));
            Contract.Ensures(iThis.EdgeCount == Contract.OldValue(iThis.EdgeCount) + (Contract.Result<bool>() ? 1 : 0));

            return default;
        }

        event EdgeAction<TVertex, TEdge> IMutableEdgeListGraph<TVertex, TEdge>.EdgeAdded
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        int IMutableEdgeListGraph<TVertex, TEdge>.AddEdgeRange(IEnumerable<TEdge> edges)
        {
            IMutableEdgeListGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(edges != null);
            Contract.Requires(typeof(TEdge).IsValueType || edges.All(edge => edge != null));
            Contract.Requires(edges.All(edge =>
                                            iThis.ContainsVertex(edge.Source) &&
                                            iThis.ContainsVertex(edge.Target)
                              ));
            Contract.Ensures(edges.All(edge => iThis.ContainsEdge(edge)), "all edge from edges belong to the graph");
            Contract.Ensures(
                Contract.Result<int>() == Contract.OldValue(edges.Count(edge => !iThis.ContainsEdge(edge))));
            Contract.Ensures(iThis.EdgeCount == Contract.OldValue(iThis.EdgeCount) + Contract.Result<int>());

            return default;
        }

        bool IMutableEdgeListGraph<TVertex, TEdge>.RemoveEdge(TEdge e)
        {
            IMutableEdgeListGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(e != null);
            Contract.Ensures(Contract.Result<bool>() == Contract.OldValue(iThis.ContainsEdge(e)));
            Contract.Ensures(!iThis.ContainsEdge(e));
            Contract.Ensures(iThis.EdgeCount == Contract.OldValue(iThis.EdgeCount) - (Contract.Result<bool>() ? 1 : 0));

            return default;
        }

        event EdgeAction<TVertex, TEdge> IMutableEdgeListGraph<TVertex, TEdge>.EdgeRemoved
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        int IMutableEdgeListGraph<TVertex, TEdge>.RemoveEdgeIf(EdgePredicate<TVertex, TEdge> predicate)
        {
            IMutableEdgeListGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(predicate != null);
            Contract.Ensures(Contract.Result<int>() == Contract.OldValue(iThis.Edges.Count(e => predicate(e))));
            Contract.Ensures(iThis.Edges.All(e => !predicate(e)));
            Contract.Ensures(iThis.EdgeCount == Contract.OldValue(iThis.EdgeCount) - Contract.Result<int>());

            return default;
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

        bool IEdgeSet<TVertex, TEdge>.IsEdgesEmpty
        {
            get { throw new NotImplementedException(); }
        }

        int IEdgeSet<TVertex, TEdge>.EdgeCount
        {
            get { throw new NotImplementedException(); }
        }

        IEnumerable<TEdge> IEdgeSet<TVertex, TEdge>.Edges
        {
            get { throw new NotImplementedException(); }
        }

        bool IEdgeSet<TVertex, TEdge>.ContainsEdge(TEdge edge)
        {
            throw new NotImplementedException();
        }

        bool IVertexSet<TVertex>.IsVerticesEmpty
        {
            get { throw new NotImplementedException(); }
        }

        int IVertexSet<TVertex>.VertexCount
        {
            get { throw new NotImplementedException(); }
        }

        IEnumerable<TVertex> IVertexSet<TVertex>.Vertices
        {
            get { throw new NotImplementedException(); }
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