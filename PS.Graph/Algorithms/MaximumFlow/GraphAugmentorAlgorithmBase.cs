using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using PS.Graph.Algorithms.Services;

namespace PS.Graph.Algorithms.MaximumFlow
{
    public abstract class GraphAugmentorAlgorithmBase<TVertex, TEdge, TGraph> : AlgorithmBase<TGraph>,
                                                                                IDisposable
        where TEdge : IEdge<TVertex>
        where TGraph : IMutableVertexAndEdgeSet<TVertex, TEdge>
    {
        private readonly List<TEdge> _augmentedEdges = new List<TEdge>();

        #region Constructors

        protected GraphAugmentorAlgorithmBase(
            IAlgorithmComponent host,
            TGraph visitedGraph,
            VertexFactory<TVertex> vertexFactory,
            EdgeFactory<TVertex, TEdge> edgeFactory
        )
            : base(host, visitedGraph)
        {
            Contract.Requires(vertexFactory != null);
            Contract.Requires(edgeFactory != null);

            VertexFactory = vertexFactory;
            EdgeFactory = edgeFactory;
        }

        #endregion

        #region Properties

        public bool Augmented { get; private set; }

        public ICollection<TEdge> AugmentedEdges
        {
            get { return _augmentedEdges; }
        }

        public EdgeFactory<TVertex, TEdge> EdgeFactory { get; }

        public TVertex SuperSink { get; private set; }

        public TVertex SuperSource { get; private set; }

        public VertexFactory<TVertex> VertexFactory { get; }

        #endregion

        #region Events

        public event EdgeAction<TVertex, TEdge> EdgeAdded;

        public event VertexAction<TVertex> SuperSinkAdded;

        public event VertexAction<TVertex> SuperSourceAdded;

        #endregion

        #region Override members

        protected override void InternalCompute()
        {
            if (Augmented)
            {
                throw new InvalidOperationException("Graph already augmented");
            }

            SuperSource = VertexFactory();
            VisitedGraph.AddVertex(SuperSource);
            OnSuperSourceAdded(SuperSource);

            SuperSink = VertexFactory();
            VisitedGraph.AddVertex(SuperSink);
            OnSuperSinkAdded(SuperSink);

            AugmentGraph();
            Augmented = true;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Rollback();
        }

        #endregion

        #region Members

        public virtual void Rollback()
        {
            if (!Augmented)
            {
                return;
            }

            Augmented = false;
            VisitedGraph.RemoveVertex(SuperSource);
            VisitedGraph.RemoveVertex(SuperSink);
            SuperSource = default;
            SuperSink = default;
            _augmentedEdges.Clear();
        }

        protected void AddAugmentedEdge(TVertex source, TVertex target)
        {
            var edge = EdgeFactory(source, target);
            _augmentedEdges.Add(edge);
            VisitedGraph.AddEdge(edge);
            OnEdgeAdded(edge);
        }

        protected abstract void AugmentGraph();

        private void OnEdgeAdded(TEdge e)
        {
            Contract.Requires(e != null);
            var eh = EdgeAdded;
            eh?.Invoke(e);
        }

        private void OnSuperSinkAdded(TVertex v)
        {
            Contract.Requires(v != null);
            var eh = SuperSinkAdded;
            eh?.Invoke(v);
        }

        private void OnSuperSourceAdded(TVertex v)
        {
            Contract.Requires(v != null);
            var eh = SuperSourceAdded;
            eh?.Invoke(v);
        }

        #endregion
    }
}