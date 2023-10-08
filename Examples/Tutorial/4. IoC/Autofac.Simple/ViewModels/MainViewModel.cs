using Example.Models;
using PS;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;

namespace Example.ViewModels;

[DependencyRegisterAsSelf]
public class MainViewModel : BaseNotifyPropertyChanged,
                             IViewModel
{
    private string _title = "Main window title";

    public MainViewModel(ModelsService modelsService)
    {
        ModelsService = modelsService;
    }

    public ModelsService ModelsService { get; }

    public string Title
    {
        get { return _title; }
        set { SetField(ref _title, value); }
    }
}
