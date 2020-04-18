using System;
using System.Collections.Generic;
using PS.Graph.Algorithms.ConnectedComponents;

namespace PS.Graph.Algorithms.Condensation
{
    public sealed class CondensationGraphAlgorithm<TVertex, TEdge, TGraph> : AlgorithmBase<IVertexAndEdgeListGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
        where TGraph : IMutableVertexAndEdgeSet<TVertex, TEdge>, new()
    {
        #region Constructors

        public CondensationGraphAlgorithm(IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph)
            : base(visitedGraph)
        {
        }

        #endregion

        #region Properties

        public IMutableBidirectionalGraph<
            TGraph,
            CondensedEdge<TVertex, TEdge, TGraph>
        > CondensedGraph { get; private set; }

        public bool StronglyConnected { get; set; } = true;

        #endregion

        #region Override members

        protected override void InternalCompute()
        {
            // create condensated graph
            CondensedGraph = new BidirectionalGraph<
                TGraph,
                CondensedEdge<TVertex, TEdge, TGraph>
            >(false);
            if (VisitedGraph.VertexCount == 0)
            {
                return;
            }

            // compute strongly connected components
            var components = new Dictionary<TVertex, int>(VisitedGraph.VertexCount);
            var componentCount = ComputeComponentCount(components);

            var cancelManager = Services.CancelManager;
            if (cancelManager.IsCancelling) return;

            // create list vertices
            var condensatedVertices = new Dictionary<int, TGraph>(componentCount);
            for (var i = 0; i < componentCount; ++i)
            {
                var v = new TGraph();
                condensatedVertices.Add(i, v);
                CondensedGraph.AddVertex(v);
            }

            // adding vertices
            foreach (var v in VisitedGraph.Vertices)
            {
                condensatedVertices[components[v]].AddVertex(v);
            }

            if (cancelManager.IsCancelling) return;

            // condensated edges
            var condensatedEdges = new Dictionary<EdgeKey, CondensedEdge<TVertex, TEdge, TGraph>>(componentCount);

            // iterate over edges and condensate graph
            foreach (var edge in VisitedGraph.Edges)
            {
                // get component ids
                var sourceId = components[edge.Source];
                var targetId = components[edge.Target];

                // get vertices
                var sources = condensatedVertices[sourceId];
                if (sourceId == targetId)
                {
                    sources.AddEdge(edge);
                    continue;
                }

                var targets = condensatedVertices[targetId];
                // at last add edge
                var edgeKey = new EdgeKey(sourceId, targetId);
                if (!condensatedEdges.TryGetValue(edgeKey, out var condensatedEdge))
                {
                    condensatedEdge = new CondensedEdge<TVertex, TEdge, TGraph>(sources, targets);
                    condensatedEdges.Add(edgeKey, condensatedEdge);
                    CondensedGraph.AddEdge(condensatedEdge);
                }

                condensatedEdge.Edges.Add(edge);
            }
        }

        #endregion

        #region Members

        private int ComputeComponentCount(Dictionary<TVertex, int> components)
        {
            IConnectedComponentAlgorithm<TVertex, IVertexListGraph<TVertex, TEdge>> componentAlgorithm;
            if (StronglyConnected)
            {
                componentAlgorithm = new StronglyConnectedComponentAlgorithm<TVertex, TEdge>(
                    this,
                    VisitedGraph,
                    components);
            }
            else
            {
                componentAlgorithm = new WeaklyConnectedComponentAlgorithm<TVertex, TEdge>(
                    this,
                    VisitedGraph,
                    components);
            }

            componentAlgorithm.Compute();
            return componentAlgorithm.ComponentCount;
        }

        #endregion

        #region Nested type: EdgeKey

        private readonly struct EdgeKey : IEquatable<EdgeKey>
        {
            private readonly int _sourceId;
            private readonly int _targetId;

            #region Constructors

            public EdgeKey(int sourceId, int targetId)
            {
                _sourceId = sourceId;
                _targetId = targetId;
            }

            #endregion

            #region Override members

            public override int GetHashCode()
            {
                return HashCodeHelper.Combine(_sourceId, _targetId);
            }

            #endregion

            #region IEquatable<CondensationGraphAlgorithm<TVertex,TEdge,TGraph>.EdgeKey> Members

            public bool Equals(EdgeKey other)
            {
                return
                    _sourceId == other._sourceId
                    && _targetId == other._targetId;
            }

            #endregion
        }

        #endregion
    }
}