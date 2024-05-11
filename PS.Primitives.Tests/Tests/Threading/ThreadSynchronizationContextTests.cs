using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using PS.Threading;

namespace PS.Tests.Threading
{
    [TestFixture]
    public class ThreadSynchronizationContextTests
    {
        [Test]
        public void Sequential_Post_Success()
        {
            var expectedSequence = new List<object>();
            var resultSequence = new List<object>();

            using (var synchronizationContext = new ThreadSynchronizationContext(ApartmentState.MTA))
            {
                for (var i = 0; i < 5; i++)
                {
                    expectedSequence.Add(i);

                    void SendOrPostCallback(object state)
                    {
                        lock (resultSequence)
                        {
                            resultSequence.Add(state);
                        }
                    }

                    synchronizationContext.Post(SendOrPostCallback, i);
                }

                var finishEvent = new ManualResetEvent(false);
                synchronizationContext.Post(state => finishEvent.Set(), null);
                finishEvent.WaitOne();
            }

            CollectionAssert.AreEqual(expectedSequence, resultSequence);
        }

        [Test]
        public void Sequential_PostSendInsert_Success()
        {
            var expectedSequence = new List<object>();
            var resultSequence = new List<object>();

            using (var synchronizationContext = new ThreadSynchronizationContext(ApartmentState.MTA))
            {
                for (var i = 0; i < 5; i++)
                {
                    expectedSequence.Add(i);

                    void SendOrPostCallback(object state)
                    {
                        lock (resultSequence)
                        {
                            resultSequence.Add(state);
                        }

                        Thread.Sleep(100);
                    }

                    synchronizationContext.Post(SendOrPostCallback, i);
                }

                var finishEvent = new ManualResetEvent(false);
                synchronizationContext.Post(state => finishEvent.Set(), null);

                synchronizationContext.Send(state => CollectionAssert.AreNotEqual(expectedSequence, resultSequence), null);

                finishEvent.WaitOne();
            }

            CollectionAssert.AreEqual(expectedSequence, resultSequence);
        }

        [Test]
        public void Sequential_Send_Success()
        {
            var expectedSequence = new List<object>();
            var resultSequence = new List<object>();

            using (var synchronizationContext = new ThreadSynchronizationContext(ApartmentState.MTA))
            {
                for (var i = 0; i < 5; i++)
                {
                    expectedSequence.Add(i);

                    void SendOrPostCallback(object state)
                    {
                        lock (resultSequence)
                        {
                            resultSequence.Add(state);
                        }
                    }

                    synchronizationContext.Send(SendOrPostCallback, i);
                }
            }

            CollectionAssert.AreEqual(expectedSequence, resultSequence);
        }

        [Test]
        public void ThrowException_Post_Success()
        {
            var expectedException = new Exception();
            using (var synchronizationContext = new ThreadSynchronizationContext(ApartmentState.MTA))
            {
                synchronizationContext.Post(state => throw expectedException, null);
                Thread.Sleep(1000);
            }
        }

        [Test]
        public void ThrowException_PostSend_Failed()
        {
            var expectedException = new Exception();
            using (var synchronizationContext = new ThreadSynchronizationContext(ApartmentState.MTA))
            {
                synchronizationContext.Post(state => throw expectedException, null);
                Thread.Sleep(1000);

                var resultException = Assert.Catch(expectedException.GetType(), () => synchronizationContext.Send(state => throw expectedException, null));
                Assert.That(resultException, Is.EqualTo(expectedException));
            }
        }

        [Test]
        public void ThrowException_Send_Failed()
        {
            var expectedException = new Exception();
            using (var synchronizationContext = new ThreadSynchronizationContext(ApartmentState.MTA))
            {
                var resultException = Assert.Catch(expectedException.GetType(), () => synchronizationContext.Send(state => throw expectedException, null));
                Assert.That(resultException, Is.EqualTo(expectedException));
            }
        }
    }
}
