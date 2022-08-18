using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Infrastructure.Models.ExamplesService;

namespace PS.Shell.Views;

[DependencyRegisterAsSelf]
public partial class DesignView : IView<IExample>
{
    public DesignView()
    {
        InitializeComponent();
    }

    public IExample ViewModel
    {
        get { return DataContext as IExample; }
    }
}
