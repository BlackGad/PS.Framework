using Example.ViewModels;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;

namespace Example.Views;

[DependencyRegisterAsSelf]
[DependencyRegisterAsInterface(typeof(IView<Item1ViewModel>))]
public partial class Item1View : IView<Item1ViewModel>
{
    public Item1View()
    {
        InitializeComponent();
    }

    public Item1ViewModel ViewModel
    {
        get { return DataContext as Item1ViewModel; }
    }
}
