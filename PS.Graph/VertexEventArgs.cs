using System;
using System.Diagnostics.Contracts;

namespace PS.Graph
{
    [Serializable]
    public abstract class VertexEventArgs<TVertex> : EventArgs
    {
        #region Constructors

        protected VertexEventArgs(TVertex vertex)
        {
            Contract.Requires(vertex != null);
            Vertex = vertex;
        }

        #endregion

        #region Properties

        public TVertex Vertex { get; }

        #endregion
    }

    public delegate void VertexAction<in TVertex>(TVertex vertex);
}