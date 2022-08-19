using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.WPF.Controls;

namespace PS.Shell.Module.Controls.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<AutoCompleteBoxViewModel>))]
    public partial class AutoCompleteBoxView : IView<AutoCompleteBoxViewModel>
    {
        public AutoCompleteBoxView()
        {
            InitializeComponent();
        }

        public AutoCompleteBoxViewModel ViewModel
        {
            get { return DataContext as AutoCompleteBoxViewModel; }
        }

        private void AutoCompleteBox_OnPreviewItemSelection(object sender, PreviewItemSelectionEventArgs e)
        {
            if (AllowFreeItemControl.IsChecked == true)
            {
                ViewModel.Logger.Info($"Preview item selection START: Input='{e.Input}', Suggested='{e.Item}'");
                e.Item = e.Input;
                ViewModel.Logger.Info($"Preview item selection END: Input='{e.Input}', Selected='{e.Item}'");
            }
        }
    }
}
