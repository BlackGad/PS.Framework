using System.Collections.Generic;
using PS.Graph.Algorithms.Services;

namespace PS.Graph.Algorithms.RankedShortestPath
{
    public abstract class RankedShortestPathAlgorithmBase<TVertex, TEdge, TGraph> : RootedAlgorithmBase<TVertex, TGraph>
        where TEdge : IEdge<TVertex>
        where TGraph : IGraph
    {
        private List<IEnumerable<TEdge>> _computedShortestPaths;

        #region Constructors

        protected RankedShortestPathAlgorithmBase(
            IAlgorithmComponent host,
            TGraph visitedGraph,
            IDistanceRelaxer distanceRelaxer)
            : base(host, visitedGraph)
        {
            DistanceRelaxer = distanceRelaxer;
        }

        #endregion

        #region Properties

        public int ComputedShortestPathCount
        {
            get { return _computedShortestPaths?.Count ?? 0; }
        }

        public IEnumerable<IEnumerable<TEdge>> ComputedShortestPaths
        {
            get
            {
                if (_computedShortestPaths == null)
                {
                    yield break;
                }

                foreach (var path in _computedShortestPaths)
                {
                    yield return path;
                }
            }
        }

        public IDistanceRelaxer DistanceRelaxer { get; }

        public int ShortestPathCount { get; set; } = 3;

        #endregion

        #region Override members

        protected override void Initialize()
        {
            base.Initialize();
            _computedShortestPaths = new List<IEnumerable<TEdge>>(ShortestPathCount);
        }

        #endregion

        #region Members

        protected void AddComputedShortestPath(List<TEdge> path)
        {
            var pathArray = path.ToArray();
            _computedShortestPaths.Add(pathArray);
        }

        #endregion
    }
}