namespace PS.WPF.Markup
{
    public abstract class BaseFloatingMarkupExtension<T> : BaseBoxMarkupExtension<T>
    {
        private PresetMode _preset;

        protected BaseFloatingMarkupExtension()
        {
        }

        protected BaseFloatingMarkupExtension(T value)
            : base(value)
        {
        }

        public PresetMode Preset
        {
            get { return _preset; }
            set
            {
                Value = GetPreset(value);
                _preset = value;
            }
        }

        protected abstract T GetPreset(PresetMode value);

        #region Nested type: PresetMode

        public enum PresetMode
        {
            None,
            Default,
            Minimum,
            Maximum,
            PositiveInfinity,
            NegativeInfinity,
            NaN,
            Epsilon
        }

        #endregion
    }
}
