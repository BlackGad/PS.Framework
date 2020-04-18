using System.Diagnostics.Contracts;

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

    internal class AlgorithmServices : IAlgorithmServices
    {
        private readonly IAlgorithmComponent _host;

        private ICancelManager _cancelManager;

        #region Constructors

        public AlgorithmServices(IAlgorithmComponent host)
        {
            Contract.Requires(host != null);

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