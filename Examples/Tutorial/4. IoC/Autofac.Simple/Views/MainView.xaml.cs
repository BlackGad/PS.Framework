using Example.ViewModels;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;

namespace Example.Views;

[DependencyRegisterAsSelf]
[DependencyRegisterAsInterface(typeof(IView<MainViewModel>))]
public partial class MainView : IView<MainViewModel>
{
    public MainView()
    {
        InitializeComponent();
    }

    public MainViewModel ViewModel
    {
        get { return DataContext as MainViewModel; }
    }
}
