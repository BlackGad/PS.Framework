using System.Collections;
using System.Diagnostics.Contracts;

namespace PS.Graph.Graphviz.Dot
{
    public class GraphvizEdgeExtremity
    {
        #region Constructors

        public GraphvizEdgeExtremity(bool isHead)
        {
            IsHead = isHead;
            Url = null;
            IsClipped = true;
            Label = null;
            ToolTip = null;
            Logical = null;
            Same = null;
        }

        #endregion

        #region Properties

        public bool IsClipped { get; set; }

        public bool IsHead { get; }

        public string Label { get; set; }

        public string Logical { get; set; }

        public string Same { get; set; }

        public string ToolTip { get; set; }

        public string Url { get; set; }

        #endregion

        #region Members

        public void AddParameters(IDictionary dic)
        {
            Contract.Requires(dic != null);

            string text;
            if (IsHead)
            {
                text = "head";
            }
            else
            {
                text = "tail";
            }

            if (Url != null)
            {
                dic.Add(text + "URL", Url);
            }

            if (!IsClipped)
            {
                dic.Add(text + "clip", IsClipped);
            }

            if (Label != null)
            {
                dic.Add(text + "label", Label);
            }

            if (ToolTip != null)
            {
                dic.Add(text + "tooltip", ToolTip);
            }

            if (Logical != null)
            {
                dic.Add("l" + text, Logical);
            }

            if (Same != null)
            {
                dic.Add("same" + text, Same);
            }
        }

        #endregion
    }
}