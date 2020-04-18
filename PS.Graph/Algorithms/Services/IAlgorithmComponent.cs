namespace PS.Graph.Algorithms.Services
{
    public interface IAlgorithmComponent
    {
        #region Properties

        IAlgorithmServices Services { get; }

        #endregion

        #region Members

        T GetService<T>()
            where T : IService;

        bool TryGetService<T>(out T service)
            where T : IService;

        #endregion
    }
}