﻿using System.Diagnostics;
using System.Globalization;

namespace PS.Graph.Graphviz.Dot
{
    [DebuggerDisplay("{Width}x{Height}")]
    public readonly struct GraphvizSize
    {
        #region Constructors

        public GraphvizSize(int width, int height)
        {
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