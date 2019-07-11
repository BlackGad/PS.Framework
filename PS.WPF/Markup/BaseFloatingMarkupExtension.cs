namespace PS.WPF.Markup
{
    public abstract class BaseFloatingMarkupExtension<T> : BaseBoxMarkupExtension<T>
    {
        private PresetMode _preset;

        #region Constructors

        protected BaseFloatingMarkupExtension()
        {
        }

        protected BaseFloatingMarkupExtension(T value)
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

        protected abstract T GetPreset(PresetMode value);

        #endregion

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