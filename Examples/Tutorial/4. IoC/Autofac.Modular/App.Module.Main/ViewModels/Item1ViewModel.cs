using PS;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;

namespace App.Module.Main.ViewModels;

[DependencyRegisterAsSelf]
public class Item1ViewModel : BaseNotifyPropertyChanged,
                              IViewModel
{
    private string _description = "Item 1 description";
    private string _title = "Item 1 title";

    public string Description
    {
        get { return _description; }
        set { SetField(ref _description, value); }
    }

    public string Title
    {
        get { return _title; }
        set { SetField(ref _title, value); }
    }
}
