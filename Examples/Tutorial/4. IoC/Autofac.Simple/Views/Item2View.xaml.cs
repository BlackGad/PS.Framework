using Example.ViewModels;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;

namespace Example.Views;

[DependencyRegisterAsSelf]
[DependencyRegisterAsInterface(typeof(IView<Item2ViewModel>))]
public partial class Item2View : IView<Item2ViewModel>
{
    public Item2View()
    {
        InitializeComponent();
    }

    public Item2ViewModel ViewModel
    {
        get { return DataContext as Item2ViewModel; }
    }
}
