namespace PS.Graph.Graphviz.Dot
{
    public class Font
    {
        #region Constructors

        public Font(string name, float sizeInPoints)
        {
            Name = name;
            SizeInPoints = sizeInPoints;
        }

        #endregion

        #region Properties

        public string Name { get; set; }
        public float SizeInPoints { get; set; }

        #endregion
    }
}