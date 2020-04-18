using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using PS.Graph.Algorithms.Observers;
using PS.Graph.Algorithms.Services;
using PS.Graph.Algorithms.ShortestPath;
using PS.Graph.Collections;

namespace PS.Graph.Algorithms.RankedShortestPath
{
    /// <summary>
    ///     Hoffman and Pavley K-shortest path algorithm.
    /// </summary>
    /// <remarks>
    ///     Reference:
    ///     Hoffman, W. and Pavley, R. 1959. A Method for the Solution of the Nth Best Path Problem.
    ///     J. ACM 6, 4 (Oct. 1959), 506-514. DOI= http://doi.acm.org/10.1145/320998.321004
    /// </remarks>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    public sealed class HoffmanPavleyRankedShortestPathAlgorithm<TVertex, TEdge> : RankedShortestPathAlgorithmBase<TVertex, TEdge, IBidirectionalGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private readonly Func<TEdge, double> _edgeWeights;
        private TVertex _goalVertex;
        private bool _goalVertexSet;

        #region Constructors

        public HoffmanPavleyRankedShortestPathAlgorithm(
            IBidirectionalGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> edgeWeights)
            : this(null, visitedGraph, edgeWeights, DistanceRelaxers.ShortestDistance)
        {
        }

        public HoffmanPavleyRankedShortestPathAlgorithm(
            IAlgorithmComponent host,
            IBidirectionalGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> edgeWeights,
            IDistanceRelaxer distanceRelaxer)
            : base(host, visitedGraph, distanceRelaxer)
        {
            _edgeWeights = edgeWeights;
        }

        #endregion

        #region Override members

        protected override void InternalCompute()
        {
            var cancelManager = Services.CancelManager;
            if (!TryGetRootVertex(out var root))
            {
                throw new InvalidOperationException("root vertex not set");
            }

            if (!TryGetGoalVertex(out var goal))
            {
                throw new InvalidOperationException("goal vertex not set");
            }

            // start by building the minimum tree starting from the goal vertex.
            ComputeMinimumTree(goal, out var successors, out var distances);

            if (cancelManager.IsCancelling) return;

            var queue = new FibonacciQueue<DeviationPath, double>(dp => dp.Weight);
            var vertexCount = VisitedGraph.VertexCount;

            // first shortest path
            EnqueueFirstShortestPath(queue, successors, distances, root);

            while (queue.Count > 0 &&
                   ComputedShortestPathCount < ShortestPathCount)
            {
                if (cancelManager.IsCancelling) return;

                var deviation = queue.Dequeue();

                // turn into path
                var path = new List<TEdge>();
                for (var i = 0; i < deviation.DeviationIndex; ++i)
                {
                    path.Add(deviation.ParentPath[i]);
                }

                path.Add(deviation.DeviationEdge);
                var startEdge = path.Count;
                AppendShortestPath(path, successors, deviation.DeviationEdge.Target);

                Contract.Assert(Math.Abs(deviation.Weight - path.Sum(e => _edgeWeights(e))) < double.Epsilon);
                Contract.Assert(path.Count > 0);

                // add to list if loopless
                if (!path.HasCycles<TVertex, TEdge>())
                {
                    AddComputedShortestPath(path);
                }

                // append new deviation paths
                if (path.Count < vertexCount)
                {
                    EnqueueDeviationPaths(
                        queue,
                        root,
                        distances,
                        path.ToArray(),
                        startEdge
                    );
                }
            }
        }

        #endregion

        #region Members

        public void Compute(TVertex rootVertex, TVertex goalVertex)
        {
            SetRootVertex(rootVertex);
            SetGoalVertex(goalVertex);
            Compute();
        }

        public void SetGoalVertex(TVertex goalVertex)
        {
            _goalVertex = goalVertex;
            _goalVertexSet = true;
        }

        public bool TryGetGoalVertex(out TVertex goalVertex)
        {
            if (_goalVertexSet)
            {
                goalVertex = _goalVertex;
                return true;
            }

            goalVertex = default;
            return false;
        }

        private void AppendShortestPath(
            List<TEdge> path,
            IDictionary<TVertex, TEdge> successors,
            TVertex startVertex)
        {
            Contract.Ensures(path[path.Count - 1].Target.Equals(_goalVertex));

            var current = startVertex;
            while (successors.TryGetValue(current, out var edge))
            {
                path.Add(edge);
                current = edge.Target;
            }
        }

        private void ComputeMinimumTree(
            TVertex goal,
            out IDictionary<TVertex, TEdge> successors,
            out IDictionary<TVertex, double> distances)
        {
            var reversedGraph = new ReversedBidirectionalGraph<TVertex, TEdge>(VisitedGraph);
            var successorsObserver = new VertexPredecessorRecorderObserver<TVertex, SReversedEdge<TVertex, TEdge>>();

            double ReversedEdgeWeight(SReversedEdge<TVertex, TEdge> e)
            {
                return _edgeWeights(e.OriginalEdge);
            }

            var distancesObserver = new VertexDistanceRecorderObserver<TVertex, SReversedEdge<TVertex, TEdge>>(ReversedEdgeWeight);
            var shortestPath = new DijkstraShortestPathAlgorithm<TVertex, SReversedEdge<TVertex, TEdge>>(
                this,
                reversedGraph,
                ReversedEdgeWeight,
                DistanceRelaxer);
            using (successorsObserver.Attach(shortestPath))
            using (distancesObserver.Attach(shortestPath))
            {
                shortestPath.Compute(goal);
            }

            successors = new Dictionary<TVertex, TEdge>();
            foreach (var kv in successorsObserver.VertexPredecessors)
            {
                successors.Add(kv.Key, kv.Value.OriginalEdge);
            }

            distances = distancesObserver.Distances;
        }

        private void EnqueueDeviationPaths(
            IQueue<DeviationPath> queue,
            TVertex root,
            IDictionary<TVertex, double> distances,
            TEdge[] path,
            int startEdge
        )
        {
            var previousVertex = root;
            double previousWeight = 0;
            var pathVertices = new Dictionary<TVertex, int>(path.Length);
            for (var iEdge = 0; iEdge < path.Length; ++iEdge)
            {
                var edge = path[iEdge];
                if (iEdge >= startEdge)
                {
                    EnqueueDeviationPaths(
                        queue,
                        distances,
                        path,
                        iEdge,
                        previousVertex,
                        previousWeight
                    );
                }

                // update counter
                previousVertex = edge.Target;
                previousWeight += _edgeWeights(edge);

                // detection of loops
                if (iEdge == 0)
                {
                    pathVertices[edge.Source] = 0;
                }

                // we should really allow only one key
                if (pathVertices.ContainsKey(edge.Target))
                {
                    break;
                }

                pathVertices[edge.Target] = 0;
            }
        }

        private void EnqueueDeviationPaths(
            IQueue<DeviationPath> queue,
            IDictionary<TVertex, double> distances,
            TEdge[] path,
            int iEdge,
            TVertex previousVertex,
            double previousWeight
        )
        {
            var edge = path[iEdge];
            foreach (var deviationEdge in VisitedGraph.OutEdges(previousVertex))
            {
                // skip self edges,
                // skip equal edges,
                if (deviationEdge.Equals(edge) ||
                    deviationEdge.IsSelfEdge<TVertex, TEdge>())
                {
                    continue;
                }

                // any edge obviously creating a loop
                var target = deviationEdge.Target;

                if (distances.TryGetValue(target, out var distance))
                {
                    var deviationWeight = DistanceRelaxer.Combine(previousWeight, DistanceRelaxer.Combine(_edgeWeights(deviationEdge), distance));

                    var deviation = new DeviationPath(
                        path,
                        iEdge,
                        deviationEdge,
                        deviationWeight
                    );
                    queue.Enqueue(deviation);
                }
            }
        }

        private void EnqueueFirstShortestPath(
            IQueue<DeviationPath> queue,
            IDictionary<TVertex, TEdge> successors,
            IDictionary<TVertex, double> distances,
            TVertex root)
        {
            var path = new List<TEdge>();
            AppendShortestPath(
                path,
                successors,
                root);
            if (path.Count == 0)
            {
                return; // unreachable vertices
            }

            if (!path.HasCycles<TVertex, TEdge>())
            {
                AddComputedShortestPath(path);
            }

            // create deviation paths
            EnqueueDeviationPaths(
                queue,
                root,
                distances,
                path.ToArray(),
                0);
        }

        #endregion

        #region Nested type: DeviationPath

        [DebuggerDisplay("Weight = {Weight}, Index = {DeviationIndex}, Edge = {DeviationEdge}")]
        private readonly struct DeviationPath
        {
            public readonly TEdge DeviationEdge;
            public readonly int DeviationIndex;
            public readonly TEdge[] ParentPath;
            public readonly double Weight;

            #region Constructors

            public DeviationPath(
                TEdge[] parentPath,
                int deviationIndex,
                TEdge deviationEdge,
                double weight)
            {
                ParentPath = parentPath;
                DeviationIndex = deviationIndex;
                DeviationEdge = deviationEdge;
                Weight = weight;
            }

            #endregion

            #region Override members

            public override string ToString()
            {
                return $"{Weight} at {DeviationEdge} {string.Join(" -> ", ParentPath)}";
            }

            #endregion
        }

        #endregion
    }
}