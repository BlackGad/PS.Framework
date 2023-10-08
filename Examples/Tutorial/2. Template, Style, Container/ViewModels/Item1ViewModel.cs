using PS;
using PS.MVVM.Patterns;

namespace Example.ViewModels;

public class Item1ViewModel : BaseNotifyPropertyChanged,
                              IViewModel
{
    private string _title = "Menu item template changed";

    public string Title
    {
        get { return _title; }
        set { SetField(ref _title, value); }
    }
}
