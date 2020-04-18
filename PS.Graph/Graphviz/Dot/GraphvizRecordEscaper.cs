using System.Text.RegularExpressions;

namespace PS.Graph.Graphviz.Dot
{
    public sealed class GraphvizRecordEscaper
    {
        private readonly Regex _escapeRegExp = new Regex("(?<Eol>\\n)|(?<Common>\\[|\\]|\\||<|>|\"| )",
                                                         RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.Multiline);

        #region Members

        public string Escape(string text)
        {
            return _escapeRegExp.Replace(text, MatchEvaluator);
        }

        public string MatchEvaluator(Match m)
        {
            if (m.Groups["Common"] != null)
            {
                return $@"\{m.Value}";
            }

            return @"\n";
        }

        #endregion
    }
}