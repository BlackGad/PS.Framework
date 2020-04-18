using System;
using System.Collections.Generic;
using System.Linq;
using PS.Graph.Algorithms.Services;

namespace PS.Graph.Algorithms.RandomWalks
{
    /// <summary>
    ///     Wilson-Propp Cycle-Popping Algorithm for Random Tree Generation.
    /// </summary>
    [Serializable]
    public sealed class CyclePoppingRandomTreeAlgorithm<TVertex, TEdge> : RootedAlgorithmBase<TVertex, IVertexListGraph<TVertex, TEdge>>,
                                                                          IVertexColorizerAlgorithm<TVertex>,
                                                                          ITreeBuilderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private Random _rnd = new Random((int)DateTime.Now.Ticks);

        #region Constructors

        public CyclePoppingRandomTreeAlgorithm(
            IVertexListGraph<TVertex, TEdge> visitedGraph)
            : this(visitedGraph, new NormalizedMarkovEdgeChain<TVertex, TEdge>())
        {
        }

        public CyclePoppingRandomTreeAlgorithm(
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            IMarkovEdgeChain<TVertex, TEdge> edgeChain)
            : this(null, visitedGraph, edgeChain)
        {
        }

        public CyclePoppingRandomTreeAlgorithm(
            IAlgorithmComponent host,
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            IMarkovEdgeChain<TVertex, TEdge> edgeChain
        )
            : base(host, visitedGraph)
        {
            EdgeChain = edgeChain;
        }

        #endregion

        #region Properties

        public IMarkovEdgeChain<TVertex, TEdge> EdgeChain { get; }

        /// <summary>
        ///     Gets or sets the random number generator used in <c>RandomTree</c>.
        /// </summary>
        /// <value>
        ///     <see cref="Random" /> number generator
        /// </value>
        public Random Rnd
        {
            get { return _rnd; }
            set { _rnd = value; }
        }

        public IDictionary<TVertex, TEdge> Successors { get; } = new Dictionary<TVertex, TEdge>();

        public IDictionary<TVertex, GraphColor> VertexColors { get; } = new Dictionary<TVertex, GraphColor>();

        #endregion

        #region Events

        public event VertexAction<TVertex> ClearTreeVertex;

        public event VertexAction<TVertex> FinishVertex;

        public event VertexAction<TVertex> InitializeVertex;

        #endregion

        #region Override members

        protected override void Initialize()
        {
            base.Initialize();

            Successors.Clear();
            VertexColors.Clear();
            foreach (var v in VisitedGraph.Vertices)
            {
                VertexColors.Add(v, GraphColor.White);
                OnInitializeVertex(v);
            }
        }

        protected override void InternalCompute()
        {
            if (!TryGetRootVertex(out var rootVertex))
            {
                throw new InvalidOperationException("RootVertex not specified");
            }

            var cancelManager = Services.CancelManager;
            // process root
            ClearTree(rootVertex);
            SetInTree(rootVertex);

            foreach (var i in VisitedGraph.Vertices)
            {
                if (cancelManager.IsCancelling) break;
                // first pass exploring
                {
                    var visited = new Dictionary<TEdge, int>();
                    var u = i;
                    while (NotInTree(u) &&
                           TryGetSuccessor(visited, u, out var successor))
                    {
                        visited[successor] = 0;
                        Tree(u, successor);
                        if (!TryGetNextInTree(u, out u))
                        {
                            break;
                        }
                    }
                }

                // second pass, coloring
                {
                    var u = i;
                    while (NotInTree(u))
                    {
                        SetInTree(u);
                        if (!TryGetNextInTree(u, out u))
                        {
                            break;
                        }
                    }
                }
            }
        }

        #endregion

        #region ITreeBuilderAlgorithm<TVertex,TEdge> Members

        public event EdgeAction<TVertex, TEdge> TreeEdge;

        #endregion

        #region IVertexColorizerAlgorithm<TVertex> Members

        public GraphColor GetVertexColor(TVertex v)
        {
            return VertexColors[v];
        }

        #endregion

        #region Members

        public void RandomTree()
        {
            var cancelManager = Services.CancelManager;

            double eps = 1;
            bool success;
            do
            {
                if (cancelManager.IsCancelling) break;

                eps /= 2;
                success = Attempt(eps);
            } while (!success);
        }

        public void RandomTreeWithRoot(TVertex root)
        {
            SetRootVertex(root);
            Compute();
        }

        private bool Attempt(double eps)
        {
            Initialize();
            var numRoots = 0;
            var cancelManager = Services.CancelManager;

            foreach (var i in VisitedGraph.Vertices)
            {
                if (cancelManager.IsCancelling) break;

                // first pass exploring
                {
                    var visited = new Dictionary<TEdge, int>();
                    var u = i;
                    while (NotInTree(u))
                    {
                        if (Chance(eps))
                        {
                            ClearTree(u);
                            SetInTree(u);
                            ++numRoots;
                            if (numRoots > 1)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (!TryGetSuccessor(visited, u, out var successor))
                            {
                                break;
                            }

                            visited[successor] = 0;
                            Tree(u, successor);
                            if (!TryGetNextInTree(u, out u))
                            {
                                break;
                            }
                        }
                    }
                }

                // second pass, coloring
                {
                    var u = i;
                    while (NotInTree(u))
                    {
                        SetInTree(u);
                        if (!TryGetNextInTree(u, out u))
                        {
                            break;
                        }
                    }
                }
            }

            return true;
        }

        private bool Chance(double eps)
        {
            return _rnd.NextDouble() <= eps;
        }

        private void ClearTree(TVertex u)
        {
            Successors[u] = default;
            OnClearTreeVertex(u);
        }

        private bool NotInTree(TVertex u)
        {
            var color = VertexColors[u];
            return color == GraphColor.White;
        }

        private void OnClearTreeVertex(TVertex v)
        {
            var eh = ClearTreeVertex;
            eh?.Invoke(v);
        }

        private void OnFinishVertex(TVertex v)
        {
            var eh = FinishVertex;
            eh?.Invoke(v);
        }

        private void OnInitializeVertex(TVertex v)
        {
            var eh = InitializeVertex;
            eh?.Invoke(v);
        }

        private void OnTreeEdge(TEdge e)
        {
            var eh = TreeEdge;
            eh?.Invoke(e);
        }

        private void SetInTree(TVertex u)
        {
            VertexColors[u] = GraphColor.Black;
            OnFinishVertex(u);
        }

        private void Tree(TVertex u, TEdge next)
        {
            Successors[u] = next;
            OnTreeEdge(next);
        }

        private bool TryGetNextInTree(TVertex u, out TVertex next)
        {
            if (Successors.TryGetValue(u, out var nextEdge))
            {
                next = nextEdge.Target;
                return true;
            }

            next = default;
            return false;
        }

        private bool TryGetSuccessor(Dictionary<TEdge, int> visited, TVertex u, out TEdge successor)
        {
            var outEdges = VisitedGraph.OutEdges(u);
            var edges = outEdges.Where(e => !visited.ContainsKey(e));
            return EdgeChain.TryGetSuccessor(edges, u, out successor);
        }

        #endregion
    }
}