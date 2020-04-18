using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.IO;

namespace PS.Graph.Graphviz.Dot
{
    public class GraphvizEdge
    {
        #region Properties

        public string Comment { get; set; } = null;

        public GraphvizEdgeDirection Dir { get; set; } = GraphvizEdgeDirection.Forward;

        public Font Font { get; set; } = null;

        public Color FontColor { get; set; } = Color.Black;

        public GraphvizEdgeExtremity Head { get; set; } = new GraphvizEdgeExtremity(true);

        public GraphvizArrow HeadArrow { get; set; } = null;

        public string HeadPort { get; set; }

        public bool IsConstrained { get; set; } = true;

        public bool IsDecorated { get; set; } = false;

        public GraphvizEdgeLabel Label { get; set; } = new GraphvizEdgeLabel();

        public GraphvizLayer Layer { get; set; } = null;

        public int Length { get; set; } = 1;

        public int MinLength { get; set; } = 1;

        public Color StrokeColor { get; set; } = Color.Black;

        public GraphvizEdgeStyle Style { get; set; } = GraphvizEdgeStyle.Unspecified;

        public GraphvizEdgeExtremity Tail { get; set; } = new GraphvizEdgeExtremity(false);

        public GraphvizArrow TailArrow { get; set; } = null;

        public string TailPort { get; set; }

        public string ToolTip { get; set; } = null;

        public string Url { get; set; } = null;

        public double Weight { get; set; } = 1;

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
            var dic = new Hashtable();
            if (Comment != null)
            {
                dic["comment"] = Comment;
            }

            if (Dir != GraphvizEdgeDirection.Forward)
            {
                dic["dir"] = Dir.ToString().ToLower();
            }

            if (Font != null)
            {
                dic["fontname"] = Font.Name;
                dic["fontsize"] = Font.SizeInPoints;
            }

            if (FontColor != Color.Black)
            {
                dic["fontcolor"] = FontColor;
            }

            Head.AddParameters(dic);
            if (HeadArrow != null)
            {
                dic["arrowhead"] = HeadArrow.ToDot();
            }

            if (!IsConstrained)
            {
                dic["constraint"] = IsConstrained;
            }

            if (IsDecorated)
            {
                dic["decorate"] = IsDecorated;
            }

            Label.AddParameters(dic);
            if (Layer != null)
            {
                dic["layer"] = Layer.Name;
            }

            if (MinLength != 1)
            {
                dic["minlen"] = MinLength;
            }

            if (StrokeColor != Color.Black)
            {
                dic["color"] = StrokeColor;
            }

            if (Style != GraphvizEdgeStyle.Unspecified)
            {
                dic["style"] = Style.ToString().ToLower();
            }

            Tail.AddParameters(dic);
            if (TailArrow != null)
            {
                dic["arrowtail"] = TailArrow.ToDot();
            }

            if (ToolTip != null)
            {
                dic["tooltip"] = ToolTip;
            }

            if (Url != null)
            {
                dic["URL"] = Url;
            }

            if (Math.Abs(Weight - 1) > double.Epsilon)
            {
                dic["weight"] = Weight;
            }

            if (HeadPort != null)
            {
                dic["headport"] = HeadPort;
            }

            if (TailPort != null)
            {
                dic["tailport"] = TailPort;
            }

            if (Length != 1)
            {
                dic["len"] = Length;
            }

            return GenerateDot(dic);
        }

        internal string GenerateDot(Hashtable pairs)
        {
            var flag = false;
            var writer = new StringWriter();
            foreach (DictionaryEntry entry in pairs)
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

                if (entry.Value is GraphvizEdgeDirection direction)
                {
                    writer.Write("{0}={1}", entry.Key, direction.ToString().ToLower());
                    continue;
                }

                if (entry.Value is GraphvizEdgeStyle style)
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

                writer.Write(" {0}={1}", entry.Key, entry.Value.ToString().ToLower());
            }

            return writer.ToString();
        }

        #endregion
    }
}