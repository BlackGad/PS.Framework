using System;

namespace PS.Graph.Algorithms.Services
{
    public interface ICancelManager : IService
    {
        #region Properties

        /// <summary>
        ///     Gets a value indicating if a cancellation request is pending.
        /// </summary>
        /// <returns></returns>
        bool IsCancelling { get; }

        #endregion

        #region Events

        /// <summary>
        ///     Raised when the cancel method is called
        /// </summary>
        event EventHandler CancelRequested;

        /// <summary>
        ///     Raised when the cancel state has been reset
        /// </summary>
        event EventHandler CancelReset;

        #endregion

        #region Members

        /// <summary>
        ///     Requests the component to cancel its computation
        /// </summary>
        void Cancel();

        /// <summary>
        ///     Resets the cancel state
        /// </summary>
        void ResetCancel();

        #endregion
    }
}