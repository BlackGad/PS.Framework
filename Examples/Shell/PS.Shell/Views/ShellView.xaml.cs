using System.Windows;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.ViewModels;

namespace PS.Shell.Views;

[DependencyRegisterAsSelf]
[DependencyRegisterAsInterface(typeof(IView<ShellViewModel>))]
public partial class ShellView : IView<ShellViewModel>
{
    public ShellView()
    {
        InitializeComponent();
    }

    public ShellViewModel ViewModel
    {
        get { return DataContext as ShellViewModel; }
    }

    private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        ViewModel.Content = e.NewValue;
    }
}
