using System;
using System.Diagnostics.Contracts;
using PS.Graph.Graphviz.Dot;

namespace PS.Graph.Graphviz
{
    [ContractClass(typeof(DotEngineContract))]
    public interface IDotEngine
    {
        #region Members

        string Run(
            GraphvizImageType imageType,
            string dot,
            string outputFileName);

        #endregion
    }

    [ContractClassFor(typeof(IDotEngine))]
    internal abstract class DotEngineContract : IDotEngine
    {
        #region IDotEngine Members

        string IDotEngine.Run(GraphvizImageType imageType, string dot, string outputFileName)
        {
            Contract.Requires(!String.IsNullOrEmpty(dot));
            Contract.Requires(!String.IsNullOrEmpty(outputFileName));
            Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<string>()));

            return null;
        }

        #endregion
    }
}