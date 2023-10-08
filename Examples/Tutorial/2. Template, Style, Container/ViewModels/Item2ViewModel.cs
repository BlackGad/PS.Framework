using PS;
using PS.MVVM.Patterns;

namespace Example.ViewModels;

public class Item2ViewModel : BaseNotifyPropertyChanged,
                              IViewModel
{
    private string _title = "Menu item control style changed";

    public string Title
    {
        get { return _title; }
        set { SetField(ref _title, value); }
    }
}
