using System;
using System.Diagnostics.Contracts;

namespace PS.Graph.Contracts
{
    [ContractClassFor(typeof(IMutableGraph))]
    internal abstract class MutableGraphContract : IMutableGraph
    {
        #region IMutableGraph<TVertex,TEdge> Members

        void IMutableGraph.Clear()
        {
            IMutableGraph iThis = this;
        }

        bool IGraph.IsDirected
        {
            get { throw new NotImplementedException(); }
        }

        bool IGraph.AllowParallelEdges
        {
            get { throw new NotImplementedException(); }
        }

        event EventHandler IMutableGraph.Cleared
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        #endregion
    }
}