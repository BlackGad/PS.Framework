using System;
using System.Diagnostics.Contracts;

namespace PS.Graph.Algorithms.Contracts
{
    [ContractClassFor(typeof(IAlgorithm<>))]
    internal abstract class AlgorithmContract<TGraph> : IAlgorithm<TGraph>
    {
        #region IAlgorithm<TGraph> Members

        TGraph IAlgorithm<TGraph>.VisitedGraph
        {
            get
            {
                Contract.Ensures(Contract.Result<TGraph>() != null);

                return default;
            }
        }

        object IComputation.SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        ComputationState IComputation.State
        {
            get { throw new NotImplementedException(); }
        }

        void IComputation.Compute()
        {
            throw new NotImplementedException();
        }

        void IComputation.Abort()
        {
            throw new NotImplementedException();
        }

        event EventHandler IComputation.StateChanged
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event EventHandler IComputation.Started
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event EventHandler IComputation.Finished
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event EventHandler IComputation.Aborted
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        #endregion
    }
}