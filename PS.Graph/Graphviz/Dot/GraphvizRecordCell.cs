using System.Text;

namespace PS.Graph.Graphviz.Dot
{
    public class GraphvizRecordCell
    {
        #region Properties

        public GraphvizRecordCellCollection Cells { get; } = new GraphvizRecordCellCollection();

        public bool HasPort
        {
            get
            {
                if (Port != null)
                {
                    return Port.Length > 0;
                }

                return false;
            }
        }

        public bool HasText
        {
            get
            {
                if (Text != null)
                {
                    return Text.Length > 0;
                }

                return false;
            }
        }

        public string Port { get; set; } = null;

        public string Text { get; set; } = null;

        protected GraphvizRecordEscaper Escaper { get; } = new GraphvizRecordEscaper();

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
            var builder = new StringBuilder();
            if (HasPort)
            {
                builder.AppendFormat("<{0}> ", Escaper.Escape(Port));
            }

            if (HasText)
            {
                builder.AppendFormat("{0}", Escaper.Escape(Text));
            }

            if (Cells.Count > 0)
            {
                builder.Append(" { ");
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

                builder.Append(" } ");
            }

            return builder.ToString();
        }

        #endregion
    }
}