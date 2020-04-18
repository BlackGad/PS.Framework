using System;
using System.Collections.Generic;
using PS.Graph.Algorithms.Services;

namespace PS.Graph.Algorithms.Exploration
{
    public sealed class CloneableVertexGraphExplorerAlgorithm<TVertex, TEdge> : RootedAlgorithmBase<TVertex, IMutableVertexAndEdgeSet<TVertex, TEdge>>,
                                                                                ITreeBuilderAlgorithm<TVertex, TEdge>
        where TVertex : ICloneable, IComparable<TVertex>
        where TEdge : IEdge<TVertex>
    {
        private readonly Queue<TVertex> _unexploredVertices = new Queue<TVertex>();

        #region Constructors

        public CloneableVertexGraphExplorerAlgorithm(
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph
        )
            : this(null, visitedGraph)
        {
        }

        public CloneableVertexGraphExplorerAlgorithm(
            IAlgorithmComponent host,
            IMutableVertexAndEdgeSet<TVertex, TEdge> visitedGraph
        )
            : base(host, visitedGraph)
        {
        }

        #endregion

        #region Properties

        public EdgePredicate<TVertex, TEdge> AddEdgePredicate { get; set; } = e => true;

        public VertexPredicate<TVertex> AddVertexPredicate { get; set; } = v => true;

        public VertexPredicate<TVertex> ExploreVertexPredicate { get; set; } = v => true;

        public Predicate<CloneableVertexGraphExplorerAlgorithm<TVertex, TEdge>> FinishedPredicate { get; set; } = new DefaultFinishedPredicate().Test;

        public bool FinishedSuccessfully { get; private set; }

        public IList<ITransitionFactory<TVertex, TEdge>> TransitionFactories { get; } = new List<ITransitionFactory<TVertex, TEdge>>();

        public IEnumerable<TVertex> UnexploredVertices
        {
            get { return _unexploredVertices; }
        }

        #endregion

        #region Events

        public event EdgeAction<TVertex, TEdge> BackEdge;

        public event VertexAction<TVertex> DiscoverVertex;
        public event EdgeAction<TVertex, TEdge> EdgeSkipped;

        #endregion

        #region Override members

        protected override void InternalCompute()
        {
            if (!TryGetRootVertex(out var rootVertex))
            {
                throw new InvalidOperationException("RootVertex is not specified");
            }

            VisitedGraph.Clear();
            _unexploredVertices.Clear();
            FinishedSuccessfully = false;

            if (!AddVertexPredicate(rootVertex))
            {
                throw new ArgumentException("StartVertex does not satisfy AddVertexPredicate");
            }

            OnDiscoverVertex(rootVertex);

            while (_unexploredVertices.Count > 0)
            {
                // are we done yet ?
                if (!FinishedPredicate(this))
                {
                    FinishedSuccessfully = false;
                    return;
                }

                var current = _unexploredVertices.Dequeue();
                var clone = (TVertex)current.Clone();

                // let's make sure we want to explore this one
                if (!ExploreVertexPredicate(clone))
                {
                    continue;
                }

                foreach (var transitionFactory in TransitionFactories)
                {
                    GenerateFromTransitionFactory(clone, transitionFactory);
                }
            }

            FinishedSuccessfully = true;
        }

        #endregion

        #region ITreeBuilderAlgorithm<TVertex,TEdge> Members

        public event EdgeAction<TVertex, TEdge> TreeEdge;

        #endregion

        #region Members

        private void GenerateFromTransitionFactory(
            TVertex current,
            ITransitionFactory<TVertex, TEdge> transitionFactory
        )
        {
            if (!transitionFactory.IsValid(current))
            {
                return;
            }

            foreach (var transition in transitionFactory.Apply(current))
            {
                if (
                    !AddVertexPredicate(transition.Target)
                    || !AddEdgePredicate(transition))
                {
                    OnEdgeSkipped(transition);
                    continue;
                }

                var backEdge = VisitedGraph.ContainsVertex(transition.Target);
                if (!backEdge)
                {
                    OnDiscoverVertex(transition.Target);
                }

                VisitedGraph.AddEdge(transition);
                if (backEdge)
                {
                    OnBackEdge(transition);
                }
                else
                {
                    OnTreeEdge(transition);
                }
            }
        }

        private void OnBackEdge(TEdge e)
        {
            var eh = BackEdge;
            eh?.Invoke(e);
        }

        private void OnDiscoverVertex(TVertex v)
        {
            VisitedGraph.AddVertex(v);
            _unexploredVertices.Enqueue(v);

            var eh = DiscoverVertex;
            eh?.Invoke(v);
        }

        private void OnEdgeSkipped(TEdge e)
        {
            var eh = EdgeSkipped;
            eh?.Invoke(e);
        }

        private void OnTreeEdge(TEdge e)
        {
            var eh = TreeEdge;
            eh?.Invoke(e);
        }

        #endregion

        #region Nested type: DefaultFinishedPredicate

        public sealed class DefaultFinishedPredicate
        {
            #region Constructors

            public DefaultFinishedPredicate()
            {
            }

            public DefaultFinishedPredicate(
                int maxVertexCount,
                int maxEdgeCount)
            {
                MaxVertexCount = maxVertexCount;
                MaxEdgeCount = maxEdgeCount;
            }

            #endregion

            #region Properties

            public int MaxEdgeCount { get; set; } = 1000;

            public int MaxVertexCount { get; set; } = 1000;

            #endregion

            #region Members

            public bool Test(CloneableVertexGraphExplorerAlgorithm<TVertex, TEdge> t)
            {
                if (t.VisitedGraph.VertexCount > MaxVertexCount)
                {
                    return false;
                }

                if (t.VisitedGraph.EdgeCount > MaxEdgeCount)
                {
                    return false;
                }

                return true;
            }

            #endregion
        }

        #endregion
    }
}