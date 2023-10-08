using App.Module.Main.ViewModels;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;

namespace App.Module.Main.Views;

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
}
