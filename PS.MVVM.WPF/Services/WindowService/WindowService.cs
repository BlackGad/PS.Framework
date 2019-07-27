using System;
using System.Threading.Tasks;
using System.Windows;
using PS.MVVM.Extensions;
using PS.WPF.Extensions;

namespace PS.MVVM.Services.WindowService
{
    public abstract class WindowService : IWindowService
    {
        #region IWindowService Members

        TViewModel IWindowService.Show<TViewModel>(TViewModel viewModel, string key)
        {
            var association = GetAssociation(typeof(TViewModel), key);
            if (association == null)
            {
                var message = $"Could not find view that associated with {typeof(TViewModel).Name} view model";
                if (!string.IsNullOrEmpty(key)) message += $" and '{key}' key";
                throw new ArgumentException(message);
            }

            viewModel = (TViewModel)ResolveViewModel(typeof(TViewModel), viewModel);
            var window = ResolveWindow(association);
            var view = ResolveView(association, viewModel);

            window.DataContext = viewModel;
            window.Content = view;

            window.Show();

            return viewModel;
        }

        Task<TViewModel> IWindowService.ShowModalAsync<TViewModel>(TViewModel viewModel, string key)
        {
            var result = new TaskCompletionSource<TViewModel>();

            try
            {
                var association = GetAssociation(typeof(TViewModel), key);

                if (association == null)
                {
                    var message = $"Could not find view that associated with {typeof(TViewModel).Name} view model";
                    if (!string.IsNullOrEmpty(key)) message += $" and '{key}' key";
                    throw new ArgumentException(message);
                }

                viewModel = (TViewModel)ResolveViewModel(typeof(TViewModel), viewModel);
                var view = ResolveView(association, viewModel);

                var window = ResolveWindow(association);
                window.DataContext = viewModel;
                window.Content = view;

                window.Dispatcher.Postpone(() => { result.TrySetResult(window.ShowDialog() == true ? viewModel : default); });
            }
            catch (Exception e)
            {
                result.TrySetException(e);
            }

            return result.Task;
        }

        #endregion

        #region Members

        protected abstract IViewAssociation GetAssociation(Type viewModelType, string key);

        protected virtual Window ProvideWindow(IViewAssociation association)
        {
            return new Window();
        }

        protected virtual object Resolve(Type type)
        {
            return Activator.CreateInstance(type);
        }

        private FrameworkElement ResolveView(IViewAssociation association, object viewModel)
        {
            var frameworkElement = (FrameworkElement)Resolve(association.ViewType);
            if (frameworkElement == null) throw new InvalidOperationException("View not provided");
            if (viewModel != null) frameworkElement.DataContext = viewModel;

            return frameworkElement;
        }

        private object ResolveViewModel(Type viewModelType, object viewModel)
        {
            return viewModel ?? Resolve(viewModelType);
        }

        private Window ResolveWindow(IViewAssociation association)
        {
            var window = ProvideWindow(association);
            if (window == null) throw new InvalidOperationException("Window not provided");
            if (!Equals(window, Application.Current.MainWindow) && window.Owner == null)
            {
                window.Owner = Application.Current.MainWindow;
            }

            var windowStyle = association.GetContainerStyle();
            if (windowStyle != null) window.Style = windowStyle;
            return window;
        }

        #endregion
    }
}