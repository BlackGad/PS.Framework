namespace PS.WPF.Theming
{
    public abstract class Theme
    {
        public static ThemeCurrent Current { get; }

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

        public ThemeBrushes Brushes { get; }

        public ThemeColors Colors { get; }

        public ThemeFonts Fonts { get; }

        public ThemeFontSizes FontSizes { get; }
    }
}
