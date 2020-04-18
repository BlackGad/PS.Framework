using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace PS.Graph.Contracts
{
    [ContractClassFor(typeof(IMutableBidirectionalGraph<,>))]
    internal abstract class MutableBidirectionalGraphContract<TVertex, TEdge> : IMutableBidirectionalGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Properties

        public bool AllowParallelEdges
        {
            get { throw new NotImplementedException(); }
        }

        public int EdgeCount
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<TEdge> Edges
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsDirected
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsEdgesEmpty
        {
            get { throw new NotImplementedException(); }
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

        #endregion

        #region IMutableBidirectionalGraph<TVertex,TEdge> Members

        int IMutableBidirectionalGraph<TVertex, TEdge>.RemoveInEdgeIf(TVertex v, EdgePredicate<TVertex, TEdge> predicate)
        {
            IMutableBidirectionalGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(v != null);
            Contract.Requires(predicate != null);
            Contract.Requires(iThis.ContainsVertex(v));
            Contract.Ensures(iThis.ContainsVertex(v));
            Contract.Ensures(iThis.InEdges(v).All(e => predicate(e)));
            Contract.Ensures(Contract.Result<int>() == Contract.OldValue(iThis.InEdges(v).Count(e => predicate(e))));
            Contract.Ensures(iThis.InDegree(v) == Contract.OldValue(iThis.InDegree(v)) - Contract.Result<int>());

            return default;
        }

        void IMutableBidirectionalGraph<TVertex, TEdge>.ClearInEdges(TVertex v)
        {
            IMutableBidirectionalGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(v != null);
            Contract.Requires(iThis.ContainsVertex(v));
            Contract.Ensures(iThis.EdgeCount == Contract.OldValue(iThis.EdgeCount) - Contract.OldValue(iThis.InDegree(v)));
            Contract.Ensures(iThis.InDegree(v) == 0);
        }

        void IMutableBidirectionalGraph<TVertex, TEdge>.ClearEdges(TVertex v)
        {
            IMutableBidirectionalGraph<TVertex, TEdge> iThis = this;
            Contract.Requires(v != null);
            Contract.Requires(iThis.ContainsVertex(v));
            Contract.Ensures(!iThis.ContainsVertex(v));
        }

        bool IMutableVertexAndEdgeSet<TVertex, TEdge>.AddVerticesAndEdge(TEdge edge)
        {
            throw new NotImplementedException();
        }

        int IMutableVertexAndEdgeSet<TVertex, TEdge>.AddVerticesAndEdgeRange(IEnumerable<TEdge> edges)
        {
            throw new NotImplementedException();
        }

        int IMutableIncidenceGraph<TVertex, TEdge>.RemoveOutEdgeIf(TVertex v, EdgePredicate<TVertex, TEdge> predicate)
        {
            throw new NotImplementedException();
        }

        void IMutableIncidenceGraph<TVertex, TEdge>.ClearOutEdges(TVertex v)
        {
            throw new NotImplementedException();
        }

        void IMutableIncidenceGraph<TVertex, TEdge>.TrimEdgeExcess()
        {
            throw new NotImplementedException();
        }

        void IMutableGraph.Clear()
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

        event EventHandler IMutableGraph.Cleared
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
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

        bool IBidirectionalIncidenceGraph<TVertex, TEdge>.IsInEdgesEmpty(TVertex v)
        {
            throw new NotImplementedException();
        }

        int IBidirectionalIncidenceGraph<TVertex, TEdge>.InDegree(TVertex v)
        {
            throw new NotImplementedException();
        }

        IEnumerable<TEdge> IBidirectionalIncidenceGraph<TVertex, TEdge>.InEdges(TVertex v)
        {
            throw new NotImplementedException();
        }

        bool IBidirectionalIncidenceGraph<TVertex, TEdge>.TryGetInEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            throw new NotImplementedException();
        }

        TEdge IBidirectionalIncidenceGraph<TVertex, TEdge>.InEdge(TVertex v, int index)
        {
            throw new NotImplementedException();
        }

        int IBidirectionalIncidenceGraph<TVertex, TEdge>.Degree(TVertex v)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Members

        public bool ContainsEdge(TVertex source, TVertex target)
        {
            throw new NotImplementedException();
        }

        public bool ContainsEdge(TEdge edge)
        {
            throw new NotImplementedException();
        }

        public bool ContainsVertex(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        public int Degree(TVertex v)
        {
            throw new NotImplementedException();
        }

        public int InDegree(TVertex v)
        {
            throw new NotImplementedException();
        }

        public TEdge InEdge(TVertex v, int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEdge> InEdges(TVertex v)
        {
            throw new NotImplementedException();
        }

        public bool IsInEdgesEmpty(TVertex v)
        {
            throw new NotImplementedException();
        }

        public bool IsOutEdgesEmpty(TVertex v)
        {
            throw new NotImplementedException();
        }

        public int OutDegree(TVertex v)
        {
            throw new NotImplementedException();
        }

        public TEdge OutEdge(TVertex v, int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEdge> OutEdges(TVertex v)
        {
            throw new NotImplementedException();
        }

        public bool TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            throw new NotImplementedException();
        }

        public bool TryGetEdges(TVertex source, TVertex target, out IEnumerable<TEdge> edges)
        {
            throw new NotImplementedException();
        }

        public bool TryGetInEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            throw new NotImplementedException();
        }

        public bool TryGetOutEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}