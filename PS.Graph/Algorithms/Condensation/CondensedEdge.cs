using System;
using System.Collections.Generic;

namespace PS.Graph.Algorithms.Condensation
{
    [Serializable]
    public sealed class CondensedEdge<TVertex, TEdge, TGraph> : Edge<TGraph>
        where TEdge : IEdge<TVertex>
        where TGraph : IMutableVertexAndEdgeSet<TVertex, TEdge>, new()
    {
        private List<TEdge> _edges = new List<TEdge>();

        #region Constructors

        public CondensedEdge(TGraph source, TGraph target)
            : base(source, target)
        {
        }

        #endregion

        #region Properties

        public IList<TEdge> Edges
        {
            get { return _edges; }
        }

        #endregion
    }
}