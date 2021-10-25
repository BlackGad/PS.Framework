using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shell;
using PS.Patterns.Command;
using PS.WPF.Resources;

namespace PS.WPF.Controls
{
    public class ChromelessWindow : Window
    {
        #region Property definitions

        public static readonly DependencyProperty ButtonCloseVisibilityProperty =
            DependencyProperty.Register("ButtonCloseVisibility",
                                        typeof(Visibility),
                                        typeof(ChromelessWindow),
                                        new FrameworkPropertyMetadata(default(Visibility)));

        public static readonly DependencyProperty ButtonMaximizeVisibilityProperty =
            DependencyProperty.Register("ButtonMaximizeVisibility",
                                        typeof(Visibility),
                                        typeof(ChromelessWindow),
                                        new FrameworkPropertyMetadata(default(Visibility)));

        public static readonly DependencyProperty ButtonMinimizeVisibilityProperty =
            DependencyProperty.Register("ButtonMinimizeVisibility",
                                        typeof(Visibility),
                                        typeof(ChromelessWindow),
                                        new FrameworkPropertyMetadata(default(Visibility)));

        private static readonly DependencyPropertyKey CloseCommandPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(CloseCommand),
                                                typeof(ICommand),
                                                typeof(ChromelessWindow),
                                                new FrameworkPropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty CloseCommandProperty = CloseCommandPropertyKey.DependencyProperty;

        public static readonly DependencyProperty HeaderBackgroundProperty =
            DependencyProperty.Register("HeaderBackground",
                                        typeof(Brush),
                                        typeof(ChromelessWindow),
                                        new FrameworkPropertyMetadata(default(Brush)));

        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register("HeaderTemplate",
                                        typeof(System.Windows.DataTemplate),
                                        typeof(ChromelessWindow),
                                        new FrameworkPropertyMetadata(default(System.Windows.DataTemplate)));

        public static readonly DependencyProperty LinkHeaderAndContentAreasProperty =
            DependencyProperty.Register(nameof(LinkHeaderAndContentAreas),
                                        typeof(bool),
                                        typeof(ChromelessWindow),
                                        new FrameworkPropertyMetadata(default(bool)));

        private static readonly DependencyPropertyKey MaximizedContentMarginPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MaximizedContentMargin),
                                                typeof(Thickness),
                                                typeof(ChromelessWindow),
                                                new FrameworkPropertyMetadata(default(Thickness)));

        public static readonly DependencyProperty MaximizedContentMarginProperty = MaximizedContentMarginPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey MaximizeRestoreCommandPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MaximizeRestoreCommand),
                                                typeof(ICommand),
                                                typeof(ChromelessWindow),
                                                new FrameworkPropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty MaximizeRestoreCommandProperty = MaximizeRestoreCommandPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey MinimizeCommandPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MinimizeCommand),
                                                typeof(ICommand),
                                                typeof(ChromelessWindow),
                                                new FrameworkPropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty MinimizeCommandProperty = MinimizeCommandPropertyKey.DependencyProperty;

        public static readonly DependencyProperty WindowIconVisibilityProperty =
            DependencyProperty.Register(nameof(WindowIconVisibility),
                                        typeof(Visibility),
                                        typeof(ChromelessWindow),
                                        new FrameworkPropertyMetadata(default(Visibility)));

        #endregion

        #region Constants

        #endregion

        #region Constructors

        static ChromelessWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChromelessWindow), new FrameworkPropertyMetadata(typeof(ChromelessWindow)));
            ResourceHelper.SetDefaultStyle(typeof(ChromelessWindow), Resource.ControlStyle);

            var existingIsResizablePropertyMetadata = IsResizableProperty.GetMetadata(typeof(ChromelessWindow));
            IsResizableProperty.OverrideMetadata(typeof(ChromelessWindow),
                                                 new FrameworkPropertyMetadata(
                                                     existingIsResizablePropertyMetadata.DefaultValue,
                                                     (o, args) => { },
                                                     existingIsResizablePropertyMetadata.CoerceValueCallback));
        }

        public ChromelessWindow()
        {
            CloseCommand = new RelayCommand(Close);
            MinimizeCommand = new RelayCommand(Minimize);
            MaximizeRestoreCommand = new RelayCommand(MaximizeRestore);

            WindowChrome.SetWindowChrome(this,
                                         new WindowChrome
                                         {
                                             CaptionHeight = Components.SystemParameters.WindowCaptionHeightWithResizeFrame
                                         });

            var borderWidth = SystemParameters.FixedFrameVerticalBorderWidth + SystemParameters.ResizeFrameVerticalBorderWidth;
            var borderHeight = SystemParameters.FixedFrameHorizontalBorderHeight + SystemParameters.ResizeFrameHorizontalBorderHeight;

            MaximizedContentMargin = new Thickness(borderWidth, borderHeight, borderWidth, 0);
        }

        #endregion

        #region Properties

        public Visibility ButtonCloseVisibility
        {
            get { return (Visibility)GetValue(ButtonCloseVisibilityProperty); }
            set { SetValue(ButtonCloseVisibilityProperty, value); }
        }

        public Visibility ButtonMaximizeVisibility
        {
            get { return (Visibility)GetValue(ButtonMaximizeVisibilityProperty); }
            set { SetValue(ButtonMaximizeVisibilityProperty, value); }
        }

        public Visibility ButtonMinimizeVisibility
        {
            get { return (Visibility)GetValue(ButtonMinimizeVisibilityProperty); }
            set { SetValue(ButtonMinimizeVisibilityProperty, value); }
        }

        public ICommand CloseCommand
        {
            get { return (ICommand)GetValue(CloseCommandProperty); }
            private set { SetValue(CloseCommandPropertyKey, value); }
        }

        public Brush HeaderBackground
        {
            get { return (Brush)GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }

        public System.Windows.DataTemplate HeaderTemplate
        {
            get { return (System.Windows.DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public bool LinkHeaderAndContentAreas
        {
            get { return (bool)GetValue(LinkHeaderAndContentAreasProperty); }
            set { SetValue(LinkHeaderAndContentAreasProperty, value); }
        }

        public Thickness MaximizedContentMargin
        {
            get { return (Thickness)GetValue(MaximizedContentMarginProperty); }
            private set { SetValue(MaximizedContentMarginPropertyKey, value); }
        }

        public ICommand MaximizeRestoreCommand
        {
            get { return (ICommand)GetValue(MaximizeRestoreCommandProperty); }
            private set { SetValue(MaximizeRestoreCommandPropertyKey, value); }
        }

        public ICommand MinimizeCommand
        {
            get { return (ICommand)GetValue(MinimizeCommandProperty); }
            private set { SetValue(MinimizeCommandPropertyKey, value); }
        }

        public Visibility WindowIconVisibility
        {
            get { return (Visibility)GetValue(WindowIconVisibilityProperty); }
            set { SetValue(WindowIconVisibilityProperty, value); }
        }

        #endregion

        #region Override members

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            //Custom windows chrome has arrange issue on not default size content. So chatter SizeToContent to invalidate window size
            if (SizeToContent != SizeToContent.Manual)
            {
                var initialSizeToContent = SizeToContent;
                SizeToContent = SizeToContent.Manual;
                SizeToContent = initialSizeToContent;
            }

            //Custom windows chrome has arrange issue. So force visual invalidation on source initialized
            InvalidateVisual();
        }

        #endregion

        #region Members

        private void MaximizeRestore()
        {
            if (!IsResizable) return;

            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }

        private void Minimize()
        {
            WindowState = WindowState.Minimized;
        }

        #endregion

        #region Nested type: Resource

        public static class Resource
        {
            #region Constants

            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/ChromelessWindow.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ButtonCloseGeometry =
                ResourceDescriptor.Create<Geometry>(description: "Button close geometry",
                                                    resourceDictionary: Default);

            public static readonly ResourceDescriptor ButtonMaximizeGeometry =
                ResourceDescriptor.Create<Geometry>(description: "Button maximize geometry",
                                                    resourceDictionary: Default);

            public static readonly ResourceDescriptor ButtonMinimizeGeometry =
                ResourceDescriptor.Create<Geometry>(description: "Button minimize geometry",
                                                    resourceDictionary: Default);

            public static readonly ResourceDescriptor ButtonRestoreGeometry =
                ResourceDescriptor.Create<Geometry>(description: "Button restore geometry",
                                                    resourceDictionary: Default);

            public static readonly ResourceDescriptor CommandButtonStyle =
                ResourceDescriptor.Create<Style>(description: "Default command button style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor CommandCloseButtonStyle =
                ResourceDescriptor.Create<Style>(description: "Default command for close button style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor CommandMaximizeRestoreButtonStyle =
                ResourceDescriptor.Create<Style>(description: "Default command for maximize restore button style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor CommandMinimizeButtonStyle =
                ResourceDescriptor.Create<Style>(description: "Default command for minimize button style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default Window style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default Window control template",
                                                           resourceDictionary: Default);

            public static readonly ResourceDescriptor HeaderTemplate =
                ResourceDescriptor.Create<System.Windows.DataTemplate>(description: "Default Window header date template",
                                                                       resourceDictionary: Default);

            #endregion
        }

        #endregion
    }
}