using System.IO;

namespace PS.Graph.Graphviz.Dot
{
    public class GraphvizArrow
    {
        #region Constructors

        public GraphvizArrow(GraphvizArrowShape shape)
        {
            Shape = shape;
            Clipping = GraphvizArrowClipping.None;
            Filling = GraphvizArrowFilling.Close;
        }

        public GraphvizArrow(GraphvizArrowShape shape, GraphvizArrowClipping clip, GraphvizArrowFilling fill)
        {
            Shape = shape;
            Clipping = clip;
            Filling = fill;
        }

        #endregion

        #region Properties

        public GraphvizArrowClipping Clipping { get; set; }

        public GraphvizArrowFilling Filling { get; set; }

        public GraphvizArrowShape Shape { get; set; }

        #endregion

        #region Override members

        public override string ToString()
        {
            return ToDot();
        }

        #endregion

        #region Members

        public string ToDot()
        {
            using (var writer = new StringWriter())
            {
                if (Filling == GraphvizArrowFilling.Open)
                {
                    writer.Write('o');
                }

                switch (Clipping)
                {
                    case GraphvizArrowClipping.Left:
                        writer.Write('l');
                        break;

                    case GraphvizArrowClipping.Right:
                        writer.Write('r');
                        break;
                }

                writer.Write(Shape.ToString().ToLower());
                return writer.ToString();
            }
        }

        #endregion
    }
}