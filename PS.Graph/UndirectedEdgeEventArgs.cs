using System;

namespace PS.Graph
{
    [Serializable]
    public class UndirectedEdgeEventArgs<TVertex, TEdge> : EdgeEventArgs<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Constructors

        public UndirectedEdgeEventArgs(TEdge edge, bool reversed)
            : base(edge)
        {
            Reversed = reversed;
        }

        #endregion

        #region Properties

        public bool Reversed { get; }

        public TVertex Source
        {
            get { return Reversed ? Edge.Target : Edge.Source; }
        }

        public TVertex Target
        {
            get { return Reversed ? Edge.Source : Edge.Target; }
        }

        #endregion
    }

    public delegate void UndirectedEdgeAction<TVertex, TEdge>(
        Object sender,
        UndirectedEdgeEventArgs<TVertex, TEdge> e)
        where TEdge : IEdge<TVertex>;
}