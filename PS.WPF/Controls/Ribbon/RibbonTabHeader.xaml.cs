using System;
using System.Windows;
using System.Windows.Controls;
using PS.WPF.Extensions;
using PS.WPF.Resources;

namespace PS.WPF.Controls.Ribbon
{
    public class RibbonTabHeader : System.Windows.Controls.Ribbon.RibbonTabHeader
    {
        public static readonly DependencyProperty IsFirstInContextualGroupProperty =
            DependencyProperty.Register(nameof(IsFirstInContextualGroup),
                                        typeof(bool),
                                        typeof(RibbonTabHeader),
                                        new FrameworkPropertyMetadata(default(bool)));

        public static readonly DependencyProperty IsLastInContextualGroupProperty =
            DependencyProperty.Register(nameof(IsLastInContextualGroup),
                                        typeof(bool),
                                        typeof(RibbonTabHeader),
                                        new FrameworkPropertyMetadata(default(bool)));

        private static object OnCoerceContextMenu(DependencyObject sender, object value, CoerceValueCallback originCallback)
        {
            var menu = originCallback?.Invoke(sender, value) ?? value;
            if (menu is ContextMenu contextMenu)
            {
                var ribbon = (Ribbon)sender.GetValue(RibbonProperty);
                ribbon?.ValidateContextMenu(contextMenu);
            }

            return menu;
        }

        static RibbonTabHeader()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonTabHeader), new FrameworkPropertyMetadata(typeof(RibbonTabHeader)));
            ResourceHelper.SetDefaultStyle(typeof(RibbonTabHeader), Resource.ControlStyle);
            ContextMenuProperty.Override(typeof(RibbonTabHeader), coerce: OnCoerceContextMenu);
        }

        public RibbonTabHeader()
        {
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        public bool IsFirstInContextualGroup
        {
            get { return (bool)GetValue(IsFirstInContextualGroupProperty); }
            set { SetValue(IsFirstInContextualGroupProperty, value); }
        }

        public bool IsLastInContextualGroup
        {
            get { return (bool)GetValue(IsLastInContextualGroupProperty); }
            set { SetValue(IsLastInContextualGroupProperty, value); }
        }

        private void OnLayoutUpdated(object sender, EventArgs e)
        {
            if (Ribbon == null || ContextualTabGroup == null) return;

            if (!ContextualTabGroup.IsVisible)
            {
                IsFirstInContextualGroup = false;
                IsLastInContextualGroup = false;
                return;
            }

            var contextualTabRelativeLeft = ContextualTabGroup.TranslatePoint(new Point(), Ribbon).X;
            var contextualTabRelativeRight = contextualTabRelativeLeft + ContextualTabGroup.ActualWidth;

            var thisLeft = TranslatePoint(new Point(), Ribbon).X;
            var thisRight = thisLeft + ActualWidth;

            IsFirstInContextualGroup = Math.Abs(contextualTabRelativeLeft - thisLeft) < 2;
            IsLastInContextualGroup = Math.Abs(contextualTabRelativeRight - thisRight) < 2;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            LayoutUpdated += OnLayoutUpdated;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            LayoutUpdated -= OnLayoutUpdated;
        }

        #region Nested type: Resource

        public static class Resource
        {
            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/Ribbon/RibbonTabHeader.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default RibbonTabHeader style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default RibbonTabHeader control template",
                                                           resourceDictionary: Default);
        }

        #endregion
    }
}
