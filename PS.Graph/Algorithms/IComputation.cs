using System;
using System.Diagnostics.Contracts;

namespace PS.Graph.Algorithms
{
    [ContractClass(typeof(Contracts.ComputationContract))]
    public interface IComputation
    {
        #region Properties

        ComputationState State { get; }
        object SyncRoot { get; }

        #endregion

        #region Events

        event EventHandler Aborted;
        event EventHandler Finished;
        event EventHandler Started;

        event EventHandler StateChanged;

        #endregion

        #region Members

        void Abort();

        void Compute();

        #endregion
    }
}