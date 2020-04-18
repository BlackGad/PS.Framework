using System;
using System.Collections.Generic;
using System.Linq;

namespace PS.Graph.Algorithms.RandomWalks
{
    [Serializable]
    public sealed class VanishingWeightedMarkovEdgeChain<TVertex, TEdge> : WeightedMarkovEdgeChainBase<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private double _factor;

        #region Constructors

        public VanishingWeightedMarkovEdgeChain(IDictionary<TEdge, double> weights)
            : this(weights, 0.2)
        {
        }

        public VanishingWeightedMarkovEdgeChain(IDictionary<TEdge, double> weights, double factor)
            : base(weights)
        {
            _factor = factor;
        }

        #endregion

        #region Properties

        public double Factor
        {
            get { return _factor; }
            set { _factor = value; }
        }

        #endregion

        #region Override members

        public override bool TryGetSuccessor(IImplicitGraph<TVertex, TEdge> g, TVertex u, out TEdge successor)
        {
            if (!g.IsOutEdgesEmpty(u))
            {
                // get outWeight
                var outWeight = GetOutWeight(g, u);
                // get successor
                if (TryGetSuccessor(g, u, Rand.NextDouble() * outWeight, out var s))
                {
                    // update probabilities
                    Weights[s] *= Factor;

                    // normalize
                    foreach (var e in g.OutEdges(u))
                    {
                        Weights[e] /= outWeight;
                    }

                    successor = s;
                    return true;
                }
            }

            successor = default;
            return false;
        }

        public override bool TryGetSuccessor(IEnumerable<TEdge> edges, TVertex u, out TEdge successor)
        {
            var edgeList = edges.ToList();
            // get outWeight
            var outWeight = GetWeights(edgeList);
            // get successor
            if (TryGetSuccessor(edgeList, Rand.NextDouble() * outWeight, out var s))
            {
                // update probabilities
                Weights[s] *= Factor;

                // normalize
                foreach (var e in edgeList)
                {
                    Weights[e] /= outWeight;
                }

                successor = s;
                return true;
            }

            successor = default;
            return false;
        }

        #endregion
    }
}