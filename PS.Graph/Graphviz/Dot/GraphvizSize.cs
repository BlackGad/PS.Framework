using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace PS.Graph.Graphviz.Dot
{
    [DebuggerDisplay("{Width}x{Height}")]
    public readonly struct GraphvizSizeF
    {
        #region Constructors

        public GraphvizSizeF(float width, float height)
        {
            Contract.Requires(width >= 0);
            Contract.Requires(height >= 0);

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

    [DebuggerDisplay("{Width}x{Height}")]
    public readonly struct GraphvizSize
    {
        #region Constructors

        public GraphvizSize(int width, int height)
        {
            Contract.Requires(width >= 0);
            Contract.Requires(height >= 0);

            Width = width;
            Height = height;
        }

        #endregion

        #region Properties

        public int Height { get; }

        public bool IsEmpty
        {
            get { return Width == 0 || Height == 0; }
        }

        public int Width { get; }

        #endregion

        #region Override members

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}x{1}", Width, Height);
        }

        #endregion
    }
}