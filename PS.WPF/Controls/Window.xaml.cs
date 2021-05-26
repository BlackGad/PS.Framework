using System;
using System.Windows;
using System.Windows.Controls;
using PS.WPF.Resources;

namespace PS.WPF.Controls
{
    public class Window : System.Windows.Window
    {
        #region Property definitions

        public static readonly DependencyProperty CommandsProperty =
            DependencyProperty.Register("Commands",
                                        typeof(UICommandCollection),
                                        typeof(Window),
                                        new FrameworkPropertyMetadata(default(UICommandCollection), null, OnCoerceCommands));

        public static readonly DependencyProperty CommandButtonsHorizontalAlignmentProperty =
            DependencyProperty.Register("CommandButtonsHorizontalAlignment",
                                        typeof(HorizontalAlignment),
                                        typeof(Window),
                                        new FrameworkPropertyMetadata(default(HorizontalAlignment)));

        public static readonly DependencyProperty IsResizableProperty =
            DependencyProperty.Register("IsResizable",
                                        typeof(bool),
                                        typeof(Window),
                                        new FrameworkPropertyMetadata(true, OnResizableChanged));

        #endregion

        #region Static members

        private static object OnCoerceCommands(DependencyObject d, object baseValue)
        {
            return baseValue ?? new UICommandCollection();
        }

        private static void OnResizableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (Window)d;
            if (e.NewValue is bool value)
            {
                owner.ResizeMode = value ? ResizeMode.CanResize : ResizeMode.NoResize;
            }
        }

        #endregion

        #region Constructors

        static Window()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(typeof(Window)));
            ResourceHelper.SetDefaultStyle(typeof(Window), WindowResource.ControlStyle);
        }

        public Window()
        {
            CoerceValue(CommandsProperty);
        }

        #endregion

        #region Properties

        public HorizontalAlignment CommandButtonsHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(CommandButtonsHorizontalAlignmentProperty); }
            set { SetValue(CommandButtonsHorizontalAlignmentProperty, value); }
        }

        public UICommandCollection Commands
        {
            get { return (UICommandCollection)GetValue(CommandsProperty); }
            set { SetValue(CommandsProperty, value); }
        }

        public bool IsResizable
        {
            get { return (bool)GetValue(IsResizableProperty); }
            set { SetValue(IsResizableProperty, value); }
        }

        #endregion

        #region Override members

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var source = DependencyPropertyHelper.GetValueSource(this, CommandsProperty).BaseValueSource;
            if (source == BaseValueSource.Style && Commands != null)
            {
                //Commands collection instance was set via style. Because we use SharedResourceDictionary instance sharing as well.
                //We need to make a duplicate.
                SetCurrentValue(CommandsProperty, new UICommandCollection(Commands));
            }
        }

        #endregion

        #region Nested type: WindowResource

        public static class WindowResource
        {
            #region Constants

            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/Window.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default Window style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default Window control template",
                                                           resourceDictionary: Default);

            #endregion
        }

        #endregion
    }
}