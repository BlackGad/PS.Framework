using System;
using System.Windows;
using System.Windows.Controls;
using PS.Extensions;
using PS.WPF.Extensions;
using PS.WPF.Patterns.Command;
using PS.WPF.Resources;

namespace PS.WPF.Controls
{
    public class UICommandsControl : ItemsControl
    {
        static UICommandsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UICommandsControl), new FrameworkPropertyMetadata(typeof(UICommandsControl)));
            ResourceHelper.SetDefaultStyle(typeof(UICommandsControl), Resource.ControlStyle);
        }

        public UICommandsControl()
        {
            Loaded += (sender, args) => Dispatcher.Postpone(() =>
            {
                var commands = Items.Enumerate<IUICommand>();
                commands.ForEach(command => command?.RaiseCanExecuteChanged());
            });
        }

        #region Nested type: Resource

        public static class Resource
        {
            private static readonly Uri Default = new Uri("/PS.WPF;component/Controls/UICommandsControl.xaml", UriKind.RelativeOrAbsolute);
            public static readonly ResourceDescriptor CommandButtonTemplate = ResourceDescriptor.Create<System.Windows.DataTemplate>(Default);
            public static readonly ResourceDescriptor ControlStyle = ResourceDescriptor.Create<Style>(Default);
            public static readonly ResourceDescriptor ControlTemplate = ResourceDescriptor.Create<ControlTemplate>(Default);

            public static readonly ResourceDescriptor ItemContainerStyle = ResourceDescriptor.Create<Style>(Default);
        }

        #endregion
    }
}
