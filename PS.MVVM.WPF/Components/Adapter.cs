using System;
using System.Windows;

namespace PS.MVVM.Components
{
    
    public abstract class Adapter : Freezable,
                                    IDisposable
    {
        #region Override members

        protected override Freezable CreateInstanceCore()
        {
            throw new NotSupportedException();
        }

        #endregion

        #region IDisposable Members

        public abstract void Dispose();

        #endregion

        #region Event handlers

        protected virtual void ContainerOnLoaded(object sender, RoutedEventArgs e)
        {
        }

        protected virtual void ContainerOnUnloaded(object sender, RoutedEventArgs e)
        {
        }

        #endregion

        #region Members

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

        #endregion
    }

    public abstract class Adapter<T> : Adapter
    {
        #region Members

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

        #endregion
    }
}