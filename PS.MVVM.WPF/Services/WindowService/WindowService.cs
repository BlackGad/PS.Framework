using System;
using System.Threading.Tasks;
using System.Windows;
using PS.MVVM.Components;
using PS.WPF.Extensions;
using PS.WPF.Resources;

namespace PS.MVVM.Services.WindowService
{
    public abstract class WindowService : IWindowService
    {
        TViewModel IWindowService.Show<TViewModel>(TViewModel viewModel, string region)
        {
            viewModel = (TViewModel)(viewModel as object ?? Resolve(typeof(TViewModel)));
            var window = CreateWindow(viewModel, region);

            OnPreviewWindowShow(window, viewModel, region);

            void OnWindowOnClosed(object sender, EventArgs args)
            {
                window.Closed -= OnWindowOnClosed;
                OnWindowClose(window, viewModel, region);
            }

            window.Closed += OnWindowOnClosed;

            window.Show();

            return viewModel;
        }

        Task<TViewModel> IWindowService.ShowModalAsync<TViewModel>(TViewModel viewModel, string region)
        {
            var result = new TaskCompletionSource<TViewModel>();

            try
            {
                viewModel = (TViewModel)(viewModel as object ?? Resolve(typeof(TViewModel)));
                var window = CreateWindow(viewModel, region);
                window.Dispatcher.Postpone(() =>
                {
                    OnPreviewWindowShow(window, viewModel, region);

                    void OnWindowOnClosed(object sender, EventArgs args)
                    {
                        window.Closed -= OnWindowOnClosed;
                        OnWindowClose(window, viewModel, region);
                    }

                    window.Closed += OnWindowOnClosed;

                    var dialogResult = window.ShowDialog();
                    result.TrySetResult(dialogResult == true ? viewModel : default);
                });
            }
            catch (Exception e)
            {
                result.TrySetException(e);
            }

            return result.Task;
        }

        protected virtual Window CreateWindow()
        {
            return new Window();
        }

        protected abstract IViewAssociation GetAssociation(Type consumerServiceType, Type viewModelType, string key);

        protected virtual void OnPreviewWindowShow<TViewModel>(Window window, TViewModel viewModel, string region)
        {
        }

        protected virtual void OnWindowClose<TViewModel>(Window window, TViewModel viewModel, string region)
        {
        }

        protected virtual object Resolve(Type type)
        {
            return Activator.CreateInstance(type);
        }

        private Window CreateWindow<TViewModel>(TViewModel viewModel, string region)
        {
            var window = CreateWindow();
            if (window == null) throw new InvalidOperationException("Window not provided");
            if (!Equals(window, Application.Current.MainWindow) && window.Owner == null)
            {
                window.Owner = Application.Current.MainWindow;
            }

            window.DataContext = viewModel;
            window.Content = viewModel;

            var viewModelType = viewModel.GetType();

            var styleAssociation = GetAssociation(typeof(StyleResolver), viewModelType, region);
            if (styleAssociation?.Payload is Style payloadStyle)
            {
                window.Style = payloadStyle;
            }

            if (styleAssociation?.Payload is ResourceDescriptor payloadStyleResourceDescriptor)
            {
                window.Style = payloadStyleResourceDescriptor.GetResource<Style>();
            }

            var templateAssociation = GetAssociation(typeof(TemplateResolver), viewModelType, region);
            if (templateAssociation?.Payload is DataTemplate template)
            {
                window.ContentTemplate = template;
            }

            if (templateAssociation?.Payload is ResourceDescriptor payloadTemplateResourceDescriptor)
            {
                window.ContentTemplate = payloadTemplateResourceDescriptor.GetResource<DataTemplate>();
            }

            return window;
        }
    }
}
