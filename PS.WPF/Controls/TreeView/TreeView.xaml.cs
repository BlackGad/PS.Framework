using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using PS.Extensions;
using PS.WPF.Resources;

namespace PS.WPF.Controls.TreeView
{
    public class TreeView : System.Windows.Controls.TreeView
    {
        #region Constructors

        static TreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeView), new FrameworkPropertyMetadata(typeof(TreeView)));
            ResourceHelper.SetDefaultStyle(typeof(TreeView), Resource.ControlStyle);
        }

        public TreeView()
        {
            AddHandler(PreviewMouseRightButtonDownEvent, new MouseButtonEventHandler(OnPreviewMouseRightButtonDown));
        }

        #endregion

        #region Override members

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0 && e.Key == Key.Tab) return;

            base.OnKeyDown(e);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TreeViewItem();
        }

        #endregion

        #region Event handlers

        private void OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var hierarchy = e.OriginalSource.Traverse(o => o is Visual visual ? VisualTreeHelper.GetParent(visual) : null).ToList();

            var treeViewItem = hierarchy.Enumerate<TreeViewItem>().FirstOrDefault();
            if (treeViewItem == null) return;

            treeViewItem.Focus();
            e.Handled = true;
        }

        #endregion

        #region Nested type: Resource

        public static class Resource
        {
            #region Constants

            private static readonly Uri Default = new Uri("/PS.WPF;component/Controls/TreeView/TreeView.xaml", UriKind.RelativeOrAbsolute);
            public static readonly ResourceDescriptor ControlStyle = ResourceDescriptor.Create<Style>(Default);

            #endregion
        }

        #endregion
    }
}