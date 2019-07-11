using System;
using PS.Extensions;

namespace PS.WPF.Markup
{
    public abstract class BaseIntegerMarkupExtension<T> : BaseBoxMarkupExtension<T>
    {
        private PresetMode _preset;

        #region Constructors

        protected BaseIntegerMarkupExtension()
        {
        }

        protected BaseIntegerMarkupExtension(T value)
            : base(value)
        {
        }

        #endregion

        #region Properties

        public PresetMode Preset
        {
            get { return _preset; }
            set
            {
                Value = GetPreset(value);
                _preset = value;
            }
        }

        #endregion

        #region Members

        private T GetPreset(PresetMode value)
        {
            if (!typeof(T).TryGetNumericLimits(out var max, out var min)) throw new NotSupportedException();

            switch (value)
            {
                case PresetMode.None:
                case PresetMode.Default:
                    return default;
                case PresetMode.Minimum:
                    return (T)min;
                case PresetMode.Maximum:
                    return (T)max;
                default:
                    throw new ArgumentOutOfRangeException("value", value, null);
            }
        }

        #endregion

        #region Nested type: PresetMode

        public enum PresetMode
        {
            None,
            Default,
            Minimum,
            Maximum
        }

        #endregion
    }
}