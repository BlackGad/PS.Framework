using System.Collections.ObjectModel;

namespace PS.Graph.Graphviz.Dot
{
    public sealed class GraphvizRecordCellCollection : Collection<GraphvizRecordCell>
    {
        #region Constructors

        public GraphvizRecordCellCollection()
        {
        }

        public GraphvizRecordCellCollection(GraphvizRecordCell[] items)
            : base(items)
        {
        }

        public GraphvizRecordCellCollection(GraphvizRecordCellCollection items)
            : base(items)
        {
        }

        #endregion
    }
}