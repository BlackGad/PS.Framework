using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace PS.Graph.Algorithms.Condensation
{
    public sealed class EdgeMergeCondensationGraphAlgorithm<TVertex, TEdge> : AlgorithmBase<IBidirectionalGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        #region Constructors

        public EdgeMergeCondensationGraphAlgorithm(
            IBidirectionalGraph<TVertex, TEdge> visitedGraph,
            IMutableBidirectionalGraph<TVertex, MergedEdge<TVertex, TEdge>> condensatedGraph,
            VertexPredicate<TVertex> vertexPredicate
        )
            : base(visitedGraph)
        {
            Contract.Requires(condensatedGraph != null);
            Contract.Requires(vertexPredicate != null);

            CondensatedGraph = condensatedGraph;
            VertexPredicate = vertexPredicate;
        }

        #endregion

        #region Properties

        public IMutableBidirectionalGraph<TVertex,
            MergedEdge<TVertex, TEdge>
        > CondensatedGraph { get; }

        public VertexPredicate<TVertex> VertexPredicate { get; }

        #endregion

        #region Override members

        protected override void InternalCompute()
        {
            // adding vertices to the new graph
            // and pushing filtered vertices in queue
            var filteredVertices = new Queue<TVertex>();
            foreach (var v in VisitedGraph.Vertices)
            {
                CondensatedGraph.AddVertex(v);
                if (VertexPredicate(v)) continue;

                filteredVertices.Enqueue(v);
            }

            // adding all edges
            foreach (var edge in VisitedGraph.Edges)
            {
                var mergedEdge = new MergedEdge<TVertex, TEdge>(edge.Source, edge.Target);
                mergedEdge.Edges.Add(edge);

                CondensatedGraph.AddEdge(mergedEdge);
            }

            // remove vertices
            while (filteredVertices.Count > 0)
            {
                var filteredVertex = filteredVertices.Dequeue();

                // do the cross product between inEdges and outEdges
                MergeVertex(filteredVertex);
            }
        }

        #endregion

        #region Members

        private void MergeVertex(TVertex v)
        {
            // get in edges and outEdge
            var inEdges =
                new List<MergedEdge<TVertex, TEdge>>(CondensatedGraph.InEdges(v));
            var outEdges =
                new List<MergedEdge<TVertex, TEdge>>(CondensatedGraph.OutEdges(v));

            // remove vertex
            CondensatedGraph.RemoveVertex(v);

            // add condensated edges
            foreach (var inEdge in inEdges)
            {
                if (inEdge.Source.Equals(v)) continue;

                foreach (var outEdge in outEdges)
                {
                    if (outEdge.Target.Equals(v)) continue;

                    var newEdge =
                        MergedEdge<TVertex, TEdge>.Merge(inEdge, outEdge);
                    CondensatedGraph.AddEdge(newEdge);
                }
            }
        }

        #endregion
    }
}