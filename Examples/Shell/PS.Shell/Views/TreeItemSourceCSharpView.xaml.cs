using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Infrastructure.Models.ExamplesService;

namespace PS.Shell.Views;

[DependencyRegisterAsSelf]
public partial class TreeItemSourceCSharpView : IView<ISourceCSharp>
{
    public TreeItemSourceCSharpView()
    {
        InitializeComponent();
    }

    public ISourceCSharp ViewModel
    {
        get { return DataContext as ISourceCSharp; }
    }
}
