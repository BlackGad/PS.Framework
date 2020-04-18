namespace PS.Graph.Algorithms.Services
{
    /// <summary>
    ///     Common services available to algorithm instances
    /// </summary>
    public interface IAlgorithmServices
    {
        #region Properties

        ICancelManager CancelManager { get; }

        #endregion
    }
}