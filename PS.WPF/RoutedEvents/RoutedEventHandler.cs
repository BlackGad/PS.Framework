using System.Windows;

namespace PS.WPF.RoutedEvents
{
    public delegate void RoutedEventHandler<in T>(object sender, T e)
        where T : RoutedEventArgs;
}
