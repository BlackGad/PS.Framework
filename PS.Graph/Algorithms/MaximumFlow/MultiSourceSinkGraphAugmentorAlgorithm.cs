using PS.Graph.Algorithms.Services;

namespace PS.Graph.Algorithms.MaximumFlow
{
    public sealed class MultiSourceSinkGraphAugmentorAlgorithm<TVertex, TEdge> : GraphAugmentorAlgorithmBase<TVertex, TEdge, IMutableBidirectionalGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        #region Constructors

        public MultiSourceSinkGraphAugmentorAlgorithm(
            IMutableBidirectionalGraph<TVertex, TEdge> visitedGraph,
            VertexFactory<TVertex> vertexFactory,
            EdgeFactory<TVertex, TEdge> edgeFactory)
            : this(null, visitedGraph, vertexFactory, edgeFactory)
        {
        }

        public MultiSourceSinkGraphAugmentorAlgorithm(
            IAlgorithmComponent host,
            IMutableBidirectionalGraph<TVertex, TEdge> visitedGraph,
            VertexFactory<TVertex> vertexFactory,
            EdgeFactory<TVertex, TEdge> edgeFactory)
            : base(host, visitedGraph, vertexFactory, edgeFactory)
        {
        }

        #endregion

        #region Override members

        protected override void AugmentGraph()
        {
            var cancelManager = Services.CancelManager;
            foreach (var v in VisitedGraph.Vertices)
            {
                if (cancelManager.IsCancelling) break;

                // is source
                if (VisitedGraph.IsInEdgesEmpty(v))
                {
                    AddAugmentedEdge(SuperSource, v);
                }

                // is sink
                if (VisitedGraph.IsOutEdgesEmpty(v))
                {
                    AddAugmentedEdge(v, SuperSink);
                }
            }
        }

        #endregion
    }
}