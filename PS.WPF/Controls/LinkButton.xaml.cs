using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using PS.Extensions;
using PS.WPF.Resources;

namespace PS.WPF.Controls
{
    public class LinkButton : System.Windows.Controls.Button
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text",
                                        typeof(string),
                                        typeof(LinkButton),
                                        new FrameworkPropertyMetadata(default(string)));

        static LinkButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LinkButton), new FrameworkPropertyMetadata(typeof(LinkButton)));
            ResourceHelper.SetDefaultStyle(typeof(LinkButton), Resource.ControlStyle);
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property.AreEqual(CommandParameterProperty))
            {
                this.InternalMethodCall<ButtonBase>("UpdateCanExecute");
            }
        }

        #region Nested type: Resource

        public static class Resource
        {
            private static readonly Uri Default = new Uri("/PS.WPF;component/Controls/LinkButton.xaml", UriKind.RelativeOrAbsolute);
            public static readonly ResourceDescriptor ControlStyle = ResourceDescriptor.Create<Style>(Default);
            public static readonly ResourceDescriptor ControlTemplate = ResourceDescriptor.Create<ControlTemplate>(Default);
        }

        #endregion
    }
}
