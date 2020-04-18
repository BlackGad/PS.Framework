namespace PS.Graph.Graphviz.Dot
{
    public sealed class GraphvizFont
    {
        #region Constructors

        public GraphvizFont(string name, float sizeInPoints)
        {
            Name = name;
            SizeInPoints = sizeInPoints;
        }

        #endregion

        #region Properties

        public string Name { get; }
        public float SizeInPoints { get; }

        #endregion
    }
}