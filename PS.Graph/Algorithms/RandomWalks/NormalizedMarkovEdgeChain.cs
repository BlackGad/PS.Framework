using System;
using System.Collections.Generic;
using System.Linq;

namespace PS.Graph.Algorithms.RandomWalks
{
    [Serializable]
    public sealed class NormalizedMarkovEdgeChain<TVertex, TEdge> : MarkovEdgeChainBase<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Override members

        public override bool TryGetSuccessor(IImplicitGraph<TVertex, TEdge> g, TVertex u, out TEdge successor)
        {
            var outDegree = g.OutDegree(u);
            if (outDegree > 0)
            {
                var index = Rand.Next(0, outDegree);
                successor = g.OutEdge(u, index);
                return true;
            }

            successor = default;
            return false;
        }

        public override bool TryGetSuccessor(IEnumerable<TEdge> edges, TVertex u, out TEdge successor)
        {
            var edgeList = edges.ToList();
            var edgeCount = edgeList.Count;

            if (edgeCount > 0)
            {
                var index = Rand.Next(0, edgeCount);
                successor = edgeList.ElementAt(index);
                return true;
            }

            successor = default;
            return false;
        }

        #endregion
    }
}