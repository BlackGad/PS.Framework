using System;

namespace PS.Patterns.Aware
{
    public interface IInvokeEventHandlerAware
    {
        #region Members

        void InvokeEventHandler(Delegate @delegate, object sender, object args);

        #endregion
    }
}