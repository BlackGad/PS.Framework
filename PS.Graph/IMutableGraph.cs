using System;
using System.Diagnostics.Contracts;
using PS.Graph.Contracts;

namespace PS.Graph
{
    /// <summary>
    ///     A mutable graph instance
    /// </summary>
    [ContractClass(typeof(MutableGraphContract))]
    public interface IMutableGraph : IGraph
    {
        #region Events

        /// <summary>
        ///     Called when the graph vertices and edges have been cleared.
        /// </summary>
        event EventHandler Cleared;

        #endregion

        #region Members

        /// <summary>
        ///     Clears the vertex and edges
        /// </summary>
        void Clear();

        #endregion
    }
}