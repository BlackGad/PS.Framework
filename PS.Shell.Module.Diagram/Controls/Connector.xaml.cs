using System;
using System.Windows;
using System.Windows.Controls;
using PS.WPF.Resources;

namespace PS.Shell.Module.Diagram.Controls
{
    public class Connector : ContentControl
    {
        #region Constructors

        static Connector()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Connector), new FrameworkPropertyMetadata(typeof(Connector)));
            ResourceHelper.SetDefaultStyle(typeof(Connector), Resource.ControlStyle);
        }

        #endregion

        #region Nested type: Resource

        public static class Resource
        {
            #region Constants

            private static readonly Uri Default =
                new Uri("/PS.Shell.Module.Diagram;component/Controls/Connector.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default Connector style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default Connector control template",
                                                           resourceDictionary: Default);

            #endregion
        }

        #endregion
    }
}