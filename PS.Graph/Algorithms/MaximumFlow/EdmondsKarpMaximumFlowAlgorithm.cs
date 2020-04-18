using System;
using PS.Graph.Algorithms.Observers;
using PS.Graph.Algorithms.Search;
using PS.Graph.Algorithms.Services;
using PS.Graph.Predicates;

namespace PS.Graph.Algorithms.MaximumFlow
{
    /// <summary>
    ///     Edmond and Karp maximum flow algorithm
    ///     for directed graph with positive capacities and
    ///     flows.
    /// </summary>
    /// <typeparam name="TVertex">type of a vertex</typeparam>
    /// <typeparam name="TEdge">type of an edge</typeparam>
    [Serializable]
    public sealed class EdmondsKarpMaximumFlowAlgorithm<TVertex, TEdge> : MaximumFlowAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Constructors

        public EdmondsKarpMaximumFlowAlgorithm(
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> g,
            Func<TEdge, double> capacities,
            EdgeFactory<TVertex, TEdge> edgeFactory
        )
            : this(null, g, capacities, edgeFactory)
        {
        }

        public EdmondsKarpMaximumFlowAlgorithm(
            IAlgorithmComponent host,
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> g,
            Func<TEdge, double> capacities,
            EdgeFactory<TVertex, TEdge> edgeFactory
        )
            : base(host, g, capacities, edgeFactory)
        {
        }

        #endregion

        #region Properties

        private IVertexListGraph<TVertex, TEdge> ResidualGraph
        {
            get
            {
                return new FilteredVertexListGraph<
                    TVertex,
                    TEdge,
                    IVertexListGraph<TVertex, TEdge>
                >(
                    VisitedGraph,
                    v => true,
                    new ResidualEdgePredicate<TVertex, TEdge>(ResidualCapacities).Test
                );
            }
        }

        #endregion

        #region Override members

        /// <summary>
        ///     Computes the maximum flow between Source and Sink.
        /// </summary>
        /// <returns></returns>
        protected override void InternalCompute()
        {
            if (Source == null)
            {
                throw new InvalidOperationException("Source is not specified");
            }

            if (Sink == null)
            {
                throw new InvalidOperationException("Sink is not specified");
            }

            if (Services.CancelManager.IsCancelling)
            {
                return;
            }

            var g = VisitedGraph;
            foreach (var u in g.Vertices)
            foreach (var e in g.OutEdges(u))
            {
                var capacity = Capacities(e);
                if (capacity < 0)
                {
                    throw new InvalidOperationException("negative edge capacity");
                }

                ResidualCapacities[e] = capacity;
            }

            VertexColors[Sink] = GraphColor.Gray;
            while (VertexColors[Sink] != GraphColor.White)
            {
                var vis = new VertexPredecessorRecorderObserver<TVertex, TEdge>(
                    Predecessors
                );
                var queue = new Collections.Queue<TVertex>();
                var bfs = new BreathFirstSearchAlgorithm<TVertex, TEdge>(
                    ResidualGraph,
                    queue,
                    VertexColors
                );
                using (vis.Attach(bfs))
                {
                    bfs.Compute(Source);
                }

                if (VertexColors[Sink] != GraphColor.White)
                {
                    Augment(Source, Sink);
                }
            } // while

            MaxFlow = 0;
            foreach (var e in g.OutEdges(Source))
            {
                MaxFlow += Capacities(e) - ResidualCapacities[e];
            }
        }

        #endregion

        #region Members

        private void Augment(
            TVertex source,
            TVertex sink
        )
        {
            TEdge e;

            // find minimum residual capacity along the augmenting path
            var delta = double.MaxValue;
            var u = sink;
            do
            {
                e = Predecessors[u];
                delta = Math.Min(delta, ResidualCapacities[e]);
                u = e.Source;
            } while (!u.Equals(source));

            // push delta units of flow along the augmenting path
            u = sink;
            do
            {
                e = Predecessors[u];
                ResidualCapacities[e] -= delta;
                if (ReversedEdges != null && ReversedEdges.ContainsKey(e))
                {
                    ResidualCapacities[ReversedEdges[e]] += delta;
                }

                u = e.Source;
            } while (!u.Equals(source));
        }

        #endregion
    }
}