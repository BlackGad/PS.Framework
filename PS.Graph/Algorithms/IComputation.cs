using System;

namespace PS.Graph.Algorithms
{
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