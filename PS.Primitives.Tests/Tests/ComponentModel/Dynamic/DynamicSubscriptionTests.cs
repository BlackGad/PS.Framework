﻿using System;
using NUnit.Framework;
using PS.ComponentModel.Dynamic;
using PS.TestReferences.DynamicSubscriptionTests;

namespace PS.Tests.ComponentModel.Dynamic
{
    [TestFixture]
    public class DynamicSubscriptionTests
    {
        #region Members

        [Test]
        public void Subscription_Test()
        {
            var delegateCallCount = 0;

            var subscription = new DynamicSubscription<Test, EventHandler<EventArgs>>((test, handler) => test.TestEvent += handler,
                                                                                      (test, handler) => test.TestEvent -= handler);

            var instance = new Test();
            subscription.Subscribe(instance, (sender, args) => { delegateCallCount++; });
            instance.RaiseEvent();
            subscription.Unsubscribe(instance);
            instance.RaiseEvent();

            Assert.AreEqual(1, delegateCallCount);
        }

        #endregion
    }
}