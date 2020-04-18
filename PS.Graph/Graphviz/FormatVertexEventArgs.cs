using PS.Graph.Graphviz.Dot;

namespace PS.Graph.Graphviz
{
    public sealed class FormatVertexEventArgs<TVertex> : VertexEventArgs<TVertex>
    {
        #region Constructors

        internal FormatVertexEventArgs(TVertex v, GraphvizVertex vertexFormatter)
            : base(v)
        {
            VertexFormatter = vertexFormatter;
        }

        #endregion

        #region Properties

        public GraphvizVertex VertexFormatter { get; }

        #endregion
    }
}