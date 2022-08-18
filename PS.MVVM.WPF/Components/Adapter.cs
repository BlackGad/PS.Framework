using System;
using System.Windows;

namespace PS.MVVM.Components
{
    public abstract class Adapter : Freezable,
                                    IDisposable
    {
        protected override Freezable CreateInstanceCore()
        {
            throw new NotSupportedException();
        }

        public abstract void Dispose();

        protected virtual void ContainerOnLoaded(object sender, RoutedEventArgs e)
        {
        }

        protected virtual void ContainerOnUnloaded(object sender, RoutedEventArgs e)
        {
        }

        public void Attach(object container)
        {
            if (container is FrameworkElement frameworkElement)
            {
                frameworkElement.Loaded += ContainerOnLoaded;
                frameworkElement.Unloaded += ContainerOnUnloaded;
            }

            OnAttach(container);
        }

        public void Detach(object container)
        {
            OnDetach(container);
            if (container is FrameworkElement frameworkElement)
            {
                frameworkElement.Loaded -= ContainerOnLoaded;
                frameworkElement.Unloaded -= ContainerOnUnloaded;
            }
        }

        protected abstract void OnAttach(object container);

        protected abstract void OnDetach(object container);
    }

    public abstract class Adapter<T> : Adapter
    {
        protected sealed override void OnAttach(object container)
        {
            if (container is T typedContainer)
            {
                OnAttach(typedContainer);
            }
            else
            {
                throw new ArgumentException($"Invalid container type. {typeof(T).Name} expected");
            }
        }

        protected sealed override void OnDetach(object container)
        {
            if (container is T typedContainer)
            {
                OnDetach(typedContainer);
            }
            else
            {
                throw new ArgumentException($"Invalid container type. {typeof(T).Name} expected");
            }
        }

        protected abstract void OnAttach(T container);

        protected abstract void OnDetach(T container);
    }
}
