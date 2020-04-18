using System;

namespace PS.Graph
{
    [Serializable]
    public abstract class VertexEventArgs<TVertex> : EventArgs
    {
        #region Constructors

        protected VertexEventArgs(TVertex vertex)
        {
            Vertex = vertex;
        }

        #endregion

        #region Properties

        public TVertex Vertex { get; }

        #endregion
    }

    public delegate void VertexAction<in TVertex>(TVertex vertex);
}