using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using PS.Extensions;

namespace PS.Tests.Extensions
{
    [TestFixture]
    public class EnumerateExtensionsTests
    {
        #region Members

        [Test]
        public void Enumerate_NotNull_Success()
        {
            IEnumerable source = new[] { 1, 2, 3 };
            var result = source.Enumerate();
            Assert.IsInstanceOf<IEnumerable<object>>(result);
            CollectionAssert.AreEqual(source, result);
        }

        [Test]
        public void Enumerate_Null_Success()
        {
            object source = null;
            var result = source.Enumerate();
            Assert.IsInstanceOf<IEnumerable<object>>(result);
        }

        #endregion
    }
}