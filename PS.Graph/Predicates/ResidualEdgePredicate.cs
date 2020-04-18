using System.Collections.Generic;

namespace PS.Graph.Predicates
{
    public sealed class ResidualEdgePredicate<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Constructors

        public ResidualEdgePredicate(
            IDictionary<TEdge, double> residualCapacities)
        {
            ResidualCapacities = residualCapacities;
        }

        #endregion

        #region Properties

        public IDictionary<TEdge, double> ResidualCapacities { get; }

        #endregion

        #region Members

        public bool Test(TEdge e)
        {
            return 0 < ResidualCapacities[e];
        }

        #endregion
    }
}