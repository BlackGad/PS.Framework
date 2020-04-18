using System.Collections.Generic;
using PS.Graph.Algorithms.Services;

namespace PS.Graph.Algorithms.MaximumFlow
{
    /// <summary>
    ///     This algorithm modifies a bipartite graph into a related graph, where each Vertex in one partition is
    ///     connected to a newly added "SuperSource" and each Vertex in the other partition is connected to a newly added
    ///     "SuperSink"
    ///     When the maximum flow of this related graph is computed, the edges used for the flow are also those which make up
    ///     the maximum match for the bipartite graph.
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    internal class BipartiteToMaximumFlowGraphAugmentorAlgorithm<TVertex, TEdge> : GraphAugmentorAlgorithmBase<TVertex, TEdge, IMutableVertexAndEdgeSet<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        #region Constructors

        public BipartiteToMaximumFlowGraphAugmentorAlgorithm(
            IMutableVertexAndEdgeSet<TVertex, TEdge> visitedGraph,
            IEnumerable<TVertex> vertexSetA,
            IEnumerable<TVertex> vertexSetB,
            VertexFactory<TVertex> vertexFactory,
            EdgeFactory<TVertex, TEdge> edgeFactory
        )
            : this(null, visitedGraph, vertexSetA, vertexSetB, vertexFactory, edgeFactory)
        {
        }

        public BipartiteToMaximumFlowGraphAugmentorAlgorithm(
            IAlgorithmComponent host,
            IMutableVertexAndEdgeSet<TVertex, TEdge> visitedGraph,
            IEnumerable<TVertex> vertexSetA,
            IEnumerable<TVertex> vertexSetB,
            VertexFactory<TVertex> vertexFactory,
            EdgeFactory<TVertex, TEdge> edgeFactory
        )
            : base(host, visitedGraph, vertexFactory, edgeFactory)
        {
            VertexSetA = vertexSetA;
            VertexSetB = vertexSetB;
        }

        #endregion

        #region Properties

        public IEnumerable<TVertex> VertexSetA { get; }
        public IEnumerable<TVertex> VertexSetB { get; }

        #endregion

        #region Override members

        protected override void AugmentGraph()
        {
            var cancelManager = Services.CancelManager;
            foreach (var v in VertexSetA)
            {
                if (cancelManager.IsCancelling) break;

                AddAugmentedEdge(SuperSource, v);
            }

            foreach (var v in VertexSetB)
            {
                if (cancelManager.IsCancelling) break;

                AddAugmentedEdge(v, SuperSink);
            }
        }

        #endregion
    }
}