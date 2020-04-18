using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace PS.Graph
{
    [Serializable]
    [DebuggerDisplay("VertexCount = {VertexCount}, EdgeCount = {EdgeCount}")]
    public sealed class UndirectedBidirectionalGraph<TVertex, TEdge> : IUndirectedGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Constructors

        public UndirectedBidirectionalGraph(IBidirectionalGraph<TVertex, TEdge> visitedGraph)
        {
            Contract.Requires(visitedGraph != null);

            VisitedGraph = visitedGraph;
        }

        #endregion

        #region Properties

        public IBidirectionalGraph<TVertex, TEdge> VisitedGraph { get; }

        #endregion

        #region IUndirectedGraph<TVertex,TEdge> Members

        public EdgeEqualityComparer<TVertex, TEdge> EdgeEqualityComparer { get; } = EdgeExtensions.GetUndirectedVertexEquality<TVertex, TEdge>();

        [Pure]
        public IEnumerable<TEdge> AdjacentEdges(TVertex v)
        {
            foreach (var e in VisitedGraph.OutEdges(v))
            {
                yield return e;
            }

            foreach (var e in VisitedGraph.InEdges(v))
            {
                // we skip self edges here since
                // we already did those in the outEdge run
                if (e.Source.Equals(e.Target))
                {
                    continue;
                }

                yield return e;
            }
        }

        [Pure]
        public int AdjacentDegree(TVertex v)
        {
            return VisitedGraph.Degree(v);
        }

        [Pure]
        public bool IsAdjacentEdgesEmpty(TVertex v)
        {
            return VisitedGraph.IsOutEdgesEmpty(v) && VisitedGraph.IsInEdgesEmpty(v);
        }

        [Pure]
        public TEdge AdjacentEdge(TVertex v, int index)
        {
            throw new NotSupportedException();
        }

        [Pure]
        public bool ContainsEdge(TVertex source, TVertex target)
        {
            throw new NotSupportedException();
        }

        public bool TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            throw new NotSupportedException();
        }

        public bool IsVerticesEmpty
        {
            get { return VisitedGraph.IsVerticesEmpty; }
        }

        public int VertexCount
        {
            get { return VisitedGraph.VertexCount; }
        }

        public IEnumerable<TVertex> Vertices
        {
            get { return VisitedGraph.Vertices; }
        }

        [Pure]
        public bool ContainsVertex(TVertex vertex)
        {
            return VisitedGraph.ContainsVertex(vertex);
        }

        public bool IsEdgesEmpty
        {
            get { return VisitedGraph.IsEdgesEmpty; }
        }

        public int EdgeCount
        {
            get { return VisitedGraph.EdgeCount; }
        }

        public IEnumerable<TEdge> Edges
        {
            get { return VisitedGraph.Edges; }
        }

        [Pure]
        public bool ContainsEdge(TEdge edge)
        {
            return VisitedGraph.ContainsEdge(edge);
        }

        public bool IsDirected
        {
            get { return false; }
        }

        public bool AllowParallelEdges
        {
            get { return VisitedGraph.AllowParallelEdges; }
        }

        #endregion
    }
}