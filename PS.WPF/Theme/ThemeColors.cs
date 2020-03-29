using System.Windows.Media;

namespace PS.WPF.Theme
{
    public static class ThemeColors
    {
        #region Static members

        public static Color Accent
        {
            get { return _accent; }
            set
            {
                _accent = value;
                Accent75 = FromColor(value, 0.75);
                Accent50 = FromColor(value, 0.50);
                Accent25 = FromColor(value, 0.25);
            }
        }

        public static Color Accent25 { get; private set; }
        public static Color Accent50 { get; private set; }
        public static Color Accent75 { get; private set; }

        public static Color Basic
        {
            get { return _basic; }
            set
            {
                _basic = value;
                Basic75 = FromColor(value, 0.75);
                Basic50 = FromColor(value, 0.50);
                Basic25 = FromColor(value, 0.25);
            }
        }

        public static Color Basic25 { get; private set; }
        public static Color Basic50 { get; private set; }
        public static Color Basic75 { get; private set; }

        public static Color Error { get; set; }

        public static Color Highlight
        {
            get { return _highlight; }
            set
            {
                _highlight = value;
                Highlight75 = FromColor(value, 0.75);
                Highlight50 = FromColor(value, 0.50);
                Highlight25 = FromColor(value, 0.25);
            }
        }

        public static Color Highlight25 { get; private set; }
        public static Color Highlight50 { get; private set; }
        public static Color Highlight75 { get; private set; }

        public static Color Main
        {
            get { return _main; }
            set
            {
                _main = value;
                Main75 = FromColor(value, 0.75);
                Main50 = FromColor(value, 0.50);
                Main25 = FromColor(value, 0.25);
            }
        }

        public static Color Main25 { get; private set; }
        public static Color Main50 { get; private set; }
        public static Color Main75 { get; private set; }

        public static Color Marker { get; set; }

        public static Color Strong { get; set; }

        public static Color Success { get; set; }

        public static Color Warning
        {
            get { return _warning; }
            set
            {
                _warning = value;
                Warning75 = FromColor(value, 0.75);
                Warning50 = FromColor(value, 0.50);
                Warning25 = FromColor(value, 0.25);
            }
        }

        public static Color Warning25 { get; private set; }
        public static Color Warning50 { get; private set; }
        public static Color Warning75 { get; private set; }

        private static Color FromColor(Color color, double opacity)
        {
            if (opacity > 1) opacity = 1;
            if (opacity < 0) opacity = 0;
            var a = (byte)((int)(255 * opacity) & 0xFF);
            return Color.FromArgb(a, color.R, color.G, color.B);
        }

        #endregion

        #region Constructors

        static ThemeColors()
        {
            Accent = Colors.RoyalBlue;
            Main = Colors.White;
            Basic = Colors.DarkGray;
            Strong = Colors.Gray;
            Marker = Colors.Black;
            Error = Colors.Red;
            Success = Colors.Green;
            Warning = Colors.Orange;
            Highlight = Colors.RoyalBlue;
        }

        #endregion

        private static Color _accent;
        private static Color _basic;
        private static Color _highlight;
        private static Color _main;
        private static Color _warning;
    }
}