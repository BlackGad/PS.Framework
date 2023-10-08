using System.Windows;
using System.Windows.Controls;

namespace Example;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register(nameof(SelectedItem),
                                    typeof(object),
                                    typeof(MainWindow),
                                    new FrameworkPropertyMetadata(default(object)));

    public MainWindow()
    {
        InitializeComponent();

        AddHandler(MenuItem.ClickEvent, new RoutedEventHandler(MenuItemClicked));
    }

    public object SelectedItem
    {
        get { return GetValue(SelectedItemProperty); }
        set { SetValue(SelectedItemProperty, value); }
    }

    private void MenuItemClicked(object sender, RoutedEventArgs e)
    {
        if (e.OriginalSource is FrameworkElement element)
        {
            SelectedItem = element.DataContext;
        }
    }
}
