using System.Diagnostics.Contracts;

namespace PS.Graph.Algorithms
{
    [ContractClass(typeof(Contracts.AlgorithmContract<>))]
    public interface IAlgorithm<out TGraph> : IComputation
    {
        #region Properties

        TGraph VisitedGraph { get; }

        #endregion
    }
}