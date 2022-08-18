using System;

namespace PS.Patterns.Aware
{
    public interface IInvokeEventHandlerAware
    {
        void InvokeEventHandler(Delegate @delegate, object sender, object args);
    }
}
