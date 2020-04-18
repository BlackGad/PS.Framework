using System;
using System.Diagnostics.Contracts;
using PS.Graph.Graphviz.Dot;

namespace PS.Graph.Graphviz
{
    public interface IDotEngine
    {
        #region Members

        string Run(
            GraphvizImageType imageType,
            string dot,
            string outputFileName);

        #endregion
    }

    internal abstract class DotEngineContract : IDotEngine
    {
        #region IDotEngine Members

        string IDotEngine.Run(GraphvizImageType imageType, string dot, string outputFileName)
        {
            Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<string>()));

            return null;
        }

        #endregion
    }
}