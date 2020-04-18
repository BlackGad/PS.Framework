using System;
using System.Diagnostics.Contracts;

namespace PS.Graph.Algorithms.Observers.Contracts
{
    [ContractClassFor(typeof(IObserver<>))]
    internal abstract class ObserverContract<TAlgorithm> : IObserver<TAlgorithm>
    {
        #region IObserver<TAlgorithm> Members

        IDisposable IObserver<TAlgorithm>.Attach(TAlgorithm algorithm)
        {
            Contract.Requires(algorithm != null);
            Contract.Ensures(Contract.Result<IDisposable>() != null);

            return default;
        }

        #endregion
    }
}