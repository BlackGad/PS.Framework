using System;
using System.Threading;

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

    internal class CancelManager : ICancelManager
    {
        private int _cancelling;

        #region ICancelManager Members

        public event EventHandler CancelRequested;

        public void Cancel()
        {
            var value = Interlocked.Increment(ref _cancelling);
            if (value == 0)
            {
                var eh = CancelRequested;
                eh?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool IsCancelling
        {
            get { return _cancelling > 0; }
        }

        /// <summary>
        ///     Raised when the cancel state has been reset
        /// </summary>
        public event EventHandler CancelReset;

        /// <summary>
        ///     Resets the cancel state
        /// </summary>
        public void ResetCancel()
        {
            var value = Interlocked.Exchange(ref _cancelling, 0);
            if (value != 0)
            {
                var eh = CancelReset;
                eh?.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}