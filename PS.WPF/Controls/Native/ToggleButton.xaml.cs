using System;
using System.Windows;
using System.Windows.Controls;
using PS.WPF.Resources;

namespace PS.WPF.Controls.Native
{
    public static class ToggleButton
    {
        #region Constants

        public static readonly Uri DictionaryLocation = new Uri("/PS.WPF;component/Controls/Native/ToggleButton.xaml", UriKind.RelativeOrAbsolute);

        public static readonly ResourceDescriptor ToggleButtonStyle = ResourceDescriptor.Create<Style>(DictionaryLocation);
        public static readonly ResourceDescriptor ToggleButtonTemplate = ResourceDescriptor.Create<ControlTemplate>(DictionaryLocation);

        #endregion
    }
}