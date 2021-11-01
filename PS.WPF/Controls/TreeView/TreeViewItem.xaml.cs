using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using PS.WPF.Extensions;
using PS.WPF.Resources;
using PS.WPF.ValueConverters;

namespace PS.WPF.Controls.TreeView
{
    public class TreeViewItem : System.Windows.Controls.TreeViewItem
    {
        #region Property definitions

        public static readonly DependencyProperty AbsoluteHierarchicalItemsOffsetProperty =
            DependencyProperty.Register(nameof(AbsoluteHierarchicalItemsOffset),
                                        typeof(double),
                                        typeof(TreeViewItem),
                                        new FrameworkPropertyMetadata(default(double)));

        public static readonly DependencyProperty ExpandOnDoubleClickProperty =
            DependencyProperty.Register(nameof(ExpandOnDoubleClick),
                                        typeof(bool),
                                        typeof(TreeViewItem),
                                        new FrameworkPropertyMetadata(true));

        public static readonly DependencyProperty HierarchicalItemsOffsetProperty =
            DependencyProperty.Register(nameof(HierarchicalItemsOffset),
                                        typeof(double),
                                        typeof(TreeViewItem),
                                        new FrameworkPropertyMetadata(default(double)));

        public static readonly DependencyProperty LockedExpandStateProperty =
            DependencyProperty.Register(nameof(LockedExpandState),
                                        typeof(bool?),
                                        typeof(TreeViewItem),
                                        new FrameworkPropertyMetadata(default(bool?)));

        public static readonly DependencyProperty ToggleButtonVisibilityProperty =
            DependencyProperty.Register(nameof(ToggleButtonVisibility),
                                        typeof(Visibility),
                                        typeof(TreeViewItem),
                                        new FrameworkPropertyMetadata(default(Visibility)));

        #endregion

        private bool _freezeExpandChange;

        #region Constructors

        static TreeViewItem()
        {
            IsExpandedProperty.Override(typeof(TreeViewItem),
                                        coerce: (d, baseValue, original) =>
                                        {
                                            var owner = (TreeViewItem)d;
                                            if (owner.LockedExpandState.HasValue) return owner.LockedExpandState;
                                            if (owner._freezeExpandChange) return owner.IsExpanded;
                                            return original?.Invoke(d, baseValue) ?? baseValue;
                                        });

            DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeViewItem), new FrameworkPropertyMetadata(typeof(TreeViewItem)));
            ResourceHelper.SetDefaultStyle(typeof(TreeViewItem), Resource.ControlStyle);
        }

        #endregion

        #region Properties

        public double AbsoluteHierarchicalItemsOffset
        {
            get { return (double)GetValue(AbsoluteHierarchicalItemsOffsetProperty); }
            set { SetValue(AbsoluteHierarchicalItemsOffsetProperty, value); }
        }

        public bool ExpandOnDoubleClick
        {
            get { return (bool)GetValue(ExpandOnDoubleClickProperty); }
            set { SetValue(ExpandOnDoubleClickProperty, value); }
        }

        public double HierarchicalItemsOffset
        {
            get { return (double)GetValue(HierarchicalItemsOffsetProperty); }
            set { SetValue(HierarchicalItemsOffsetProperty, value); }
        }

        public bool? LockedExpandState
        {
            get { return (bool?)GetValue(LockedExpandStateProperty); }
            set { SetValue(LockedExpandStateProperty, value); }
        }

        public Visibility ToggleButtonVisibility
        {
            get { return (Visibility)GetValue(ToggleButtonVisibilityProperty); }
            set { SetValue(ToggleButtonVisibilityProperty, value); }
        }

        #endregion

        #region Override members

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            if (element is TreeViewItem treeViewItem)
            {
                BindingOperations.SetBinding(treeViewItem,
                                             AbsoluteHierarchicalItemsOffsetProperty,
                                             new MultiBinding
                                             {
                                                 Converter = NumericConverters.MultiAdd,
                                                 Bindings =
                                                 {
                                                     new Binding
                                                     {
                                                         Source = this,
                                                         Path = new PropertyPath(AbsoluteHierarchicalItemsOffsetProperty)
                                                     },
                                                     new Binding
                                                     {
                                                         Source = this,
                                                         Path = new PropertyPath(HierarchicalItemsOffsetProperty)
                                                     }
                                                 }
                                             });
            }
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
            if (element is TreeViewItem treeViewItem)
            {
                BindingOperations.ClearBinding(treeViewItem, AbsoluteHierarchicalItemsOffsetProperty);
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TreeViewItem();
        }

        /// <summary>Provides class handling for a <see cref="E:System.Windows.UIElement.MouseLeftButtonDown" /> event.</summary>
        /// <param name="e">The event data.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            try
            {
                if (!ExpandOnDoubleClick) _freezeExpandChange = true;
                base.OnMouseLeftButtonDown(e);
            }
            finally
            {
                _freezeExpandChange = false;
            }
        }

        #endregion

        #region Nested type: Resource

        public static class Resource
        {
            #region Constants

            private static readonly Uri Default = new Uri("/PS.WPF;component/Controls/TreeView/TreeViewItem.xaml", UriKind.RelativeOrAbsolute);
            public static readonly ResourceDescriptor ControlStyle = ResourceDescriptor.Create<Style>(Default);
            public static readonly ResourceDescriptor ControlTemplate = ResourceDescriptor.Create<ControlTemplate>(Default);

            public static readonly ResourceDescriptor ExpandCollapseToggleStyle = ResourceDescriptor.Create<ControlTemplate>(Default);
            public static readonly ResourceDescriptor ExpandCollapseToggleTemplate = ResourceDescriptor.Create<ControlTemplate>(Default);

            #endregion
        }

        #endregion
    }
}