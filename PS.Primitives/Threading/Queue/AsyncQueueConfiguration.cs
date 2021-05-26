using System;
using System.Threading;
using PS.Patterns;

namespace PS.Threading.Queue
{
    public class AsyncQueueConfiguration<TPayload, TResult> : ICloneable,
                                                              IAsyncQueueConfiguration<TPayload, TResult>
    {
        #region Constructors

        public AsyncQueueConfiguration()
        {
            SuppressMessageExceptionForwarding = true;
            ApartmentState = ApartmentState.MTA;
        }

        #endregion

        #region Properties

        public ApartmentState ApartmentState { get; set; }
        public Action CancellationHandler { get; set; }
        public CancellationToken? CancellationToken { get; set; }
        public TimeSpan? ConsumePeriod { get; set; }
        public Func<CancellationToken, TPayload> ConsumeTimedOutHandler { get; set; }
        public Action<Exception> ErrorHandler { get; set; }
        public Func<TPayload, CancellationToken, TResult> ProcessHandler { get; set; }
        public bool SuppressMessageExceptionForwarding { get; set; }

        #endregion

        #region IAsyncQueueConfiguration<TPayload,TResult> Members

        IAsyncQueueConfiguration<TPayload, TResult> IAsyncQueueConfiguration<TPayload, TResult>.ApartmentState(ApartmentState apartmentState)
        {
            ApartmentState = apartmentState;
            return this;
        }

        IAsyncQueueConfiguration<TPayload, TResult> IAsyncQueueConfiguration<TPayload, TResult>.CancellationHandler(Action cancellationHandler)
        {
            CancellationHandler = cancellationHandler;
            return this;
        }

        IAsyncQueueConfiguration<TPayload, TResult> IAsyncQueueConfiguration<TPayload, TResult>.CancellationToken(CancellationToken? cancellationToken)
        {
            CancellationToken = cancellationToken;
            return this;
        }

        IAsyncQueueConfiguration<TPayload, TResult> IAsyncQueueConfiguration<TPayload, TResult>.ConsumePeriod(TimeSpan? consumePeriod)
        {
            ConsumePeriod = consumePeriod;
            return this;
        }

        IAsyncQueueConfiguration<TPayload, TResult> IAsyncQueueConfiguration<TPayload, TResult>.ConsumeTimedOutHandler(Func<CancellationToken, TPayload> handler)
        {
            ConsumeTimedOutHandler = handler;
            return this;
        }

        IAsyncQueueConfiguration<TPayload, TResult> IAsyncQueueConfiguration<TPayload, TResult>.ErrorHandler(Action<Exception> errorHandler)
        {
            ErrorHandler = errorHandler;
            return this;
        }

        IAsyncQueueConfiguration<TPayload, TResult> IAsyncQueueConfiguration<TPayload, TResult>.SuppressMessageExceptionForwarding(bool value)
        {
            SuppressMessageExceptionForwarding = value;
            return this;
        }

        IAsyncQueue<TPayload, TResult> IFluentBuilder<IAsyncQueue<TPayload, TResult>>.Create()
        {
            return new AsyncQueue<TPayload, TResult>(Clone());
        }

        #endregion

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        #region Members

        public AsyncQueueConfiguration<TPayload, TResult> Clone()
        {
            return new AsyncQueueConfiguration<TPayload, TResult>
            {
                ProcessHandler = ProcessHandler,
                CancellationHandler = CancellationHandler,
                ConsumeTimedOutHandler = ConsumeTimedOutHandler,
                ErrorHandler = ErrorHandler,
                SuppressMessageExceptionForwarding = SuppressMessageExceptionForwarding,
                ConsumePeriod = ConsumePeriod,
                CancellationToken = CancellationToken
            };
        }

        #endregion
    }
}