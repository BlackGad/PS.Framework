﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace PS.Graph.Contracts
{
    [ContractClassFor(typeof(IMutableUndirectedGraph<,>))]
    internal abstract class MutableUndirectedGraphContract<TVertex, TEdge> : IMutableUndirectedGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region IMutableUndirectedGraph<TVertex,TEdge> Members

        int IMutableUndirectedGraph<TVertex, TEdge>.RemoveAdjacentEdgeIf(
            TVertex vertex,
            EdgePredicate<TVertex, TEdge> predicate)
        {
            IMutableUndirectedGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(vertex != null);
            Contract.Requires(predicate != null);
            Contract.Ensures(Contract.Result<int>() == Contract.OldValue(iThis.AdjacentEdges(vertex).Count(e => predicate(e))));
            Contract.Ensures(iThis.AdjacentEdges(vertex).All(v => !predicate(v)));

            return default;
        }

        void IMutableUndirectedGraph<TVertex, TEdge>.ClearAdjacentEdges(TVertex vertex)
        {
            IMutableUndirectedGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(vertex != null);
            Contract.Ensures(iThis.AdjacentDegree(vertex) == 0);
        }

        bool IMutableEdgeListGraph<TVertex, TEdge>.AddEdge(TEdge edge)
        {
            throw new NotImplementedException();
        }

        event EdgeAction<TVertex, TEdge> IMutableEdgeListGraph<TVertex, TEdge>.EdgeAdded
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        int IMutableEdgeListGraph<TVertex, TEdge>.AddEdgeRange(IEnumerable<TEdge> edges)
        {
            throw new NotImplementedException();
        }

        bool IMutableEdgeListGraph<TVertex, TEdge>.RemoveEdge(TEdge edge)
        {
            throw new NotImplementedException();
        }

        event EdgeAction<TVertex, TEdge> IMutableEdgeListGraph<TVertex, TEdge>.EdgeRemoved
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        int IMutableEdgeListGraph<TVertex, TEdge>.RemoveEdgeIf(EdgePredicate<TVertex, TEdge> predicate)
        {
            throw new NotImplementedException();
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

        event VertexAction<TVertex> IMutableVertexSet<TVertex>.VertexAdded
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        bool IMutableVertexSet<TVertex>.AddVertex(TVertex v)
        {
            throw new NotImplementedException();
        }

        int IMutableVertexSet<TVertex>.AddVertexRange(IEnumerable<TVertex> vertices)
        {
            throw new NotImplementedException();
        }

        event VertexAction<TVertex> IMutableVertexSet<TVertex>.VertexRemoved
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        bool IMutableVertexSet<TVertex>.RemoveVertex(TVertex v)
        {
            throw new NotImplementedException();
        }

        int IMutableVertexSet<TVertex>.RemoveVertexIf(VertexPredicate<TVertex> predicate)
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

        [Pure]
        EdgeEqualityComparer<TVertex, TEdge> IImplicitUndirectedGraph<TVertex, TEdge>.EdgeEqualityComparer
        {
            get { throw new NotImplementedException(); }
        }

        IEnumerable<TEdge> IImplicitUndirectedGraph<TVertex, TEdge>.AdjacentEdges(TVertex v)
        {
            throw new NotImplementedException();
        }

        int IImplicitUndirectedGraph<TVertex, TEdge>.AdjacentDegree(TVertex v)
        {
            throw new NotImplementedException();
        }

        bool IImplicitUndirectedGraph<TVertex, TEdge>.IsAdjacentEdgesEmpty(TVertex v)
        {
            throw new NotImplementedException();
        }

        TEdge IImplicitUndirectedGraph<TVertex, TEdge>.AdjacentEdge(TVertex v, int index)
        {
            throw new NotImplementedException();
        }

        bool IImplicitUndirectedGraph<TVertex, TEdge>.ContainsEdge(TVertex source, TVertex target)
        {
            throw new NotImplementedException();
        }

        bool IImplicitUndirectedGraph<TVertex, TEdge>.TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            throw new NotImplementedException();
        }

        bool IMutableVertexAndEdgeSet<TVertex, TEdge>.AddVerticesAndEdge(TEdge edge)
        {
            throw new NotImplementedException();
        }

        int IMutableVertexAndEdgeSet<TVertex, TEdge>.AddVerticesAndEdgeRange(IEnumerable<TEdge> edges)
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