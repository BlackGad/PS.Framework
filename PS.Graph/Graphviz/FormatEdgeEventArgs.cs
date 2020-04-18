using PS.Graph.Graphviz.Dot;

namespace PS.Graph.Graphviz
{
    public sealed class FormatEdgeEventArgs<TVertex, TEdge> : EdgeEventArgs<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Constructors

        internal FormatEdgeEventArgs(TEdge e, GraphvizEdge edgeFormatter)
            : base(e)
        {
            #if CONTRACTS_BUG
            Contract.Requires(edgeFormatter != null);
            #endif
            EdgeFormatter = edgeFormatter;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Edge formatter
        /// </summary>
        public GraphvizEdge EdgeFormatter { get; }

        #endregion
    }

    public delegate void FormatEdgeAction<TVertex, TEdge>(
        object sender,
        FormatEdgeEventArgs<TVertex, TEdge> e)
        where TEdge : IEdge<TVertex>;
}