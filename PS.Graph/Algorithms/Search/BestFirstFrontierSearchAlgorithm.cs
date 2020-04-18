using System;
using System.Collections.Generic;
using PS.Graph.Algorithms.Services;
using PS.Graph.Collections;

namespace PS.Graph.Algorithms.Search
{
    /// <summary>
    ///     Best first frontier search
    /// </summary>
    /// <remarks>
    ///     Algorithm from Frontier Search, Korkf, Zhand, Thayer, Hohwald.
    /// </remarks>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    public sealed class BestFirstFrontierSearchAlgorithm<TVertex, TEdge> : RootedSearchAlgorithmBase<TVertex, IBidirectionalIncidenceGraph<TVertex, TEdge>>,
                                                                           ITreeBuilderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private readonly IDistanceRelaxer _distanceRelaxer;
        private readonly Func<TEdge, double> _edgeWeights;

        #region Constructors

        public BestFirstFrontierSearchAlgorithm(
            IAlgorithmComponent host,
            IBidirectionalIncidenceGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> edgeWeights,
            IDistanceRelaxer distanceRelaxer)
            : base(host, visitedGraph)
        {
            _edgeWeights = edgeWeights;
            _distanceRelaxer = distanceRelaxer;
        }

        #endregion

        #region Properties

        #if DEBUG
        public int OperatorMaxCount { get; private set; } = -1;
        #endif

        #endregion

        #region Override members

        protected override void InternalCompute()
        {
            if (!TryGetRootVertex(out var root))
            {
                throw new InvalidOperationException("root vertex not set");
            }

            if (!TryGetGoalVertex(out var goal))
            {
                throw new InvalidOperationException("goal vertex not set");
            }

            // little shortcut
            if (root.Equals(goal))
            {
                OnGoalReached();
                return; // found it
            }

            var cancelManager = Services.CancelManager;
            var open = new BinaryHeap<double, TVertex>(_distanceRelaxer.Compare);
            var operators = new Dictionary<TEdge, GraphColor>();
            var g = VisitedGraph;

            // (1) Place the initial node on Open, with all its operators marked unused.
            open.Add(0, root);
            foreach (var edge in g.OutEdges(root))
            {
                operators.Add(edge, GraphColor.White);
            }

            while (open.Count > 0)
            {
                if (cancelManager.IsCancelling) return;

                // (3) Else, choose an Open node n of lowest cost for expansion
                var entry = open.RemoveMinimum();
                var cost = entry.Key;
                var n = entry.Value;

                // (4) if node n is a goal node, terminate with success
                if (n.Equals(goal))
                {
                    OnGoalReached();
                    return;
                }

                // (5) else, expand node n, 
                // generating all successors n' reachable via unused legal operators
                // compute their cost and delete node n
                foreach (var edge in g.OutEdges(n))
                {
                    if (edge.IsSelfEdge<TVertex, TEdge>())
                    {
                        continue; // skip self-edges
                    }

                    var hasColor = operators.TryGetValue(edge, out var edgeColor);
                    if (!hasColor || edgeColor == GraphColor.White)
                    {
                        var weight = _edgeWeights(edge);
                        var nCost = _distanceRelaxer.Combine(cost, weight);

                        // (7) foreach neighboring node of n' mark the operator from n to n' as used
                        // (8) for each node n', if there is no copy of n' in open addit
                        // else save on open on the copy of n' with lowest cost. Mark as used all operators
                        // mak as used in any of the copies
                        operators[edge] = GraphColor.Gray;
                        if (open.MinimumUpdate(nCost, edge.Target))
                        {
                            OnTreeEdge(edge);
                        }
                    }
                    else
                    {
                        // edge already seen, remove it
                        operators.Remove(edge);
                    }
                }

                #if DEBUG
                OperatorMaxCount = Math.Max(OperatorMaxCount, operators.Count);
                #endif

                // (6) in a directed graph, generate each predecessor node n via an unused operator
                // and create dummy nodes for each with costs of infinity
                foreach (var edge in g.InEdges(n))
                {
                    if (operators.TryGetValue(edge, out var edgeColor) &&
                        edgeColor == GraphColor.Gray)
                    {
                        // delete node n
                        operators.Remove(edge);
                    }
                }
            }
        }

        #endregion

        #region ITreeBuilderAlgorithm<TVertex,TEdge> Members

        public event EdgeAction<TVertex, TEdge> TreeEdge;

        #endregion

        #region Members

        private void OnTreeEdge(TEdge edge)
        {
            var eh = TreeEdge;
            eh?.Invoke(edge);
        }

        #endregion
    }
}