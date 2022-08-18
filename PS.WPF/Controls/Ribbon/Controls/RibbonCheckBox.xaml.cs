using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using PS.Extensions;
using PS.WPF.Resources;

namespace PS.WPF.Controls.Ribbon.Controls
{
    public class RibbonCheckBox : System.Windows.Controls.Ribbon.RibbonCheckBox
    {
        static RibbonCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonCheckBox), new FrameworkPropertyMetadata(typeof(RibbonCheckBox)));
            ResourceHelper.SetDefaultStyle(typeof(RibbonCheckBox), Resource.ControlStyle);
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
            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/Ribbon/Controls/RibbonCheckBox.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default RibbonCheckBox style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default RibbonCheckBox control template",
                                                           resourceDictionary: Default);
        }

        #endregion
    }
}
