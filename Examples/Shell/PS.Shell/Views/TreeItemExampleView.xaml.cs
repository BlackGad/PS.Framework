using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Infrastructure.Models.ExamplesService;

namespace PS.Shell.Views;

[DependencyRegisterAsSelf]
public partial class TreeItemExampleView : IView<IExample>
{
    public TreeItemExampleView()
    {
        InitializeComponent();
    }

    public IExample ViewModel
    {
        get { return DataContext as IExample; }
    }
}
