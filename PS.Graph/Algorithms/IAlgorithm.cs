namespace PS.Graph.Algorithms
{
    public interface IAlgorithm<out TGraph> : IComputation
    {
        #region Properties

        TGraph VisitedGraph { get; }

        #endregion
    }
}