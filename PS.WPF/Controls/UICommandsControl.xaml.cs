using System;
using System.Windows;
using System.Windows.Controls;
using PS.Extensions;
using PS.WPF.Extensions;
using PS.WPF.Resources;

namespace PS.WPF.Controls
{
    public class UICommandsControl : ItemsControl
    {
        #region Constructors

        static UICommandsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UICommandsControl), new FrameworkPropertyMetadata(typeof(UICommandsControl)));
            ResourceHelper.SetDefaultStyle(typeof(UICommandsControl), Resource.ControlStyle);
        }

        public UICommandsControl()
        {
            Loaded += (sender, args) => Dispatcher.Postpone(() =>
            {
                var commands = ItemsSource.Enumerate<IUICommand>();
                commands.ForEach(command => command?.RaiseCanExecuteChanged());
            });
        }

        #endregion

        #region Nested type: Resource

        public static class Resource
        {
            #region Constants

            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/UICommandsControl.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor CommandButtonTemplate =
                ResourceDescriptor.Create<System.Windows.DataTemplate>(description: "Default Command button data template",
                                                                       resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default UICommandsControl style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default UICommandsControl control template",
                                                           resourceDictionary: Default);

            #endregion
        }

        #endregion
    }
}