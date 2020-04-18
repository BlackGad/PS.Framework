using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace PS.Graph
{
    /// <summary>
    ///     A delegate based bidirectional implicit graph
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    [Serializable]
    public class DelegateBidirectionalIncidenceGraph<TVertex, TEdge> : DelegateIncidenceGraph<TVertex, TEdge>,
                                                                       IBidirectionalIncidenceGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>, IEquatable<TEdge>
    {
        #region Constructors

        public DelegateBidirectionalIncidenceGraph(
            TryFunc<TVertex, IEnumerable<TEdge>> tryGetOutEdges,
            TryFunc<TVertex, IEnumerable<TEdge>> tryGetInEdges)
            : base(tryGetOutEdges)
        {
            Contract.Requires(tryGetInEdges != null);

            TryGetInEdgesFunc = tryGetInEdges;
        }

        #endregion

        #region Properties

        public TryFunc<TVertex, IEnumerable<TEdge>> TryGetInEdgesFunc { get; }

        #endregion

        #region IBidirectionalIncidenceGraph<TVertex,TEdge> Members

        public bool IsInEdgesEmpty(TVertex v)
        {
            return !InEdges(v).Any();
        }

        public int InDegree(TVertex v)
        {
            return InEdges(v).Count();
        }

        public IEnumerable<TEdge> InEdges(TVertex v)
        {
            if (!TryGetInEdgesFunc(v, out var result))
            {
                return Enumerable.Empty<TEdge>();
            }

            return result;
        }

        public bool TryGetInEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            return TryGetInEdgesFunc(v, out edges);
        }

        public TEdge InEdge(TVertex v, int index)
        {
            return InEdges(v).ElementAt(index);
        }

        public int Degree(TVertex v)
        {
            return InDegree(v) + OutDegree(v);
        }

        #endregion
    }
}