using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using PS.Graph.Algorithms.Search;
using PS.Graph.Algorithms.Services;
using PS.Graph.Collections;

namespace PS.Graph.Algorithms
{
    /// <summary>
    ///     Offline least common ancestor in a rooted tre
    /// </summary>
    /// <remarks>
    ///     Reference:
    ///     Gabow, H. N. and Tarjan, R. E. 1983. A linear-time algorithm for a special case
    ///     of disjoint set union. In Proceedings of the Fifteenth Annual ACM Symposium
    ///     on theory of Computing STOC '83. ACM, New York, NY, 246-251.
    ///     DOI= http://doi.acm.org/10.1145/800061.808753
    /// </remarks>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    public sealed class TarjanOfflineLeastCommonAncestorAlgorithm<TVertex, TEdge> : RootedAlgorithmBase<TVertex, IVertexListGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private readonly Dictionary<SEquatableEdge<TVertex>, TVertex> _ancestors =
            new Dictionary<SEquatableEdge<TVertex>, TVertex>();

        private SEquatableEdge<TVertex>[] _pairs;

        #region Constructors

        public TarjanOfflineLeastCommonAncestorAlgorithm(
            IVertexListGraph<TVertex, TEdge> visitedGraph)
            : this(null, visitedGraph)
        {
        }

        public TarjanOfflineLeastCommonAncestorAlgorithm(
            IAlgorithmComponent host,
            IVertexListGraph<TVertex, TEdge> visitedGraph)
            : base(host, visitedGraph)
        {
        }

        #endregion

        #region Properties

        public IDictionary<SEquatableEdge<TVertex>, TVertex> Ancestors
        {
            get { return _ancestors; }
        }

        #endregion

        #region Override members

        protected override void Initialize()
        {
            base.Initialize();
            _ancestors.Clear();
        }

        protected override void InternalCompute()
        {
            if (!TryGetRootVertex(out var root))
            {
                throw new InvalidOperationException("root vertex not set");
            }

            if (_pairs == null)
            {
                throw new InvalidOperationException("pairs not set");
            }

            var gPair = _pairs.ToAdjacencyGraph();
            var disjointSet = new ForestDisjointSet<TVertex>();
            var vAncestors = new Dictionary<TVertex, TVertex>();
            var dfs = new DepthFirstSearchAlgorithm<TVertex, TEdge>(this, VisitedGraph, new Dictionary<TVertex, GraphColor>(VisitedGraph.VertexCount));

            dfs.InitializeVertex += v => disjointSet.MakeSet(v);
            dfs.DiscoverVertex += v => vAncestors[v] = v;
            dfs.TreeEdge += edge =>
            {
                disjointSet.Union(edge.Source, edge.Target);
                vAncestors[disjointSet.FindSet(edge.Source)] = edge.Source;
            };
            dfs.FinishVertex += v =>
            {
                foreach (var e in gPair.OutEdges(v))
                {
                    if (dfs.VertexColors[e.Target] == GraphColor.Black)
                    {
                        _ancestors[e.ToVertexPair<TVertex, SEquatableEdge<TVertex>>()] = vAncestors[disjointSet.FindSet(e.Target)];
                    }
                }
            };

            // go!
            dfs.Compute(root);
        }

        #endregion

        #region Members

        public void Compute(TVertex root, IEnumerable<SEquatableEdge<TVertex>> pairs)
        {
            Contract.Requires(root != null);
            Contract.Requires(pairs != null);

            _pairs = pairs.ToArray();
            Compute(root);
        }

        public void SetVertexPairs(IEnumerable<SEquatableEdge<TVertex>> pairs)
        {
            Contract.Requires(pairs != null);

            _pairs = new List<SEquatableEdge<TVertex>>(pairs).ToArray();
        }

        public bool TryGetVertexPairs(out IEnumerable<SEquatableEdge<TVertex>> pairs)
        {
            pairs = _pairs;
            return pairs != null;
        }

        #endregion
    }
}