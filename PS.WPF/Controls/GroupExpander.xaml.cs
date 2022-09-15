using System;
using System.Windows;
using System.Windows.Controls;
using PS.WPF.Resources;

namespace PS.WPF.Controls
{
    public class GroupExpander : Expander
    {
        public static readonly DependencyProperty HeaderPaddingProperty =
            DependencyProperty.Register(nameof(HeaderPadding),
                                        typeof(Thickness),
                                        typeof(GroupExpander),
                                        new FrameworkPropertyMetadata(default(Thickness)));

        public static readonly DependencyProperty ShowIndentProperty =
            DependencyProperty.Register(nameof(ShowIndent),
                                        typeof(bool),
                                        typeof(GroupExpander),
                                        new FrameworkPropertyMetadata(true));

        static GroupExpander()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GroupExpander), new FrameworkPropertyMetadata(typeof(GroupExpander)));
            ResourceHelper.SetDefaultStyle(typeof(GroupExpander), Resource.ControlStyle);
        }

        public Thickness HeaderPadding
        {
            get { return (Thickness)GetValue(HeaderPaddingProperty); }
            set { SetValue(HeaderPaddingProperty, value); }
        }

        public bool ShowIndent
        {
            get { return (bool)GetValue(ShowIndentProperty); }
            set { SetValue(ShowIndentProperty, value); }
        }

        #region Nested type: Resource

        public static class Resource
        {
            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/GroupExpander.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default GroupExpander style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default GroupExpander control template",
                                                           resourceDictionary: Default);

            public static readonly ResourceDescriptor ToggleButtonStyle =
                ResourceDescriptor.Create<Style>(description: "Default ToggleButton style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ToggleButtonTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default ToggleButton control template",
                                                           resourceDictionary: Default);
        }

        #endregion
    }
}
