using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using PS.WPF.Controls;
using PS.WPF.Resources;

namespace PS.Shell.Infrastructure.Components
{
    public class PageControl : Control
    {
        #region Property definitions

        public static readonly DependencyProperty EditorProperty =
            DependencyProperty.Register(nameof(Editor),
                                        typeof(object),
                                        typeof(PageControl),
                                        new FrameworkPropertyMetadata(default(object)));

        public static readonly DependencyProperty LogProperty =
            DependencyProperty.Register(nameof(Log),
                                        typeof(ObservableCollection<string>),
                                        typeof(PageControl),
                                        new FrameworkPropertyMetadata(default(ObservableCollection<string>)));

        public static readonly DependencyProperty PropertiesProperty =
            DependencyProperty.Register(nameof(Properties),
                                        typeof(object),
                                        typeof(PageControl),
                                        new FrameworkPropertyMetadata(default(object)));

        #endregion

        #region Constructors

        static PageControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PageControl), new FrameworkPropertyMetadata(typeof(PageControl)));
            ResourceHelper.SetDefaultStyle(typeof(PageControl), Resource.ControlStyle);
        }

        public PageControl()
        {
            Log = new ObservableCollection<string>();
            KeyValueContentControl.SetKeyColumnWidth(this, 150);
        }

        #endregion

        #region Properties

        public object Editor
        {
            get { return GetValue(EditorProperty); }
            set { SetValue(EditorProperty, value); }
        }

        public ObservableCollection<string> Log
        {
            get { return (ObservableCollection<string>)GetValue(LogProperty); }
            set { SetValue(LogProperty, value); }
        }

        public object Properties
        {
            get { return GetValue(PropertiesProperty); }
            set { SetValue(PropertiesProperty, value); }
        }

        #endregion

        #region Nested type: Resource

        public static class Resource
        {
            #region Constants

            private static readonly Uri Default = new Uri("/PS.Shell.Infrastructure;component/Components/PageControl.xaml", UriKind.RelativeOrAbsolute);
            public static readonly ResourceDescriptor ControlStyle = ResourceDescriptor.Create<Style>(Default);
            public static readonly ResourceDescriptor ControlTemplate = ResourceDescriptor.Create<ControlTemplate>(Default);

            #endregion
        }

        #endregion
    }
}