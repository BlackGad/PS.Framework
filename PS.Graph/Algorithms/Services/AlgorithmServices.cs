namespace PS.Graph.Algorithms.Services
{
    internal class AlgorithmServices : IAlgorithmServices
    {
        private readonly IAlgorithmComponent _host;

        private ICancelManager _cancelManager;

        #region Constructors

        public AlgorithmServices(IAlgorithmComponent host)
        {
            _host = host;
        }

        #endregion

        #region IAlgorithmServices Members

        public ICancelManager CancelManager
        {
            get { return _cancelManager ?? (_cancelManager = _host.GetService<ICancelManager>()); }
        }

        #endregion
    }
}