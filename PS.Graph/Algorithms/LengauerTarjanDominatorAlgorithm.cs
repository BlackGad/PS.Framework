using System;
using System.Collections.Generic;
using PS.Graph.Algorithms.Observers;
using PS.Graph.Algorithms.Search;
using PS.Graph.Algorithms.Services;

namespace PS.Graph.Algorithms
{
    /// <summary>
    ///     Computes the dominator map of a directed graph
    /// </summary>
    /// <remarks>
    ///     Thomas Lengauer and Robert Endre Tarjan
    ///     A fast algorithm for finding dominators in a flowgraph
    ///     ACM Transactions on Programming Language and Systems, 1(1):121-141, 1979.
    /// </remarks>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    internal class LengauerTarjanDominatorAlgorithm<TVertex, TEdge> : RootedAlgorithmBase<TVertex, IBidirectionalGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        #region Constructors

        public LengauerTarjanDominatorAlgorithm(
            IAlgorithmComponent host,
            IBidirectionalGraph<TVertex, TEdge> visitedGraph)
            : base(host, visitedGraph)
        {
        }

        public LengauerTarjanDominatorAlgorithm(
            IBidirectionalGraph<TVertex, TEdge> visitedGraph)
            : this(null, visitedGraph)
        {
        }

        #endregion

        #region Override members

        protected override void InternalCompute()
        {
            var cancelManager = Services.CancelManager;
            var vertexCount = VisitedGraph.VertexCount;
            var vertices = VisitedGraph.Vertices;

            var timeStamps = new Dictionary<TVertex, int>(vertexCount);
            var stamps = new List<TVertex>(vertexCount);
            var predecessors = new Dictionary<TVertex, TEdge>(vertexCount);

            // phase 1: DFS over the graph and record vertex indices
            var dfs = new DepthFirstSearchAlgorithm<TVertex, TEdge>(this, VisitedGraph);
            using (new TimeStampObserver(stamps).Attach(dfs))
            using (new VertexTimeStamperObserver<TVertex>(timeStamps).Attach(dfs))
            using (new VertexPredecessorRecorderObserver<TVertex, TEdge>(predecessors).Attach(dfs))
            {
                dfs.Compute();
            }

            if (cancelManager.IsCancelling) return;

            // phase 2: find semidominators
            var semi = new Dictionary<TVertex, TVertex>(vertexCount);
            foreach (var v in vertices)
            {
                if (!timeStamps.TryGetValue(v, out var vTime) ||
                    !predecessors.TryGetValue(v, out var dominatorEdge))
                {
                    continue; // skip unreachable
                }

                var dominator = dominatorEdge.Source;
                if (!timeStamps.TryGetValue(dominator, out var dominatorTime))
                {
                    continue;
                }

                foreach (var e in VisitedGraph.InEdges(v))
                {
                    var u = e.Source;
                    if (!timeStamps.TryGetValue(u, out var uTime))
                    {
                        continue;
                    }

                    TVertex candidate;
                    if (uTime < vTime)
                    {
                        candidate = u;
                    }
                    else
                    {
                        var ancestor = default(TVertex);
                        // ReSharper disable once AssignNullToNotNullAttribute
                        candidate = semi[ancestor];
                    }

                    var candidateTime = timeStamps[candidate];
                    if (candidateTime < dominatorTime)
                    {
                        dominator = candidate;
                        dominatorTime = candidateTime;
                    }
                }

                semi[v] = dominator;
            }

            // phase 3:
        }

        #endregion

        #region Nested type: TimeStampObserver

        private class TimeStampObserver : Observers.IObserver<IVertexTimeStamperAlgorithm<TVertex>>

        {
            private readonly List<TVertex> _vertices;

            #region Constructors

            public TimeStampObserver(List<TVertex> vertices)
            {
                _vertices = vertices;
            }

            #endregion

            #region IObserver<IVertexTimeStamperAlgorithm<TVertex>> Members

            public IDisposable Attach(IVertexTimeStamperAlgorithm<TVertex> algorithm)
            {
                algorithm.DiscoverVertex += algorithm_DiscoverVertex;
                return new DisposableAction(
                    () => algorithm.DiscoverVertex -= algorithm_DiscoverVertex
                );
            }

            #endregion

            #region Event handlers

            private void algorithm_DiscoverVertex(TVertex v)
            {
                _vertices.Add(v);
            }

            #endregion
        }

        #endregion
    }
}