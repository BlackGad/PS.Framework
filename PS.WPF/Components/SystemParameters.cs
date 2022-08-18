namespace PS.WPF.Components
{
    public static class SystemParameters
    {
        public static double WindowCaptionHeightWithResizeFrame { get; }

        static SystemParameters()
        {
            WindowCaptionHeightWithResizeFrame = System.Windows.SystemParameters.WindowCaptionHeight +
                                                 System.Windows.SystemParameters.ResizeFrameHorizontalBorderHeight +
                                                 System.Windows.SystemParameters.FixedFrameHorizontalBorderHeight;
        }
    }
}
