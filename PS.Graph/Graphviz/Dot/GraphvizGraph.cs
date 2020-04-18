using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace PS.Graph.Graphviz.Dot
{
    public class GraphvizGraph
    {
        #region Properties

        public Color BackgroundColor { get; set; } = Color.White;

        public GraphvizClusterMode ClusterRank { get; set; } = GraphvizClusterMode.Local;

        public string Comment { get; set; } = null;

        public Font Font { get; set; } = null;

        public Color FontColor { get; set; } = Color.Black;

        public bool IsCentered { get; set; } = false;

        public bool IsCompounded { get; set; } = false;

        public bool IsConcentrated { get; set; } = false;

        public bool IsLandscape { get; set; } = false;

        public bool IsNormalized { get; set; } = false;

        public bool IsReMinCross { get; set; } = false;

        public string Label { get; set; } = null;

        public GraphvizLabelJustification LabelJustification { get; set; } = GraphvizLabelJustification.C;

        public GraphvizLabelLocation LabelLocation { get; set; } = GraphvizLabelLocation.B;

        public GraphvizLayerCollection Layers { get; } = new GraphvizLayerCollection();

        public double McLimit { get; set; } = 1;

        public string Name { get; set; } = "G";

        public double NodeSeparation { get; set; } = 0.25;

        public int NsLimit { get; set; } = -1;

        public int NsLimit1 { get; set; } = -1;

        public GraphvizOutputMode OutputOrder { get; set; } = GraphvizOutputMode.BreathFirst;

        public GraphvizPageDirection PageDirection { get; set; } = GraphvizPageDirection.Bl;

        public SizeF PageSize { get; set; } = new SizeF(0, 0);

        public double Quantum { get; set; } = 0;

        public GraphvizRankDirection RankDirection { get; set; } = GraphvizRankDirection.Tb;

        public double RankSeparation { get; set; } = 0.5;

        public GraphvizRatioMode Ratio { get; set; } = GraphvizRatioMode.Auto;

        public double Resolution { get; set; } = 0.96;

        public int Rotate { get; set; } = 0;

        public int SamplePoints { get; set; } = 8;

        public int SearchSize { get; set; } = 30;

        public SizeF Size { get; set; } = new SizeF(0, 0);

        public string StyleSheet { get; set; } = null;

        public string Url { get; set; } = null;

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
            var pairs = new Hashtable();
            if (Url != null)
            {
                pairs["URL"] = Url;
            }

            if (BackgroundColor != Color.White)
            {
                pairs["bgcolor"] = BackgroundColor;
            }

            if (IsCentered)
            {
                pairs["center"] = true;
            }

            if (ClusterRank != GraphvizClusterMode.Local)
            {
                pairs["clusterrank"] = ClusterRank.ToString().ToLower();
            }

            if (Comment != null)
            {
                pairs["comment"] = Comment;
            }

            if (IsCompounded)
            {
                pairs["compound"] = IsCompounded;
            }

            if (IsConcentrated)
            {
                pairs["concentrated"] = IsConcentrated;
            }

            if (Font != null)
            {
                pairs["fontname"] = Font.Name;
                pairs["fontsize"] = Font.SizeInPoints;
            }

            if (FontColor != Color.Black)
            {
                pairs["fontcolor"] = FontColor;
            }

            if (Label != null)
            {
                pairs["label"] = Label;
            }

            if (LabelJustification != GraphvizLabelJustification.C)
            {
                pairs["labeljust"] = LabelJustification.ToString().ToLower();
            }

            if (LabelLocation != GraphvizLabelLocation.B)
            {
                pairs["labelloc"] = LabelLocation.ToString().ToLower();
            }

            if (Layers.Count != 0)
            {
                pairs["layers"] = Layers.ToDot();
            }

            if (Math.Abs(McLimit - 1) > double.Epsilon)
            {
                pairs["mclimit"] = McLimit;
            }

            if (Math.Abs(NodeSeparation - 0.25) > double.Epsilon)
            {
                pairs["nodesep"] = NodeSeparation;
            }

            if (IsNormalized)
            {
                pairs["normalize"] = IsNormalized;
            }

            if (NsLimit > 0)
            {
                pairs["nslimit"] = NsLimit;
            }

            if (NsLimit1 > 0)
            {
                pairs["nslimit1"] = NsLimit1;
            }

            if (OutputOrder != GraphvizOutputMode.BreathFirst)
            {
                pairs["outputorder"] = OutputOrder.ToString().ToLower();
            }

            if (!PageSize.IsEmpty)
            {
                pairs["page"] = $"{PageSize.Width},{PageSize.Height}";
            }

            if (PageDirection != GraphvizPageDirection.Bl)
            {
                pairs["pagedir"] = PageDirection.ToString().ToLower();
            }

            if (Quantum > 0)
            {
                pairs["quantum"] = Quantum;
            }

            if (Math.Abs(RankSeparation - 0.5) > double.Epsilon)
            {
                pairs["ranksep"] = RankSeparation;
            }

            if (Ratio != GraphvizRatioMode.Auto)
            {
                pairs["ratio"] = Ratio.ToString().ToLower();
            }

            if (IsReMinCross)
            {
                pairs["remincross"] = IsReMinCross;
            }

            if (Math.Abs(Resolution - 0.96) > double.Epsilon)
            {
                pairs["resolution"] = Resolution;
            }

            if (Rotate != 0)
            {
                pairs["rotate"] = Rotate;
            }
            else if (IsLandscape)
            {
                pairs["orientation"] = "[1L]*";
            }

            if (SamplePoints != 8)
            {
                pairs["samplepoints"] = SamplePoints;
            }

            if (SearchSize != 30)
            {
                pairs["searchsize"] = SearchSize;
            }

            if (!Size.IsEmpty)
            {
                pairs["size"] = $"{Size.Width},{Size.Height}";
            }

            if (StyleSheet != null)
            {
                pairs["stylesheet"] = StyleSheet;
            }

            if (RankDirection != GraphvizRankDirection.Tb)
            {
                pairs["rankdir"] = RankDirection;
            }

            return GenerateDot(pairs);
        }

        internal string GenerateDot(Hashtable pairs)
        {
            var entries = new List<string>(pairs.Count);
            foreach (DictionaryEntry entry in pairs)
            {
                if (entry.Value is string)
                {
                    entries.Add($"{entry.Key}=\"{entry.Value}\"");
                    continue;
                }

                if (entry.Value is float floatValue)
                {
                    entries.Add($"{entry.Key}={floatValue.ToString(CultureInfo.InvariantCulture)}");
                    continue;
                }

                if (entry.Value is double doubleValue)
                {
                    entries.Add($"{entry.Key}={doubleValue.ToString(CultureInfo.InvariantCulture)}");
                    continue;
                }

                if (entry.Value is Color color)
                {
                    entries.Add($"{entry.Key}=\"#{color.R.ToString("x2").ToUpper()}" +
                                $"{color.G.ToString("x2").ToUpper()}" +
                                $"{color.B.ToString("x2").ToUpper()}" +
                                $"{color.A.ToString("x2").ToUpper()}\"");
                    continue;
                }

                if (entry.Value is GraphvizRankDirection || entry.Value is GraphvizPageDirection)
                {
                    entries.Add($"{entry.Key}={entry.Value}");
                    continue;
                }

                entries.Add($" {entry.Key}={entry.Value.ToString().ToLower()}");
            }

            var result = String.Join(";", entries);
            result = entries.Count > 1 ? result + ";" : result;

            return result;
        }

        #endregion
    }
}