using System.Windows.Media;

namespace PS.WPF.Theme
{
    public static class ThemeFonts
    {
        #region Static members

        public static FontFamily FontFamily { get; set; }
        public static FontFamily FontFamilyExtraStrong { get; set; }
        public static FontFamily FontFamilyLight { get; set; }
        public static FontFamily FontFamilyStrong { get; set; }
        public static double FontSize { get; set; }
        public static double FontSizeL { get; set; }
        public static double FontSizeS { get; set; }
        public static double FontSizeXL { get; set; }
        public static double FontSizeXS { get; set; }
        public static double FontSizeXXL { get; set; }
        public static double FontSizeXXXL { get; set; }
        public static double FontSizeXXXXL { get; set; }

        #endregion

        #region Constructors

        static ThemeFonts()
        {
            FontSizeXS = 10;
            FontSizeS = 11;
            FontSize = 12;
            FontSizeL = 14;
            FontSizeXL = 16;
            FontSizeXXL = 20;
            FontSizeXXXL = 24;
            FontSizeXXXXL = 36;
            FontFamily = new FontFamily("Segoe UI");
            FontFamilyLight = new FontFamily("Segoe UI Light");
            FontFamilyStrong = new FontFamily("Segoe UI Semibold");
            FontFamilyExtraStrong = new FontFamily("Segoe UI Black");
        }

        #endregion
    }
}