using System;

namespace PS.Graph.Graphviz.Dot
{
    public readonly struct GraphvizColor : IEquatable<GraphvizColor>
    {
        #region Static members

        public static GraphvizColor Black
        {
            get { return new GraphvizColor(0xFF, 0, 0, 0); }
        }

        public static GraphvizColor LightYellow
        {
            get { return new GraphvizColor(0xFF, 0xFF, 0xFF, 0xE0); }
        }

        public static GraphvizColor White
        {
            get { return new GraphvizColor(0xFF, 0xFF, 0xFF, 0xFF); }
        }

        #endregion

        #region Constructors

        public GraphvizColor(
            byte a,
            byte r,
            byte g,
            byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        #endregion

        #region Properties

        public byte A { get; }
        public byte B { get; }
        public byte G { get; }
        public byte R { get; }

        #endregion

        #region Override members

        public override int GetHashCode()
        {
            return (A << 24) | (R << 16) | (G << 8) | B;
        }

        #endregion

        #region IEquatable<GraphvizColor> Members

        public bool Equals(GraphvizColor other)
        {
            return A == other.A && R == other.R && G == other.G && B == other.B;
        }

        #endregion
    }
}