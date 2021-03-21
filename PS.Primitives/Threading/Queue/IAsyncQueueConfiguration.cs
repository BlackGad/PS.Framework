using System;
using System.Threading;
using PS.Patterns;

namespace PS.Threading.Queue
{
    public interface IAsyncQueueConfiguration<in TPayload, TResult> : IFluentBuilder<IAsyncQueue<TPayload, TResult>>
    {
        #region Members

        IAsyncQueueConfiguration<TPayload, TResult> ApartmentState(ApartmentState apartmentState);
        IAsyncQueueConfiguration<TPayload, TResult> CancellationHandler(Action cancellationHandler);
        IAsyncQueueConfiguration<TPayload, TResult> CancellationToken(CancellationToken? cancellationToken);
        IAsyncQueueConfiguration<TPayload, TResult> ConsumePeriod(TimeSpan? consumePeriod);
        IAsyncQueueConfiguration<TPayload, TResult> ConsumeTimedOutHandler(Func<CancellationToken, TPayload> consumeTimedOutHandler);
        IAsyncQueueConfiguration<TPayload, TResult> ErrorHandler(Action<Exception> errorHandler);
        IAsyncQueueConfiguration<TPayload, TResult> SuppressMessageExceptionForwarding(bool suppressMessageExceptionForwarding);

        #endregion
    }
}