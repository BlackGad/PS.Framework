using System.Windows;
using PS.Extensions;

namespace PS.MVVM.Components
{
    public static class Container
    {
        public static readonly DependencyProperty AdapterProperty =
            DependencyProperty.RegisterAttached("Adapter",
                                                typeof(Adapter),
                                                typeof(Container),
                                                new PropertyMetadata(OnAdapterChanged));

        public static Adapter GetAdapter(DependencyObject element)
        {
            return (Adapter)element.GetValue(AdapterProperty);
        }

        public static void SetAdapter(DependencyObject element, Adapter value)
        {
            element.SetValue(AdapterProperty, value);
        }

        private static void OnAdapterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue.AreEqual(e.OldValue)) return;

            if (e.OldValue is Adapter oldAdapter) oldAdapter.Detach(d);
            if (e.NewValue is Adapter newAdapter) newAdapter.Attach(d);
        }
    }
}
