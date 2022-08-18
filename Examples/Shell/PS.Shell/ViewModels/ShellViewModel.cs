using System;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Patterns.Aware;
using PS.Shell.Infrastructure.Models.ExamplesService;

namespace PS.Shell.ViewModels;

[DependencyRegisterAsSelf]
public class ShellViewModel : BaseNotifyPropertyChanged,
                              ITitleAware,
                              IViewModel
{
    private object _content;

    public ShellViewModel(IExamplesService examplesService)
    {
        ExamplesService = examplesService ?? throw new ArgumentNullException(nameof(examplesService));
        Title = App.GetApplicationTitle();
    }

    public object Content
    {
        get { return _content; }
        set { SetField(ref _content, value); }
    }

    public IExamplesService ExamplesService { get; }

    public string Title { get; }
}
