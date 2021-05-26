using System;
using System.Threading;
using System.Threading.Tasks;

namespace PS.Threading.Queue
{
    public interface IAsyncQueue<in TPayload, TResult> : IAsyncQueue<TPayload>
    {
        #region Members

        new IAsyncQueue<TPayload, TResult> Activate();
        new Task<TResult> Enqueue(TPayload payload, CancellationToken? token = null);

        #endregion
    }

    public interface IAsyncQueue<in TPayload> : IDisposable
    {
        #region Members

        IAsyncQueue<TPayload> Activate();
        Task Enqueue(TPayload payload, CancellationToken? token = null);

        #endregion
    }
}