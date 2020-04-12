using System.Windows;
using PS.Shell.Module.Diagram.Controls.MVVM;

namespace PS.Shell.Module.Diagram.Controls
{
    public class NodeProvider : DependencyObject
    {
        #region Property definitions

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(nameof(Data),
                                        typeof(INode),
                                        typeof(NodeProvider),
                                        new FrameworkPropertyMetadata(default(INode)));

        #endregion

        #region Properties

        public INode Data
        {
            get { return (INode)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        #endregion
    }
}