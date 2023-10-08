using System.Windows;
using PS.MVVM.Patterns;

namespace Example.ViewModels;

public class Item3ViewModel : DependencyObject,
                              IViewModel
{
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title),
                                    typeof(string),
                                    typeof(Item3ViewModel),
                                    new FrameworkPropertyMetadata("Item 3 title"));

    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }
}
