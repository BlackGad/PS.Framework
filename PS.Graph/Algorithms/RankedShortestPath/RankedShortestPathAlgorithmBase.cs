using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using PS.Graph.Algorithms.Services;

namespace PS.Graph.Algorithms.RankedShortestPath
{
    public abstract class RankedShortestPathAlgorithmBase<TVertex, TEdge, TGraph> : RootedAlgorithmBase<TVertex, TGraph>
        where TEdge : IEdge<TVertex>
        where TGraph : IGraph
    {
        private List<IEnumerable<TEdge>> _computedShortestPaths;
        private int _shortestPathCount = 3;

        #region Constructors

        protected RankedShortestPathAlgorithmBase(
            IAlgorithmComponent host,
            TGraph visitedGraph,
            IDistanceRelaxer distanceRelaxer)
            : base(host, visitedGraph)
        {
            Contract.Requires(distanceRelaxer != null);

            DistanceRelaxer = distanceRelaxer;
        }

        #endregion

        #region Properties

        public int ComputedShortestPathCount
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() == ComputedShortestPaths.Count());

                return _computedShortestPaths?.Count ?? 0;
            }
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

        public int ShortestPathCount
        {
            get { return _shortestPathCount; }
            set
            {
                Contract.Requires(value > 1);
                Contract.Ensures(ShortestPathCount == value);

                _shortestPathCount = value;
            }
        }

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
            Contract.Requires(path != null);
            Contract.Requires(path.All(e => e != null));

            var pathArray = path.ToArray();
            _computedShortestPaths.Add(pathArray);
        }

        #endregion
    }
}