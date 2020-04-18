using System;
using System.Collections.Generic;
using System.Linq;

namespace PS.Graph
{
    /// <summary>
    ///     A delegate-based implicit graph
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    [Serializable]
    public class DelegateImplicitGraph<TVertex, TEdge> : IImplicitGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>, IEquatable<TEdge>
    {
        #region Constructors

        public DelegateImplicitGraph(
            TryFunc<TVertex, IEnumerable<TEdge>> tryGetOutEdges)
        {
            TryGetOutEdgesFunc = tryGetOutEdges;
        }

        #endregion

        #region Properties

        public TryFunc<TVertex, IEnumerable<TEdge>> TryGetOutEdgesFunc { get; }

        #endregion

        #region IImplicitGraph<TVertex,TEdge> Members

        public bool IsOutEdgesEmpty(TVertex v)
        {
            return !OutEdges(v).Any();
        }

        public int OutDegree(TVertex v)
        {
            return OutEdges(v).Count();
        }

        public IEnumerable<TEdge> OutEdges(TVertex v)
        {
            if (!TryGetOutEdgesFunc(v, out var result))
            {
                return Enumerable.Empty<TEdge>();
            }

            return result;
        }

        public bool TryGetOutEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            return TryGetOutEdgesFunc(v, out edges);
        }

        public TEdge OutEdge(TVertex v, int index)
        {
            return OutEdges(v).ElementAt(index);
        }

        public bool IsDirected
        {
            get { return true; }
        }

        public bool AllowParallelEdges
        {
            get { return true; }
        }

        public bool ContainsVertex(TVertex vertex)
        {
            return TryGetOutEdgesFunc(vertex, out _);
        }

        #endregion
    }
}