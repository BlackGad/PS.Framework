using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;

namespace PS.Graph.Graphviz.Dot
{
    public class GraphvizVertex
    {
        #region Properties

        public string BottomLabel { get; set; } = null;

        public string Comment { get; set; } = null;

        public double Distorsion { get; set; } = 0;

        public Color FillColor { get; set; } = Color.White;

        public bool FixedSize { get; set; } = false;

        public Font Font { get; set; } = null;

        public Color FontColor { get; set; } = Color.Black;

        public string Group { get; set; } = null;

        public string Label { get; set; } = null;

        public GraphvizLayer Layer { get; set; } = null;

        public double Orientation { get; set; } = 0;

        public int Peripheries { get; set; } = -1;

        public Point? Position { get; set; }

        public GraphvizRecord Record { get; set; } = new GraphvizRecord();

        public bool Regular { get; set; } = false;

        public GraphvizVertexShape Shape { get; set; } = GraphvizVertexShape.Unspecified;

        public int Sides { get; set; } = 4;

        public SizeF Size { get; set; } = new SizeF(0f, 0f);

        public double Skew { get; set; } = 0;

        public Color StrokeColor { get; set; } = Color.Black;

        public GraphvizVertexStyle Style { get; set; } = GraphvizVertexStyle.Unspecified;

        public string ToolTip { get; set; } = null;

        public string TopLabel { get; set; } = null;

        public string Url { get; set; } = null;

        public double Z { get; set; } = -1;

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
            var pairs = new Dictionary<string, object>();
            if (Font != null)
            {
                pairs["fontname"] = Font.Name;
                pairs["fontsize"] = Font.SizeInPoints;
            }

            if (FontColor != Color.Black)
            {
                pairs["fontcolor"] = FontColor;
            }

            if (Shape != GraphvizVertexShape.Unspecified)
            {
                pairs["shape"] = Shape;
            }

            if (Style != GraphvizVertexStyle.Unspecified)
            {
                pairs["style"] = Style;
            }

            if (Shape == GraphvizVertexShape.Record)
            {
                pairs["label"] = Record;
            }
            else if (Label != null)
            {
                pairs["label"] = Label;
            }

            if (FixedSize)
            {
                pairs["fixedsize"] = true;
                if (Size.Height > 0f)
                {
                    pairs["height"] = Size.Height;
                }

                if (Size.Width > 0f)
                {
                    pairs["width"] = Size.Width;
                }
            }

            if (StrokeColor != Color.Black)
            {
                pairs["color"] = StrokeColor;
            }

            if (FillColor != Color.White)
            {
                pairs["fillcolor"] = FillColor;
            }

            if (Regular)
            {
                pairs["regular"] = Regular;
            }

            if (Url != null)
            {
                pairs["URL"] = Url;
            }

            if (ToolTip != null)
            {
                pairs["tooltip"] = ToolTip;
            }

            if (Comment != null)
            {
                pairs["comment"] = Comment;
            }

            if (Group != null)
            {
                pairs["group"] = Group;
            }

            if (Layer != null)
            {
                pairs["layer"] = Layer.Name;
            }

            if (Orientation > 0)
            {
                pairs["orientation"] = Orientation;
            }

            if (Peripheries >= 0)
            {
                pairs["peripheries"] = Peripheries;
            }

            if (Z > 0)
            {
                pairs["z"] = Z;
            }

            if (Position.HasValue)
            {
                var p = Position.Value;
                pairs["pos"] = $"{p.X},{p.Y}!";
            }

            if (Style == GraphvizVertexStyle.Diagonals || Shape == GraphvizVertexShape.MCircle || Shape == GraphvizVertexShape.MDiamond ||
                Shape == GraphvizVertexShape.MSquare)
            {
                if (TopLabel != null)
                {
                    pairs["toplabel"] = TopLabel;
                }

                if (BottomLabel != null)
                {
                    pairs["bottomlable"] = BottomLabel;
                }
            }

            if (Shape == GraphvizVertexShape.Polygon)
            {
                if (Sides != 0)
                {
                    pairs["sides"] = Sides;
                }

                if (Math.Abs(Skew) > double.Epsilon)
                {
                    pairs["skew"] = Skew;
                }

                if (Math.Abs(Distorsion) > double.Epsilon)
                {
                    pairs["distorsion"] = Distorsion;
                }
            }

            return GenerateDot(pairs);
        }

        internal string GenerateDot(Dictionary<string, object> pairs)
        {
            var flag = false;
            var writer = new StringWriter();
            foreach (var entry in pairs)
            {
                if (flag)
                {
                    writer.Write(", ");
                }
                else
                {
                    flag = true;
                }

                if (entry.Value is string)
                {
                    writer.Write("{0}=\"{1}\"", entry.Key, entry.Value);
                    continue;
                }

                if (entry.Value is float floatValue)
                {
                    writer.Write("{0}={1}", entry.Key, floatValue.ToString(CultureInfo.InvariantCulture));
                    continue;
                }

                if (entry.Value is double doubleValue)
                {
                    writer.Write("{0}={1}", entry.Key, doubleValue.ToString(CultureInfo.InvariantCulture));
                    continue;
                }

                if (entry.Value is GraphvizVertexShape shape)
                {
                    writer.Write("{0}={1}", entry.Key, shape.ToString().ToLower());
                    continue;
                }

                if (entry.Value is GraphvizVertexStyle style)
                {
                    writer.Write("{0}={1}", entry.Key, style.ToString().ToLower());
                    continue;
                }

                if (entry.Value is Color color)
                {
                    writer.Write("{0}=\"#{1}{2}{3}{4}\"",
                                 entry.Key,
                                 color.R.ToString("x2").ToUpper(),
                                 color.G.ToString("x2").ToUpper(),
                                 color.B.ToString("x2").ToUpper(),
                                 color.A.ToString("x2").ToUpper());
                    continue;
                }

                if (entry.Value is GraphvizRecord record)
                {
                    writer.WriteLine("{0}=\"{1}\"", entry.Key, record.ToDot());
                    continue;
                }

                writer.Write(" {0}={1}", entry.Key, entry.Value.ToString().ToLower());
            }

            return writer.ToString();
        }

        #endregion
    }
}