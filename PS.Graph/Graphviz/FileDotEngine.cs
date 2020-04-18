using System;
using System.IO;
using PS.Graph.Graphviz.Dot;

namespace PS.Graph.Graphviz
{
    /// <summary>
    ///     Default dot engine implementation, writes dot code to disk
    /// </summary>
    public sealed class FileDotEngine : IDotEngine
    {
        #region IDotEngine Members

        public string Run(GraphvizImageType imageType, string dot, string outputFileName)
        {
            var output = outputFileName;
            if (!output.EndsWith(".dot", StringComparison.InvariantCultureIgnoreCase))
            {
                output = output + ".dot";
            }

            File.WriteAllText(output, dot);
            return output;
        }

        #endregion
    }
}