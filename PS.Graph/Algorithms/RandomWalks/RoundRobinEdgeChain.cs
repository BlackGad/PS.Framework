using System;
using System.Collections.Generic;
using System.Linq;

namespace PS.Graph.Algorithms.RandomWalks
{
    [Serializable]
    public sealed class RoundRobinEdgeChain<TVertex, TEdge> : IEdgeChain<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private Dictionary<TVertex, int> _outEdgeIndices = new Dictionary<TVertex, int>();

        #region IEdgeChain<TVertex,TEdge> Members

        public bool TryGetSuccessor(IImplicitGraph<TVertex, TEdge> g, TVertex u, out TEdge successor)
        {
            var outDegree = g.OutDegree(u);
            if (outDegree > 0)
            {
                if (!_outEdgeIndices.TryGetValue(u, out var index))
                {
                    index = 0;
                    _outEdgeIndices.Add(u, index);
                }

                var e = g.OutEdge(u, index);
                _outEdgeIndices[u] = ++index % outDegree;

                successor = e;
                return true;
            }

            successor = default;
            return false;
        }

        public bool TryGetSuccessor(IEnumerable<TEdge> edges, TVertex u, out TEdge successor)
        {
            var edgeList = edges.ToList();
            var edgeCount = edgeList.Count;

            if (edgeCount > 0)
            {
                if (!_outEdgeIndices.TryGetValue(u, out var index))
                {
                    index = 0;
                    _outEdgeIndices.Add(u, index);
                }

                var e = edgeList.ElementAt(index);
                _outEdgeIndices[u] = ++index % edgeCount;
                successor = e;
                return true;
            }

            successor = default;
            return false;
        }

        #endregion
    }
}