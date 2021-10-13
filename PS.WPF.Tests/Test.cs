using System.Linq;
using System.Threading;
using System.Xml.Linq;
using System.Xml.XPath;
using NUnit.Framework;

namespace PS.WPF.Tests
{
    [TestFixture]
    [RequiresThread(ApartmentState.STA)]
    public class Test
    {
        #region Static members

        [Test]
        public static void TestMethod()
        {
            var configurationFilePath = @"C:\Projects\GitHub\BlackGad\PS.Framework\PS.WPF\Theme\ThemePalette.xml";
            var configuration = XDocument.Load(configurationFilePath);
            var colors = configuration.XPathSelectElements("/Colors/Color[@Name and @Name[not(.=\"\")]]").ToList();

            foreach (var color in colors)
            {
                var name = color.Attribute(XName.Get("Name")).Value;
                ;
                //Generate
            }

            Assert.Inconclusive();
        }

        #endregion
    }
}