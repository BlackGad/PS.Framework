using System;
using System.Collections.Generic;

namespace PS.Graph.Algorithms.MaximumFlow
{
    [Serializable]
    public sealed class GraphBalancerAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private List<TEdge> _deficientEdges = new List<TEdge>();
        private List<TVertex> _deficientVertices = new List<TVertex>();
        private Dictionary<TEdge, int> _preFlow = new Dictionary<TEdge, int>();

        private List<TEdge> _surplusEdges = new List<TEdge>();
        private List<TVertex> _surplusVertices = new List<TVertex>();

        #region Constructors

        public GraphBalancerAlgorithm(
            IMutableBidirectionalGraph<TVertex, TEdge> visitedGraph,
            TVertex source,
            TVertex sink,
            VertexFactory<TVertex> vertexFactory,
            EdgeFactory<TVertex, TEdge> edgeFactory
        )
        {
            VisitedGraph = visitedGraph;
            VertexFactory = vertexFactory;
            EdgeFactory = edgeFactory;
            Source = source;
            Sink = sink;

            // setting capacities = u(e) = +infinity
            foreach (var edge in VisitedGraph.Edges)
            {
                Capacities.Add(edge, double.MaxValue);
            }

            // setting preflow = l(e) = 1
            foreach (var edge in VisitedGraph.Edges)
            {
                _preFlow.Add(edge, 1);
            }
        }

        public GraphBalancerAlgorithm(
            IMutableBidirectionalGraph<TVertex, TEdge> visitedGraph,
            TVertex source,
            TVertex sink,
            IDictionary<TEdge, double> capacities)
        {
            VisitedGraph = visitedGraph;
            Source = source;
            Sink = sink;
            Capacities = capacities;

            // setting preflow = l(e) = 1
            foreach (var edge in VisitedGraph.Edges)
            {
                _preFlow.Add(edge, 1);
            }
        }

        #endregion

        #region Properties

        public bool Balanced { get; private set; }

        public TVertex BalancingSink { get; private set; }

        public TEdge BalancingSinkEdge { get; private set; }

        public TVertex BalancingSource { get; private set; }

        public TEdge BalancingSourceEdge { get; private set; }

        public IDictionary<TEdge, double> Capacities { get; } = new Dictionary<TEdge, double>();

        public ICollection<TEdge> DeficientEdges
        {
            get { return _deficientEdges; }
        }

        public ICollection<TVertex> DeficientVertices
        {
            get { return _deficientVertices; }
        }

        public EdgeFactory<TVertex, TEdge> EdgeFactory { get; }

        public TVertex Sink { get; }

        public TVertex Source { get; }

        public ICollection<TEdge> SurplusEdges
        {
            get { return _surplusEdges; }
        }

        public ICollection<TVertex> SurplusVertices
        {
            get { return _surplusVertices; }
        }

        public VertexFactory<TVertex> VertexFactory { get; }

        public IMutableBidirectionalGraph<TVertex, TEdge> VisitedGraph { get; }

        #endregion

        #region Events

        public event VertexAction<TVertex> BalancingSinkAdded;

        public event VertexAction<TVertex> BalancingSourceAdded;
        public event VertexAction<TVertex> DeficientVertexAdded;
        public event EdgeAction<TVertex, TEdge> EdgeAdded;
        public event VertexAction<TVertex> SurplusVertexAdded;

        #endregion

        #region Members

        public void Balance()
        {
            if (Balanced)
            {
                throw new InvalidOperationException("Graph already balanced");
            }

            // step 0
            // create new source, new sink
            BalancingSource = VertexFactory();
            VisitedGraph.AddVertex(BalancingSource);
            OnBalancingSourceAdded();

            BalancingSink = VertexFactory();
            VisitedGraph.AddVertex(BalancingSink);
            OnBalancingSinkAdded();

            // step 1
            BalancingSourceEdge = EdgeFactory(BalancingSource, Source);
            VisitedGraph.AddEdge(BalancingSourceEdge);
            Capacities.Add(BalancingSourceEdge, double.MaxValue);
            _preFlow.Add(BalancingSourceEdge, 0);
            OnEdgeAdded(BalancingSourceEdge);

            BalancingSinkEdge = EdgeFactory(Sink, BalancingSink);
            VisitedGraph.AddEdge(BalancingSinkEdge);
            Capacities.Add(BalancingSinkEdge, double.MaxValue);
            _preFlow.Add(BalancingSinkEdge, 0);
            OnEdgeAdded(BalancingSinkEdge);

            // step 2
            // for each surplus vertex v, add (source -> v)
            foreach (var v in VisitedGraph.Vertices)
            {
                if (v.Equals(BalancingSource))
                {
                    continue;
                }

                if (v.Equals(BalancingSink))
                {
                    continue;
                }

                if (v.Equals(Source))
                {
                    continue;
                }

                if (v.Equals(Sink))
                {
                    continue;
                }

                var balancingIndex = GetBalancingIndex(v);
                if (balancingIndex == 0)
                {
                    continue;
                }

                if (balancingIndex < 0)
                {
                    // surplus vertex
                    var edge = EdgeFactory(BalancingSource, v);
                    VisitedGraph.AddEdge(edge);
                    _surplusEdges.Add(edge);
                    _surplusVertices.Add(v);
                    _preFlow.Add(edge, 0);
                    Capacities.Add(edge, -balancingIndex);
                    OnSurplusVertexAdded(v);
                    OnEdgeAdded(edge);
                }
                else
                {
                    // deficient vertex
                    var edge = EdgeFactory(v, BalancingSink);
                    _deficientEdges.Add(edge);
                    _deficientVertices.Add(v);
                    _preFlow.Add(edge, 0);
                    Capacities.Add(edge, balancingIndex);
                    OnDeficientVertexAdded(v);
                    OnEdgeAdded(edge);
                }
            }

            Balanced = true;
        }

        public int GetBalancingIndex(TVertex v)
        {
            var bi = 0;
            foreach (var edge in VisitedGraph.OutEdges(v))
            {
                var pf = _preFlow[edge];
                bi += pf;
            }

            foreach (var edge in VisitedGraph.InEdges(v))
            {
                var pf = _preFlow[edge];
                bi -= pf;
            }

            return bi;
        }

        public void UnBalance()
        {
            if (!Balanced)
            {
                throw new InvalidOperationException("Graph is not balanced");
            }

            foreach (var edge in _surplusEdges)
            {
                VisitedGraph.RemoveEdge(edge);
                Capacities.Remove(edge);
                _preFlow.Remove(edge);
            }

            foreach (var edge in _deficientEdges)
            {
                VisitedGraph.RemoveEdge(edge);
                Capacities.Remove(edge);
                _preFlow.Remove(edge);
            }

            Capacities.Remove(BalancingSinkEdge);
            Capacities.Remove(BalancingSourceEdge);
            _preFlow.Remove(BalancingSinkEdge);
            _preFlow.Remove(BalancingSourceEdge);
            VisitedGraph.RemoveEdge(BalancingSourceEdge);
            VisitedGraph.RemoveEdge(BalancingSinkEdge);
            VisitedGraph.RemoveVertex(BalancingSource);
            VisitedGraph.RemoveVertex(BalancingSink);

            BalancingSource = default;
            BalancingSink = default;
            BalancingSourceEdge = default;
            BalancingSinkEdge = default;

            _surplusEdges.Clear();
            _deficientEdges.Clear();
            _surplusVertices.Clear();
            _deficientVertices.Clear();

            Balanced = false;
        }

        private void OnBalancingSinkAdded()
        {
            var eh = BalancingSinkAdded;
            eh?.Invoke(Sink);
        }

        private void OnBalancingSourceAdded()
        {
            var eh = BalancingSourceAdded;
            eh?.Invoke(Source);
        }

        private void OnDeficientVertexAdded(TVertex vertex)
        {
            var eh = DeficientVertexAdded;
            eh?.Invoke(vertex);
        }

        private void OnEdgeAdded(TEdge edge)
        {
            var eh = EdgeAdded;
            eh?.Invoke(edge);
        }

        private void OnSurplusVertexAdded(TVertex vertex)
        {
            var eh = SurplusVertexAdded;
            eh?.Invoke(vertex);
        }

        #endregion
    }
}