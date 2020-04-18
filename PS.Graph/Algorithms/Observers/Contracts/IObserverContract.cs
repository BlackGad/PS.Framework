using System;
using System.Diagnostics.Contracts;

namespace PS.Graph.Algorithms.Observers.Contracts
{
    internal abstract class ObserverContract<TAlgorithm> : IObserver<TAlgorithm>
    {
        #region IObserver<TAlgorithm> Members

        IDisposable IObserver<TAlgorithm>.Attach(TAlgorithm algorithm)
        {
            Contract.Ensures(Contract.Result<IDisposable>() != null);

            return default;
        }

        #endregion
    }
}