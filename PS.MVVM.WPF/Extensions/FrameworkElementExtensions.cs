using System.Windows;
using PS.Extensions;
using PS.MVVM.Patterns.Aware;

namespace PS.MVVM.Extensions
{
    public static class FrameworkElementExtensions
    {
        #region Static members

        public static void ForwardVisualLifetimeToViewModel(this FrameworkElement element)
        {
            if (element == null) return;

            element.Loaded += (sender, args) =>
            {
                if (element.DataContext is ILoadedAware loadedAware)
                {
                    loadedAware.Loaded();
                }
            };
            element.Unloaded += (sender, args) =>
            {
                if (element.DataContext is IUnloadedAware unloadedAware)
                {
                    unloadedAware.Unloaded();
                }
            };

            element.DataContextChanged += (sender, args) =>
            {
                if (args.NewValue.AreEqual(args.OldValue)) return;

                if (args.OldValue is IUnloadedAware unloadedAware && element.IsLoaded)
                {
                    unloadedAware.Unloaded();
                }

                if (args.NewValue is ILoadedAware loadedAware && element.IsLoaded)
                {
                    loadedAware.Loaded();
                }
            };
        }

        #endregion
    }
}