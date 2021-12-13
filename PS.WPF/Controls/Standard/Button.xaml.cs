using System;
using System.Windows;
using System.Windows.Controls;
using PS.WPF.Resources;

namespace PS.WPF.Controls.Standard
{
    public static class Button
    {
        #region Constants

        public static readonly Uri DictionaryLocation = new Uri("/PS.WPF;component/Controls/Standard/Button.xaml", UriKind.RelativeOrAbsolute);
        public static readonly ResourceDescriptor ButtonStyle = ResourceDescriptor.Create<Style>(DictionaryLocation);
        public static readonly ResourceDescriptor ButtonTemplate = ResourceDescriptor.Create<ControlTemplate>(DictionaryLocation);

        #endregion
    }
}