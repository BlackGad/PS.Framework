using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace PS.IoC.Components
{
    public abstract class Module : IModule
    {
        protected Module()
        {
            ThisAssembly = GetType().Assembly;
        }

        public Assembly ThisAssembly { get; }

        public void Configure(IServiceCollection serviceCollection)
        {
            Load(serviceCollection);
            AttachToComponentRegistration(serviceCollection, new ComponentRegistration(serviceCollection));
        }

        protected virtual void AttachToComponentRegistration(IServiceCollection collection, IComponentRegistration registration)
        {
        }

        protected abstract void Load(IServiceCollection collection);
    }

    interface IModuled
    {
        
    }
}
