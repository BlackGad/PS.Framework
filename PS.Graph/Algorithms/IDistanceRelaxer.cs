using System.Collections.Generic;

namespace PS.Graph.Algorithms
{
    public interface IDistanceRelaxer : IComparer<double>
    {
        #region Properties

        double InitialDistance { get; }

        #endregion

        #region Members

        double Combine(double distance, double weight);

        #endregion
    }
}