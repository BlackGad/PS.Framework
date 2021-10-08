using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PS.Extensions;
using PS.WPF.Resources;

namespace PS.WPF.Controls
{
    public class SplitButton : HeaderedItemsControl
    {
        #region Property definitions

        public static readonly DependencyProperty IsMenuOpenedProperty =
            DependencyProperty.Register(nameof(IsMenuOpened),
                                        typeof(bool),
                                        typeof(SplitButton),
                                        new FrameworkPropertyMetadata(default(bool)));

        #endregion

        private ContextMenu _contextMenu;

        #region Constructors

        static SplitButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitButton), new FrameworkPropertyMetadata(typeof(SplitButton)));
            ResourceHelper.SetDefaultStyle(typeof(SplitButton), Resource.ControlStyle);
        }

        #endregion

        #region Properties

        public bool IsMenuOpened
        {
            get { return (bool)GetValue(IsMenuOpenedProperty); }
            set { SetValue(IsMenuOpenedProperty, value); }
        }

        #endregion

        #region Override members

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_ContextMenu") is ContextMenu contextMenu)
            {
                _contextMenu = contextMenu;
            }

            UpdateContextMenu();
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            UpdateContextMenu();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is MenuItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MenuItem();
        }

        #endregion

        #region Members

        private void UpdateContextMenu()
        {
            if (_contextMenu == null || Items.Count == 0) return;

            var itemsToTransfer = Items.Enumerate<DependencyObject>().ToList();
            Items.Clear();

            _contextMenu.Items.Clear();
            _contextMenu.Items.AddRange(itemsToTransfer);
        }

        #endregion

        #region Nested type: Resource

        public static class Resource
        {
            #region Constants

            private static readonly Uri Default = new Uri("/PS.WPF;component/Controls/SplitButton.xaml", UriKind.RelativeOrAbsolute);
            public static readonly ResourceDescriptor ControlStyle = ResourceDescriptor.Create<Style>(Default);
            public static readonly ResourceDescriptor ControlTemplate = ResourceDescriptor.Create<ControlTemplate>(Default);

            public static readonly ResourceDescriptor ItemsPanelTemplate = ResourceDescriptor.Create<ItemsPanelTemplate>(Default);

            #endregion
        }

        #endregion
    }
}