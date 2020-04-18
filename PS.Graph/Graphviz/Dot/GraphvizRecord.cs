using System.Text;

namespace PS.Graph.Graphviz.Dot
{
    public class GraphvizRecord
    {
        #region Properties

        public GraphvizRecordCellCollection Cells { get; } = new GraphvizRecordCellCollection();

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
            if (Cells.Count == 0)
            {
                return "";
            }

            var builder = new StringBuilder();
            var flag = false;
            foreach (var cell in Cells)
            {
                if (flag)
                {
                    builder.AppendFormat(" | {0}", cell.ToDot());
                    continue;
                }

                builder.Append(cell.ToDot());
                flag = true;
            }

            return builder.ToString();
        }

        #endregion
    }
}