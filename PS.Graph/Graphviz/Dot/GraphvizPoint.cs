namespace PS.Graph.Graphviz.Dot
{
    public sealed class GraphvizPoint
    {
        #region Constructors

        public GraphvizPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        #endregion

        #region Properties

        public int X { get; }
        public int Y { get; }

        #endregion
    }
}