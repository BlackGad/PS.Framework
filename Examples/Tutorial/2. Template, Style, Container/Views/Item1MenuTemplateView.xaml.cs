using Example.ViewModels;
using PS.MVVM.Patterns;

namespace Example.Views;

public partial class Item1MenuTemplateView : IView<Item1ViewModel>
{
    public Item1MenuTemplateView()
    {
        InitializeComponent();
    }

    public Item1ViewModel ViewModel
    {
        get { return DataContext as Item1ViewModel; }
    }
}
