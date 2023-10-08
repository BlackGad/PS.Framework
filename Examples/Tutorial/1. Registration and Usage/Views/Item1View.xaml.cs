using Example.ViewModels;
using PS.MVVM.Patterns;

namespace Example.Views;

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
