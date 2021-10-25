using System.Windows;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Module.Ribbon.ViewModels;

namespace PS.Shell.Module.Ribbon.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<RibbonViewModel>))]
    public partial class RibbonView : IView<RibbonViewModel>
    {
        #region Constructors

        public RibbonView()
        {
            InitializeComponent();
        }

        #endregion

        #region IView<RibbonViewModel> Members

        public RibbonViewModel ViewModel
        {
            get { return DataContext as RibbonViewModel; }
        }

        #endregion

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            
        }
    }
}