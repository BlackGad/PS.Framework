using System;
using System.Collections.Generic;
using PS.Graph.Algorithms.Services;

namespace PS.Graph.Algorithms.ShortestPath
{
    [Serializable]
    public abstract class ShortestPathAlgorithmBase<TVertex, TEdge, TGraph> : RootedAlgorithmBase<TVertex, TGraph>,
                                                                              ITreeBuilderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
        where TGraph : IVertexSet<TVertex>
    {
        #region Constructors

        protected ShortestPathAlgorithmBase(
            IAlgorithmComponent host,
            TGraph visitedGraph,
            Func<TEdge, double> weights
        )
            : this(host, visitedGraph, weights, DistanceRelaxers.ShortestDistance)
        {
        }

        protected ShortestPathAlgorithmBase(
            IAlgorithmComponent host,
            TGraph visitedGraph,
            Func<TEdge, double> weights,
            IDistanceRelaxer distanceRelaxer
        )
            : base(host, visitedGraph)
        {
            Weights = weights;
            DistanceRelaxer = distanceRelaxer;
        }

        #endregion

        #region Properties

        public IDistanceRelaxer DistanceRelaxer { get; }

        public Dictionary<TVertex, double> Distances { get; private set; }

        public Dictionary<TVertex, GraphColor> VertexColors { get; private set; }

        public Func<TEdge, double> Weights { get; }

        #endregion

        #region Override members

        protected override void Initialize()
        {
            base.Initialize();
            VertexColors = new Dictionary<TVertex, GraphColor>(VisitedGraph.VertexCount);
            Distances = new Dictionary<TVertex, double>(VisitedGraph.VertexCount);
        }

        #endregion

        #region ITreeBuilderAlgorithm<TVertex,TEdge> Members

        /// <summary>
        ///     Invoked when the distance label for the target vertex is decreased.
        ///     The edge that participated in the last relaxation for vertex v is
        ///     an edge in the shortest paths tree.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> TreeEdge;

        #endregion

        #region Members

        public GraphColor GetVertexColor(TVertex vertex)
        {
            return
                VertexColors[vertex];
        }

        public bool TryGetDistance(TVertex vertex, out double distance)
        {
            return Distances.TryGetValue(vertex, out distance);
        }

        protected Func<TVertex, double> DistancesIndexGetter()
        {
            return AlgorithmExtensions.GetIndexer(Distances);
        }

        /// <summary>
        ///     Raises the <see cref="TreeEdge" /> event.
        /// </summary>
        /// <param name="e">edge that raised the event</param>
        protected virtual void OnTreeEdge(TEdge e)
        {
            var eh = TreeEdge;
            eh?.Invoke(e);
        }

        protected bool Relax(TEdge e)
        {
            var source = e.Source;
            var target = e.Target;
            if (source.Equals(target))
            {
                return false;
            }

            var du = Distances[source];
            var dv = Distances[target];
            var we = Weights(e);

            var relaxer = DistanceRelaxer;
            var duwe = relaxer.Combine(du, we);
            if (relaxer.Compare(duwe, dv) < 0)
            {
                Distances[target] = duwe;
                return true;
            }

            return false;
        }

        #endregion
    }
}