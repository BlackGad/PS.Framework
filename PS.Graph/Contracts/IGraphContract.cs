using System.Diagnostics.Contracts;

namespace PS.Graph.Contracts
{
    [ContractClassFor(typeof(IGraph))]
    internal abstract class GraphContract<TVertex> : IGraph
    {
        #region IGraph<TVertex,TEdge> Members

        bool IGraph.IsDirected
        {
            get { return default; }
        }

        bool IGraph.AllowParallelEdges
        {
            get { return default; }
        }

        #endregion
    }
}