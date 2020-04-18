using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace PS.Graph.Contracts
{
    [ContractClassFor(typeof(IUndirectedGraph<,>))]
    internal abstract class UndirectedGraphContract<TVertex, TEdge> : IUndirectedGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region IUndirectedGraph<TVertex,TEdge> Members

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

        #endregion
    }
}