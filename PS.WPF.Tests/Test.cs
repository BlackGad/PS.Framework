using System.Threading;
using NUnit.Framework;

namespace PS.WPF.Tests
{
    [TestFixture]
    [RequiresThread(ApartmentState.STA)]
    public class Test
    {
        [Test]
        public static void TestMethod()
        {
            Assert.Inconclusive();
        }
    }
}
