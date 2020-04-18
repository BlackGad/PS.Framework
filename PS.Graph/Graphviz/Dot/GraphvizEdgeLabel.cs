using System;
using System.Collections;
using System.Drawing;

namespace PS.Graph.Graphviz.Dot
{
    public class GraphvizEdgeLabel
    {
        #region Properties

        public double Angle { get; set; } = -25;

        public double Distance { get; set; } = 1;

        public bool Float { get; set; } = true;

        public Font Font { get; set; } = null;

        public Color FontColor { get; set; } = Color.Black;

        public string Value { get; set; } = null;

        #endregion

        #region Members

        public void AddParameters(IDictionary dic)
        {
            if (Value == null) return;

            dic["label"] = Value;
            if (Math.Abs(Angle - (-25)) > double.Epsilon)
            {
                dic["labelangle"] = Angle;
            }

            if (Math.Abs(Distance - 1) > double.Epsilon)
            {
                dic["labeldistance"] = Distance;
            }

            if (!Float)
            {
                dic["labelfloat"] = Float;
            }

            if (Font != null)
            {
                dic["labelfontname"] = Font.Name;
                dic["labelfontsize"] = Font.SizeInPoints;
            }
        }

        #endregion
    }
}