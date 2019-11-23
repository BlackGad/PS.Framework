using System;

namespace PS.Patterns.Aware
{
    public interface ISubscriptionAware
    {
        #region Members

        void Subscribe<T>(EventHandler<T> handler);
        void Unsubscribe<T>(EventHandler<T> handler);

        #endregion
    }
}