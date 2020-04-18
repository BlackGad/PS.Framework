using System;
using System.Collections.Generic;

namespace PS.Graph.Algorithms.RandomWalks
{
    [Serializable]
    public abstract class MarkovEdgeChainBase<TVertex, TEdge> : IMarkovEdgeChain<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private Random _rand = new Random();

        #region IMarkovEdgeChain<TVertex,TEdge> Members

        public Random Rand
        {
            get { return _rand; }
            set { _rand = value; }
        }

        public abstract bool TryGetSuccessor(IImplicitGraph<TVertex, TEdge> g, TVertex u, out TEdge successor);
        public abstract bool TryGetSuccessor(IEnumerable<TEdge> edges, TVertex u, out TEdge successor);

        #endregion
    }
}