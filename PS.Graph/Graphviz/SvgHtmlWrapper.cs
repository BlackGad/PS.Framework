using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace PS.Graph.Graphviz
{
    public static class SvgHtmlWrapper
    {
        #region Constants

        private static readonly Regex SizeRegex = new Regex("<svg width=\"(?<Width>\\d+)px\" height=\"(?<Height>\\d+)px",
                                                            RegexOptions.ExplicitCapture
                                                            | RegexOptions.Multiline
                                                            | RegexOptions.Compiled
        );

        #endregion

        #region Static members

        public static string DumpHtml(Size size, string svgFileName)
        {
            var outputFile = $"{svgFileName}.html";
            using (var html = new StreamWriter(outputFile))
            {
                html.WriteLine("<html>");
                html.WriteLine("<body>");
                html.WriteLine("<object data=\"{0}\" type=\"image/svg+xml\" width=\"{1}\" height=\"{2}\">",
                               svgFileName,
                               size.Width,
                               size.Height);
                html.WriteLine("  <embed src=\"{0}\" type=\"image/svg+xml\" width=\"{1}\" height=\"{2}\">",
                               svgFileName,
                               size.Width,
                               size.Height);
                html.WriteLine("If you see this, you need to install a SVG viewer");
                html.WriteLine("  </embed>");
                html.WriteLine("</object>");
                html.WriteLine("</body>");
                html.WriteLine("</html>");
            }

            return outputFile;
        }

        public static Size ParseSize(string svg)
        {
            var m = SizeRegex.Match(svg);
            if (!m.Success)
            {
                return new Size(400, 400);
            }

            var size = int.Parse(m.Groups["Width"].Value);
            var height = int.Parse(m.Groups["Height"].Value);
            return new Size(size, height);
        }

        /// <summary>
        ///     Creates a HTML file that wraps the SVG and returns the file name
        /// </summary>
        /// <param name="svgFileName"></param>
        /// <returns></returns>
        public static string WrapSvg(string svgFileName)
        {
            using (var reader = new StreamReader(svgFileName))
            {
                var size = ParseSize(reader.ReadToEnd());
                reader.Close();
                return DumpHtml(size, svgFileName);
            }
        }

        #endregion
    }
}