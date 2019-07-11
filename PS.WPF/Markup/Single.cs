using System;
using System.Windows.Markup;

namespace PS.WPF.Markup
{
    [MarkupExtensionReturnType(typeof(float))]
    public class Single : BaseFloatingMarkupExtension<float>
    {
        #region Constructors

        public Single()
        {
        }

        public Single(float value)
            : base(value)
        {
        }

        #endregion

        #region Override members

        protected override float GetPreset(PresetMode preset)
        {
            {
                switch (preset)
                {
                    case PresetMode.None:
                    case PresetMode.Default:
                        return default;
                    case PresetMode.Minimum:
                        return float.MinValue;
                    case PresetMode.Maximum:
                        return float.MaxValue;
                    case PresetMode.PositiveInfinity:
                        return float.PositiveInfinity;
                    case PresetMode.NegativeInfinity:
                        return float.NegativeInfinity;
                    case PresetMode.NaN:
                        return float.NaN;
                    case PresetMode.Epsilon:
                        return float.Epsilon;
                    default:
                        throw new ArgumentOutOfRangeException("preset", preset, null);
                }
            }
        }

        #endregion
    }
}