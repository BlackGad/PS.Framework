using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using PS.Graph.Algorithms.Services;

namespace PS.Graph.Algorithms.MaximumFlow
{
    /// <summary>
    ///     Abstract base class for maximum flow algorithms.    ///
    /// </summary>
    [Serializable]
    public abstract class MaximumFlowAlgorithm<TVertex, TEdge> : AlgorithmBase<IMutableVertexAndEdgeListGraph<TVertex, TEdge>>,
                                                                 IVertexColorizerAlgorithm<TVertex>
        where TEdge : IEdge<TVertex>
    {
        private TVertex _sink;
        private TVertex _source;

        #region Constructors

        protected MaximumFlowAlgorithm(
            IAlgorithmComponent host,
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> capacities,
            EdgeFactory<TVertex, TEdge> edgeFactory
        )
            : base(host, visitedGraph)
        {
            Contract.Requires(capacities != null);

            Capacities = capacities;
            Predecessors = new Dictionary<TVertex, TEdge>();
            EdgeFactory = edgeFactory;
            ResidualCapacities = new Dictionary<TEdge, double>();
            VertexColors = new Dictionary<TVertex, GraphColor>();
        }

        #endregion

        #region Properties

        public Func<TEdge, double> Capacities { get; private set; }

        public EdgeFactory<TVertex, TEdge> EdgeFactory { get; private set; }

        public double MaxFlow { get; set; }

        public Dictionary<TVertex, TEdge> Predecessors { get; private set; }

        public Dictionary<TEdge, double> ResidualCapacities { get; private set; }

        public Dictionary<TEdge, TEdge> ReversedEdges { get; protected set; }

        public TVertex Sink
        {
            get { return _sink; }
            set
            {
                Contract.Requires(value != null);
                _sink = value;
            }
        }

        public TVertex Source
        {
            get { return _source; }
            set
            {
                Contract.Requires(value != null);
                _source = value;
            }
        }

        public Dictionary<TVertex, GraphColor> VertexColors { get; private set; }

        #endregion

        #region IVertexColorizerAlgorithm<TVertex,TEdge> Members

        public GraphColor GetVertexColor(TVertex vertex)
        {
            return VertexColors[vertex];
        }

        #endregion

        #region Members

        public double Compute(TVertex source, TVertex sink)
        {
            Source = source;
            Sink = sink;
            Compute();
            return MaxFlow;
        }

        #endregion
    }
}