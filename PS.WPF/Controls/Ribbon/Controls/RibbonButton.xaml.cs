using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using PS.Extensions;
using PS.WPF.Extensions;
using PS.WPF.Resources;

namespace PS.WPF.Controls.Ribbon.Controls
{
    public class RibbonButton : System.Windows.Controls.Ribbon.RibbonButton
    {
        #region Static members

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

        #endregion

        #region Constructors

        static RibbonButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonButton), new FrameworkPropertyMetadata(typeof(RibbonButton)));
            ResourceHelper.SetDefaultStyle(typeof(RibbonButton), Resource.ControlStyle);
            ContextMenuProperty.Override(typeof(RibbonButton), coerce: OnCoerceContextMenu);
        }

        #endregion

        #region Override members

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property.AreEqual(CommandParameterProperty))
            {
                this.InternalMethodCall<ButtonBase>("UpdateCanExecute");
            }
        }

        #endregion

        #region Nested type: Resource

        public static class Resource
        {
            #region Constants

            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/Ribbon/Controls/RibbonButton.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default RibbonButton style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default RibbonButton control template",
                                                           resourceDictionary: Default);

            #endregion
        }

        #endregion
    }
}