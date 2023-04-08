using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using PS.IoC.Bootstrapper;

namespace PS.IoC.Components
{
    class ComponentRegistration : IComponentRegistration
    {
        private readonly IServiceCollection _serviceCollection;

        public ComponentRegistration(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public void HandleActivation<TService>(Action<IServiceProvider, TService> action, Action<IServiceProvider> previewActivation = null)
        {
            var serviceType = typeof(TService);

            void PreContainerCreateAction(IServiceCollection collection)
            {
                var descriptors = collection.Where(d => d.ServiceType == serviceType).ToList();
                foreach (var descriptor in descriptors)
                {
                    object ObjectFactory(IServiceProvider provider)
                    {
                        var instance = default(TService);
                        previewActivation?.Invoke(provider);

                        if (descriptor.ImplementationInstance != null)
                        {
                            instance = (TService)descriptor.ImplementationInstance;
                        }
                        else if (descriptor.ImplementationFactory != null)
                        {
                            instance = (TService)descriptor.ImplementationFactory(provider);
                        }
                        else if (descriptor.ImplementationType != null)
                        {
                            instance = (TService)provider.GetRequiredService(descriptor.ImplementationType);
                        }

                        action?.Invoke(provider, instance);
                        return instance;
                    }

                    collection.Remove(descriptor);

                    if (descriptor.ImplementationType != null && collection.All(d => d.ServiceType != descriptor.ImplementationType))
                    {
                        // Transient type factory. Will be used in ObjectFactory to spawn native type 
                        collection.Add(new ServiceDescriptor(descriptor.ImplementationType, descriptor.ImplementationType, ServiceLifetime.Transient));
                    }

                    collection.Add(new ServiceDescriptor(serviceType, ObjectFactory, descriptor.Lifetime));
                }
            }

            _serviceCollection.AddSingleton(new ContainerCreateActions
            {
                PreContainerCreateAction = PreContainerCreateAction
            });
        }
    }
}
