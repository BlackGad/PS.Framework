using System;

namespace PS.TestReferences.DynamicSubscriptionTests
{
    internal class Test
    {
        #region Events

        public event EventHandler<EventArgs> TestEvent;

        #endregion

        #region Members

        public void RaiseEvent()
        {
            TestEvent?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}