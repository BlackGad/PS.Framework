using System;
using System.Collections.Generic;

namespace PS.Graph
{
    /// <summary>
    ///     A delegate-based incidence graph
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    [Serializable]
    public class DelegateIncidenceGraph<TVertex, TEdge> : DelegateImplicitGraph<TVertex, TEdge>,
                                                          IIncidenceGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>, IEquatable<TEdge>
    {
        #region Constructors

        public DelegateIncidenceGraph(
            TryFunc<TVertex, IEnumerable<TEdge>> tryGetOutEdges)
            : base(tryGetOutEdges)
        {
        }

        #endregion

        #region IIncidenceGraph<TVertex,TEdge> Members

        public bool ContainsEdge(TVertex source, TVertex target)
        {
            return TryGetEdge(source, target, out _);
        }

        public bool TryGetEdges(TVertex source, TVertex target, out IEnumerable<TEdge> edges)
        {
            List<TEdge> result = null;
            if (TryGetOutEdges(source, out var outEdges))
            {
                foreach (var e in outEdges)
                {
                    if (e.Target.Equals(target))
                    {
                        if (result == null)
                        {
                            result = new List<TEdge>();
                        }

                        result.Add(e);
                    }
                }
            }

            edges = result?.ToArray();
            return edges != null;
        }

        public bool TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            if (TryGetOutEdges(source, out var edges))
            {
                foreach (var e in edges)
                {
                    if (e.Target.Equals(target))
                    {
                        edge = e;
                        return true;
                    }
                }
            }

            edge = default;
            return false;
        }

        #endregion
    }
}