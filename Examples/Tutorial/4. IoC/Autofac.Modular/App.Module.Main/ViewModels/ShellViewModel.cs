using PS;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;

namespace App.Module.Main.ViewModels;

[DependencyRegisterAsSelf]
public class ShellViewModel : BaseNotifyPropertyChanged,
                             IViewModel
{
    private string _title = "Main window title";

    public string Title
    {
        get { return _title; }
        set { SetField(ref _title, value); }
    }
}
