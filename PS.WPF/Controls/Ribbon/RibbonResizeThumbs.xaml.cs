using System;
using System.Windows;
using PS.WPF.Resources;

namespace PS.WPF.Controls.Ribbon
{
    public static class RibbonResizeThumbs
    {
        #region Constants

        private static readonly Uri Default =
            new Uri("/PS.WPF;component/Controls/Ribbon/RibbonResizeThumbs.xaml", UriKind.RelativeOrAbsolute);

        public static readonly ResourceDescriptor ThumbBothLeftBottomStyle =
            ResourceDescriptor.Create<Style>(description: "Default Thumb style",
                                             resourceDictionary: Default);

        public static readonly ResourceDescriptor ThumbBothRightBottomStyle =
            ResourceDescriptor.Create<Style>(description: "Default Thumb style",
                                             resourceDictionary: Default);

        public static readonly ResourceDescriptor ThumbBothRightTopStyle =
            ResourceDescriptor.Create<Style>(description: "Default Thumb style",
                                             resourceDictionary: Default);

        public static readonly ResourceDescriptor ThumbVerticalStyle =
            ResourceDescriptor.Create<Style>(description: "Default Thumb style",
                                             resourceDictionary: Default);

        #endregion
    }
}