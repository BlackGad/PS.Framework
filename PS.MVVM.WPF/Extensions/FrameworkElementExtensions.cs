using System.Windows;
using PS.Extensions;
using PS.MVVM.Patterns.Aware;

namespace PS.MVVM.Extensions;

public static class FrameworkElementExtensions
{
    public static void ForwardVisualLifetimeToViewModel(this FrameworkElement element)
    {
        if (element == null) return;

        object dataContext = null;

        element.Loaded += (sender, args) =>
        {
            if (dataContext is ILoadedAware loadedAware)
            {
                loadedAware.Loaded();
            }
        };
        element.Unloaded += (sender, args) =>
        {
            if (dataContext is IUnloadedAware unloadedAware)
            {
                unloadedAware.Unloaded();
            }
        };

        element.DataContextChanged += (sender, args) =>
        {
            if (args.NewValue.AreEqual(args.OldValue)) return;

            if (dataContext is IUnloadedAware unloadedAware && element.IsLoaded)
            {
                unloadedAware.Unloaded();
            }

            dataContext = args.NewValue;

            if (dataContext is ILoadedAware loadedAware && element.IsLoaded)
            {
                loadedAware.Loaded();
            }
        };
    }
}
