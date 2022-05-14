using System;
using System.Windows;
using System.Windows.Controls;
using PS.WPF.Resources;

namespace PS.WPF.Docking.Controls
{
    public class DockingHost : ItemsControl,
                               IDockingHost
    {
        #region Constructors

        static DockingHost()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DockingHost), new FrameworkPropertyMetadata(typeof(DockingHost)));
            ResourceHelper.SetDefaultStyle(typeof(DockingHost), Resource.ControlStyle);
        }

        #endregion

        #region Nested type: Resource

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return base.IsItemItsOwnContainerOverride(item);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return base.GetContainerForItemOverride();
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
        }

        public static class Resource
        {
            #region Constants

            private static readonly Uri Default = new Uri("/PS.WPF.Docking;component/Controls/DockingHost.xaml", UriKind.RelativeOrAbsolute);
            public static readonly ResourceDescriptor ControlStyle = ResourceDescriptor.Create<Style>(Default);
            public static readonly ResourceDescriptor ControlTemplate = ResourceDescriptor.Create<ControlTemplate>(Default);

            #endregion
        }

        #endregion
    }
}