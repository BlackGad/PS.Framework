using PS;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;

namespace App.Module.Custom.ViewModels;

[DependencyRegisterAsSelf]
public class Item2ViewModel : BaseNotifyPropertyChanged,
                              IViewModel
{
    private DateTime _date = DateTime.Now;
    private string _title = "Item 2 title";

    public DateTime Date
    {
        get { return _date; }
        set { SetField(ref _date, value); }
    }

    public string Title
    {
        get { return _title; }
        set { SetField(ref _title, value); }
    }
}
