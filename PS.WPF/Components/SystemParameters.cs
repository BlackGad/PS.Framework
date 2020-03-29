namespace PS.WPF.Components
{
    public static class SystemParameters
    {
        #region Static members

        public static double WindowCaptionHeightWithResizeFrame { get; }

        #endregion

        #region Constructors

        static SystemParameters()
        {
            WindowCaptionHeightWithResizeFrame = System.Windows.SystemParameters.WindowCaptionHeight +
                                                 System.Windows.SystemParameters.ResizeFrameHorizontalBorderHeight +
                                                 System.Windows.SystemParameters.FixedFrameHorizontalBorderHeight;
        }

        #endregion
    }
}