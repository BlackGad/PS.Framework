using System.Collections.ObjectModel;
using System.IO;

namespace PS.Graph.Graphviz.Dot
{
    public class GraphvizLayerCollection : Collection<GraphvizLayer>
    {
        #region Constructors

        public GraphvizLayerCollection()
        {
        }

        public GraphvizLayerCollection(GraphvizLayer[] items)
            : base(items)
        {
        }

        public GraphvizLayerCollection(GraphvizLayerCollection items)
            : base(items)
        {
        }

        #endregion

        #region Properties

        public string Separators { get; set; } = ":";

        #endregion

        #region Members

        public string ToDot()
        {
            if (Count == 0)
            {
                return null;
            }

            using (var writer = new StringWriter())
            {
                writer.Write("layers=\"");
                var flag = false;
                foreach (var layer in this)
                {
                    if (flag)
                    {
                        writer.Write(Separators);
                    }
                    else
                    {
                        flag = true;
                    }

                    writer.Write(layer.Name);
                }

                writer.WriteLine("\";");
                writer.WriteLine("layersep=\"{0}\"", Separators);
                return writer.ToString();
            }
        }

        #endregion
    }
}