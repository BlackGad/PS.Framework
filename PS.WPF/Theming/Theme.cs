namespace PS.WPF.Theming
{
    public abstract class Theme
    {
        #region Static members

        public static ThemeCurrent Current { get; }

        #endregion

        #region Constructors

        static Theme()
        {
            Current = new ThemeCurrent();
        }

        protected Theme()
        {
            Colors = new ThemeColors();
            Brushes = new ThemeBrushes(Colors);
            Fonts = new ThemeFonts();
            FontSizes = new ThemeFontSizes();
        }

        #endregion

        #region Properties

        public ThemeBrushes Brushes { get; }
        public ThemeColors Colors { get; }
        public ThemeFonts Fonts { get; }
        public ThemeFontSizes FontSizes { get; }

        #endregion
    }
}