using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Module.Controls.ViewModels;

namespace PS.Shell.Module.Controls.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<DecimalTextBoxGenericViewModel>))]
    public partial class DecimalTextBoxGenericView : IView<DecimalTextBoxGenericViewModel>
    {
        #region Constructors

        public DecimalTextBoxGenericView()
        {
            InitializeComponent();
        }

        #endregion

        #region IView<DecimalTextBoxGenericViewModel> Members

        public DecimalTextBoxGenericViewModel ViewModel
        {
            get { return DataContext as DecimalTextBoxGenericViewModel; }
        }

        #endregion
    }
}