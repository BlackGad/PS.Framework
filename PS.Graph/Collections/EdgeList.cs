using System;
using System.Collections.Generic;

namespace PS.Graph.Collections
{
    [Serializable]
    public sealed class EdgeList<TVertex, TEdge> : List<TEdge>,
                                                   IEdgeList<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Constructors

        public EdgeList()
        {
        }

        public EdgeList(int capacity)
            : base(capacity)
        {
        }

        public EdgeList(EdgeList<TVertex, TEdge> list)
            : base(list)
        {
        }

        #endregion

        #region IEdgeList<TVertex,TEdge> Members

        IEdgeList<TVertex, TEdge> IEdgeList<TVertex, TEdge>.Clone()
        {
            return Clone();
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        #region Members

        public EdgeList<TVertex, TEdge> Clone()
        {
            return new EdgeList<TVertex, TEdge>(this);
        }

        #endregion
    }
}