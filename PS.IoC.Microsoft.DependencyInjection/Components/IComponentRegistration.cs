using System;

namespace PS.IoC.Components
{
    public interface IComponentRegistration
    {
        void HandleActivation<TService>(Action<IServiceProvider, TService> action, Action<IServiceProvider> previewActivation = null);
    }
}
