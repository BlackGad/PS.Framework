using System;
using System.Threading;
using System.Threading.Tasks;

namespace PS.Threading.Queue
{
    public interface IAsyncQueue<in TPayload, TResult> : IAsyncQueue<TPayload>
    {
        new IAsyncQueue<TPayload, TResult> Activate();

        new Task<TResult> Enqueue(TPayload payload, CancellationToken? token = null);
    }

    public interface IAsyncQueue<in TPayload> : IDisposable
    {
        IAsyncQueue<TPayload> Activate();

        Task Enqueue(TPayload payload, CancellationToken? token = null);
    }
}
