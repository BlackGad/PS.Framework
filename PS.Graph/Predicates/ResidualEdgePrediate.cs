using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace PS.Graph.Predicates
{
    public sealed class ResidualEdgePredicate<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Constructors

        public ResidualEdgePredicate(
            IDictionary<TEdge, double> residualCapacities)
        {
            Contract.Requires(residualCapacities != null);

            ResidualCapacities = residualCapacities;
        }

        #endregion

        #region Properties

        public IDictionary<TEdge, double> ResidualCapacities { get; }

        #endregion

        #region Members

        public bool Test(TEdge e)
        {
            Contract.Requires(e != null);
            return 0 < ResidualCapacities[e];
        }

        #endregion
    }
}