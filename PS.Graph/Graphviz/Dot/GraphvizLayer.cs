namespace PS.Graph.Graphviz.Dot
{
    public class GraphvizLayer
    {
        #region Constructors

        public GraphvizLayer(string name)
        {
            Name = name;
        }

        #endregion

        #region Properties

        public string Name { get; set; }

        #endregion
    }
}