using System;
using System.Collections.Generic;

namespace PS.Graph.Collections
{
    [Serializable]
    public sealed class VertexList<TVertex> : List<TVertex>,
                                              ICloneable

    {
        #region Constructors

        public VertexList()
        {
        }

        public VertexList(int capacity)
            : base(capacity)
        {
        }

        public VertexList(VertexList<TVertex> other)
            : base(other)
        {
        }

        #endregion

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        #region Members

        public VertexList<TVertex> Clone()
        {
            return new VertexList<TVertex>(this);
        }

        #endregion
    }
}