using System;
using System.Collections.Generic;

namespace PS.Graph.Algorithms.RandomWalks
{
    [Serializable]
    public abstract class WeightedMarkovEdgeChainBase<TVertex, TEdge> : MarkovEdgeChainBase<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private IDictionary<TEdge, double> _weights;

        #region Constructors

        protected WeightedMarkovEdgeChainBase(IDictionary<TEdge, double> weights)
        {
            _weights = weights;
        }

        #endregion

        #region Properties

        public IDictionary<TEdge, double> Weights
        {
            get { return _weights; }
            set { _weights = value; }
        }

        #endregion

        #region Members

        protected double GetOutWeight(IImplicitGraph<TVertex, TEdge> g, TVertex u)
        {
            var edges = g.OutEdges(u);
            return GetWeights(edges);
        }

        protected double GetWeights(IEnumerable<TEdge> edges)
        {
            double outWeight = 0;
            foreach (var e in edges)
            {
                outWeight += _weights[e];
            }

            return outWeight;
        }

        protected bool TryGetSuccessor(IImplicitGraph<TVertex, TEdge> g, TVertex u, double position, out TEdge successor)
        {
            var edges = g.OutEdges(u);
            return TryGetSuccessor(edges, position, out successor);
        }

        protected bool TryGetSuccessor(IEnumerable<TEdge> edges, double position, out TEdge successor)
        {
            double pos = 0;
            foreach (var e in edges)
            {
                var nextPos = pos + _weights[e];
                if (position >= pos && position <= nextPos)
                {
                    successor = e;
                    return true;
                }

                pos = nextPos;
            }

            successor = default;
            return false;
        }

        #endregion
    }
}