using System;
using System.Diagnostics.Contracts;

namespace PS.Graph.Algorithms.RandomWalks
{
    [Serializable]
    public sealed class RandomWalkAlgorithm<TVertex, TEdge> : ITreeBuilderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private IEdgeChain<TVertex, TEdge> _edgeChain;
        private EdgePredicate<TVertex, TEdge> _endPredicate;

        #region Constructors

        public RandomWalkAlgorithm(IImplicitGraph<TVertex, TEdge> visitedGraph)
            : this(visitedGraph, new NormalizedMarkovEdgeChain<TVertex, TEdge>())
        {
        }

        public RandomWalkAlgorithm(
            IImplicitGraph<TVertex, TEdge> visitedGraph,
            IEdgeChain<TVertex, TEdge> edgeChain
        )
        {
            Contract.Requires(visitedGraph != null);
            Contract.Requires(edgeChain != null);

            VisitedGraph = visitedGraph;
            _edgeChain = edgeChain;
        }

        #endregion

        #region Properties

        public IEdgeChain<TVertex, TEdge> EdgeChain
        {
            get { return _edgeChain; }
            set
            {
                Contract.Requires(value != null);

                _edgeChain = value;
            }
        }

        public EdgePredicate<TVertex, TEdge> EndPredicate
        {
            get { return _endPredicate; }
            set { _endPredicate = value; }
        }

        public IImplicitGraph<TVertex, TEdge> VisitedGraph { get; }

        #endregion

        #region Events

        public event VertexAction<TVertex> EndVertex;

        public event VertexAction<TVertex> StartVertex;

        #endregion

        #region ITreeBuilderAlgorithm<TVertex,TEdge> Members

        public event EdgeAction<TVertex, TEdge> TreeEdge;

        #endregion

        #region Members

        public void Generate(TVertex root)
        {
            Contract.Requires(root != null);

            Generate(root, 100);
        }

        public void Generate(TVertex root, int walkCount)
        {
            Contract.Requires(root != null);

            var count = 0;
            var v = root;

            OnStartVertex(root);
            while (count < walkCount && TryGetSuccessor(v, out var e))
            {
                // if dead end stop
                if (e == null)
                {
                    break;
                }

                // if end predicate, test
                if (_endPredicate != null && _endPredicate(e))
                {
                    break;
                }

                OnTreeEdge(e);
                v = e.Target;
                // upgrade count
                ++count;
            }

            OnEndVertex(v);
        }

        private void OnEndVertex(TVertex v)
        {
            EndVertex?.Invoke(v);
        }

        private void OnStartVertex(TVertex v)
        {
            StartVertex?.Invoke(v);
        }

        private void OnTreeEdge(TEdge e)
        {
            TreeEdge?.Invoke(e);
        }

        private bool TryGetSuccessor(TVertex u, out TEdge successor)
        {
            return EdgeChain.TryGetSuccessor(VisitedGraph, u, out successor);
        }

        #endregion
    }
}