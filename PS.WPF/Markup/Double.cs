using System;
using System.Windows.Markup;

namespace PS.WPF.Markup
{
    [MarkupExtensionReturnType(typeof(double))]
    public class Double : BaseFloatingMarkupExtension<double>
    {
        public Double()
        {
        }

        public Double(double value)
            : base(value)
        {
        }

        protected override double GetPreset(PresetMode preset)
        {
            {
                switch (preset)
                {
                    case PresetMode.None:
                    case PresetMode.Default:
                        return default;
                    case PresetMode.Minimum:
                        return double.MinValue;
                    case PresetMode.Maximum:
                        return double.MaxValue;
                    case PresetMode.PositiveInfinity:
                        return double.PositiveInfinity;
                    case PresetMode.NegativeInfinity:
                        return double.NegativeInfinity;
                    case PresetMode.NaN:
                        return double.NaN;
                    case PresetMode.Epsilon:
                        return double.Epsilon;
                    default:
                        throw new ArgumentOutOfRangeException("preset", preset, null);
                }
            }
        }
    }
}
