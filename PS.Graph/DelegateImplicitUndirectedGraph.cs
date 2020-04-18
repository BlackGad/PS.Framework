using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace PS.Graph
{
    /// <summary>
    ///     A functional implicit undirected graph
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    [Serializable]
    public class DelegateImplicitUndirectedGraph<TVertex, TEdge> : IImplicitUndirectedGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Constructors

        public DelegateImplicitUndirectedGraph(
            TryFunc<TVertex, IEnumerable<TEdge>> tryGetAdjacencyEdges,
            bool allowParallelEdges)
        {
            Contract.Requires(tryGetAdjacencyEdges != null);

            TryGetAdjacencyEdgesFunc = tryGetAdjacencyEdges;
            AllowParallelEdges = allowParallelEdges;
        }

        #endregion

        #region Properties

        public TryFunc<TVertex, IEnumerable<TEdge>> TryGetAdjacencyEdgesFunc { get; }

        #endregion

        #region IImplicitUndirectedGraph<TVertex,TEdge> Members

        public EdgeEqualityComparer<TVertex, TEdge> EdgeEqualityComparer { get; } = EdgeExtensions.GetUndirectedVertexEquality<TVertex, TEdge>();

        public bool IsAdjacentEdgesEmpty(TVertex v)
        {
            return !AdjacentEdges(v).Any();
        }

        public int AdjacentDegree(TVertex v)
        {
            return AdjacentEdges(v).Count();
        }

        public IEnumerable<TEdge> AdjacentEdges(TVertex v)
        {
            if (!TryGetAdjacencyEdgesFunc(v, out var result))
            {
                return Enumerable.Empty<TEdge>();
            }

            return result;
        }

        public TEdge AdjacentEdge(TVertex v, int index)
        {
            return AdjacentEdges(v).ElementAt(index);
        }

        public bool IsDirected
        {
            get { return false; }
        }

        public bool AllowParallelEdges { get; }

        public bool ContainsVertex(TVertex vertex)
        {
            return
                TryGetAdjacencyEdgesFunc(vertex, out _);
        }

        public bool TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            if (TryGetAdjacentEdges(source, out var edges))
            {
                foreach (var e in edges)
                {
                    if (EdgeEqualityComparer(e, source, target))
                    {
                        edge = e;
                        return true;
                    }
                }
            }

            edge = default;
            return false;
        }

        public bool ContainsEdge(TVertex source, TVertex target)
        {
            return TryGetEdge(source, target, out _);
        }

        #endregion

        #region Members

        public bool TryGetAdjacentEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            return TryGetAdjacencyEdgesFunc(v, out edges);
        }

        #endregion
    }
}