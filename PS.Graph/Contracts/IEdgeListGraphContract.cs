using System;
using System.Diagnostics.Contracts;

namespace PS.Graph.Contracts
{
    [ContractClassFor(typeof(IEdgeListGraph<,>))]
    internal abstract class EdgeListGraphContract<TVertex, TEdge> : IEdgeListGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region IEdgeListGraph<TVertex,TEdge> Members

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

        System.Collections.Generic.IEnumerable<TEdge> IEdgeSet<TVertex, TEdge>.Edges
        {
            get { throw new NotImplementedException(); }
        }

        bool IEdgeSet<TVertex, TEdge>.ContainsEdge(TEdge edge)
        {
            throw new NotImplementedException();
        }

        public bool IsVerticesEmpty
        {
            get { throw new NotImplementedException(); }
        }

        public int VertexCount
        {
            get { throw new NotImplementedException(); }
        }

        public System.Collections.Generic.IEnumerable<TVertex> Vertices
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