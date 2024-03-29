﻿using System;
using System.Windows;
using System.Windows.Controls;
using PS.WPF.Controls;
using PS.WPF.Resources;

namespace PS.Shell.Infrastructure.Components
{
    public class PageControl : Control
    {
        public static readonly DependencyProperty EditorProperty =
            DependencyProperty.Register(nameof(Editor),
                                        typeof(object),
                                        typeof(PageControl),
                                        new FrameworkPropertyMetadata(default(object)));

        public static readonly DependencyProperty PropertiesProperty =
            DependencyProperty.Register(nameof(Properties),
                                        typeof(object),
                                        typeof(PageControl),
                                        new FrameworkPropertyMetadata(default(object)));

        static PageControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PageControl), new FrameworkPropertyMetadata(typeof(PageControl)));
            ResourceHelper.SetDefaultStyle(typeof(PageControl), Resource.ControlStyle);
        }

        public PageControl()
        {
            HeaderedContent.SetHeaderColumnWidth(this, 150);
        }

        public object Editor
        {
            get { return GetValue(EditorProperty); }
            set { SetValue(EditorProperty, value); }
        }

        public object Properties
        {
            get { return GetValue(PropertiesProperty); }
            set { SetValue(PropertiesProperty, value); }
        }

        #region Nested type: Resource

        public static class Resource
        {
            private static readonly Uri Default = new Uri("/PS.Shell.Infrastructure;component/Components/PageControl.xaml", UriKind.RelativeOrAbsolute);
            public static readonly ResourceDescriptor ControlStyle = ResourceDescriptor.Create<Style>(Default);
            public static readonly ResourceDescriptor ControlTemplate = ResourceDescriptor.Create<ControlTemplate>(Default);
        }

        #endregion
    }
}
