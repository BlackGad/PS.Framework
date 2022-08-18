using System;

namespace PS.TestReferences.DynamicSubscriptionTests
{
    internal class Test
    {
        public event EventHandler<EventArgs> TestEvent;

        public void RaiseEvent()
        {
            TestEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
