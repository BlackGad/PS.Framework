using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using PS.Extensions;

namespace PS.Tests.Extensions
{
    [TestFixture]
    public class EnumerateExtensionsTests
    {
        [Test]
        public void Enumerate_NotNull_Success()
        {
            IEnumerable source = new[] { 1, 2, 3 };
            var result = source.Enumerate();
            CollectionAssert.AreEqual(source, result);
        }

        [Test]
        public void Enumerate_Null_Success()
        {
            object source = null;
            var result = source.Enumerate();
            Assert.That(result, Is.InstanceOf<IEnumerable<object>>());
        }
    }
}
