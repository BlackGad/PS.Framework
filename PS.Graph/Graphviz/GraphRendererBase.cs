using PS.Graph.Graphviz.Dot;

namespace PS.Graph.Graphviz
{
    public abstract class GraphRendererBase<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Constructors

        protected GraphRendererBase(IEdgeListGraph<TVertex, TEdge> visitedGraph)
        {
            Graphviz = new GraphvizAlgorithm<TVertex, TEdge>(visitedGraph);
            Initialize();
        }

        #endregion

        #region Properties

        public GraphvizAlgorithm<TVertex, TEdge> Graphviz { get; }

        public IEdgeListGraph<TVertex, TEdge> VisitedGraph
        {
            get { return Graphviz.VisitedGraph; }
        }

        #endregion

        #region Members

        public string Generate(IDotEngine dot, string fileName)
        {
            return Graphviz.Generate(dot, fileName);
        }

        protected virtual void Initialize()
        {
            Graphviz.CommonVertexFormat.Style = GraphvizVertexStyle.Filled;
            Graphviz.CommonVertexFormat.FillColor = System.Drawing.Color.LightYellow;
            Graphviz.CommonVertexFormat.Font = new Font("Tahoma", 8.25F);
            Graphviz.CommonVertexFormat.Shape = GraphvizVertexShape.Box;

            Graphviz.CommonEdgeFormat.Font = new Font("Tahoma", 8.25F);
        }

        #endregion
    }
}