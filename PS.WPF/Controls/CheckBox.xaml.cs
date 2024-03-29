﻿using System;
using System.Windows;
using System.Windows.Controls;
using PS.WPF.Resources;

namespace PS.WPF.Controls
{
    public class CheckBox : System.Windows.Controls.CheckBox
    {
        static CheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckBox), new FrameworkPropertyMetadata(typeof(CheckBox)));
            ResourceHelper.SetDefaultStyle(typeof(CheckBox), Resource.ControlStyle);
        }

        #region Nested type: Resource

        public static class Resource
        {
            private static readonly Uri Default = new Uri("/PS.WPF;component/Controls/CheckBox.xaml", UriKind.RelativeOrAbsolute);
            public static readonly ResourceDescriptor ControlStyle = ResourceDescriptor.Create<Style>(Default);
            public static readonly ResourceDescriptor ControlTemplate = ResourceDescriptor.Create<ControlTemplate>(Default);
        }

        #endregion
    }
}
