using System;
using System.Collections.Generic;

namespace PS.Graph.Predicates
{
    [Serializable]
    public sealed class ReversedResidualEdgePredicate<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Constructors

        public ReversedResidualEdgePredicate(
            IDictionary<TEdge, double> residualCapacities,
            IDictionary<TEdge, TEdge> reversedEdges)
        {
            ResidualCapacities = residualCapacities;
            ReversedEdges = reversedEdges;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Residual capacities map
        /// </summary>
        public IDictionary<TEdge, double> ResidualCapacities { get; }

        /// <summary>
        ///     Reversed edges map
        /// </summary>
        public IDictionary<TEdge, TEdge> ReversedEdges { get; }

        #endregion

        #region Members

        public bool Test(TEdge e)
        {
            return 0 < ResidualCapacities[ReversedEdges[e]];
        }

        #endregion
    }
}