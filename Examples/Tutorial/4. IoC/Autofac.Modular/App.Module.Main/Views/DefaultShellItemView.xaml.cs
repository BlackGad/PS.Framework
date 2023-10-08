using App.Module.Main.ViewModels;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;

namespace App.Module.Main.Views;

[DependencyRegisterAsSelf]
public partial class DefaultShellItemView : IView<object>
{
    public DefaultShellItemView()
    {
        InitializeComponent();
    }

    public object ViewModel
    {
        get { return DataContext as Item1ViewModel; }
    }
}
