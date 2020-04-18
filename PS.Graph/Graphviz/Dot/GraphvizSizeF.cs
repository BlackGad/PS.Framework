using System;
using System.Diagnostics;
using System.Globalization;

namespace PS.Graph.Graphviz.Dot
{
    [DebuggerDisplay("{Width}x{Height}")]
    public readonly struct GraphvizSizeF
    {
        #region Constructors

        public GraphvizSizeF(float width, float height)
        {
            Width = width;
            Height = height;
        }

        #endregion

        #region Properties

        public float Height { get; }

        public bool IsEmpty
        {
            get { return Math.Abs(Width) < float.Epsilon || Math.Abs(Height) < float.Epsilon; }
        }

        public float Width { get; }

        #endregion

        #region Override members

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}x{1}", Width, Height);
        }

        #endregion
    }
}