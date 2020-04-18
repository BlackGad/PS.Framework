using System;
using System.Collections.Generic;
using System.Linq;

namespace PS.Graph.Algorithms.RandomWalks
{
    [Serializable]
    public sealed class WeightedMarkovEdgeChain<TVertex, TEdge> : WeightedMarkovEdgeChainBase<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Constructors

        public WeightedMarkovEdgeChain(IDictionary<TEdge, double> weights)
            : base(weights)
        {
        }

        #endregion

        #region Override members

        public override bool TryGetSuccessor(IImplicitGraph<TVertex, TEdge> g, TVertex u, out TEdge successor)
        {
            // get number of out-edges
            var n = g.OutDegree(u);
            if (n > 0)
            {
                // compute out-edge su
                var outWeight = GetOutWeight(g, u);
                // scale and get next edge
                var r = Rand.NextDouble() * outWeight;
                return TryGetSuccessor(g, u, r, out successor);
            }

            successor = default;
            return false;
        }

        public override bool TryGetSuccessor(IEnumerable<TEdge> edges, TVertex u, out TEdge sucessor)
        {
            var edgeList = edges.ToList();
            // compute out-edge su
            var outWeight = GetWeights(edgeList);
            // scale and get next edge
            var r = Rand.NextDouble() * outWeight;
            return TryGetSuccessor(edgeList, r, out sucessor);
        }

        #endregion
    }
}