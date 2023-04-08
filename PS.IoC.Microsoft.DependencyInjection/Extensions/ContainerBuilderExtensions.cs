using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using PS.IoC.Attributes;
using PS.IoC.Bootstrapper;
using PS.IoC.Components;
using PS.IoC.InternalExtensions;

namespace PS.IoC.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static IServiceCollection RegisterAssemblyTypesWithAttributes(this IServiceCollection builder, params Assembly[] assemblies)
        {
            return builder.RegisterTypesWithAttributes(assemblies.SelectMany(t => t.GetAssemblyTypes()).ToArray());
        }

        public static IServiceCollection RegisterModule<TModule>(this IServiceCollection builder)
            where TModule : IModule
        {
            var module = Activator.CreateInstance<TModule>();
            module.Configure(builder);
            return builder;
        }

        public static IServiceCollection RegisterTypesWithAttributes(this IServiceCollection builder, params Type[] types)
        {
            var groupedPerLifetimeTypes = types.Where(t => t?.IsAbstract == false)
                                               .Select(t =>
                                               {
                                                   var lifetimeAttribute = t.GetCustomAttribute<DependencyLifetimeAttribute>();
                                                   var dependencyAutoActivateAttribute = t.GetCustomAttribute<DependencyAutoActivateAttribute>();
                                                   return new
                                                   {
                                                       Type = t,
                                                       RegistrationTypes = GetRegistrationTypes(t),
                                                       GenericRegistrationTypes = GetGenericRegistrationTypes(t),
                                                       Lifetime = lifetimeAttribute?.Lifetime ?? DependencyLifetime.InstancePerDependency,
                                                       LifetimeScopes = lifetimeAttribute?.Scopes,
                                                       AutoActivate = dependencyAutoActivateAttribute != null
                                                   };
                                               })
                                               .Where(s => s.RegistrationTypes.Any() || s.GenericRegistrationTypes.Any())
                                               .ToLookup(s => s.Lifetime, s => s);

            foreach (var group in groupedPerLifetimeTypes)
            {
                foreach (var selection in group)
                {
                    if (selection.GenericRegistrationTypes.Any())
                    {
                        var lifetime = GetLifetime(group.Key);
                        foreach (var registrationType in selection.GenericRegistrationTypes)
                        {
                            var descriptor = new ServiceDescriptor(registrationType, selection.Type, lifetime);
                            builder.Add(descriptor);

                            if (selection.AutoActivate)
                            {
                                builder.AddSingleton(new ContainerCreateActions
                                {
                                    PostContainerCreateAction = provider => provider.GetRequiredService(registrationType)
                                });
                            }
                        }
                    }

                    if (selection.RegistrationTypes.Any())
                    {
                        var lifetime = GetLifetime(group.Key);
                        foreach (var registrationType in selection.RegistrationTypes)
                        {
                            var descriptor = new ServiceDescriptor(registrationType, selection.Type, lifetime);
                            builder.Add(descriptor);

                            if (selection.AutoActivate)
                            {
                                builder.AddSingleton(new ContainerCreateActions
                                {
                                    PostContainerCreateAction = provider => provider.GetRequiredService(registrationType)
                                });
                            }
                        }
                    }
                }
            }

            return builder;
        }

        private static Type[] GetGenericRegistrationTypes(Type type)
        {
            var types = new List<Type>();

            foreach (var attribute in type.GetCustomAttributes<DependencyRegisterAsGenericTypeOfAttribute>())
            {
                types.Add(attribute.GenericType);
            }

            return types.Distinct().ToArray();
        }

        private static ServiceLifetime GetLifetime(DependencyLifetime lifetime)
        {
            switch (lifetime)
            {
                case DependencyLifetime.InstanceSingle:
                    return ServiceLifetime.Singleton;
                case DependencyLifetime.InstancePerDependency:
                    return ServiceLifetime.Transient;
                case DependencyLifetime.InstancePerLifetimeScope:
                    return ServiceLifetime.Scoped;
                case DependencyLifetime.InstancePerMatchingLifetimeScope:
                    return ServiceLifetime.Scoped;
                case DependencyLifetime.InstancePerRequest:
                    return ServiceLifetime.Scoped;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static Type[] GetRegistrationTypes(Type type)
        {
            var types = new List<Type>();
            if (type.GetCustomAttributes<DependencyRegisterAsSelfAttribute>().Any()) types.Add(type);

            foreach (var attribute in type.GetCustomAttributes<DependencyRegisterAsInterfaceAttribute>())
            {
                types.Add(attribute.InterfaceType);
            }

            foreach (var attribute in type.GetCustomAttributes<DependencyRegisterAsClosedTypeOfAttribute>())
            {
                var closedTypes = type.TypesAssignableFrom()
                                      .Where(t => t.IsClosedTypeOf(attribute.ClosedType))
                                      .ToArray();
                types.AddRange(closedTypes);
            }

            return types.Distinct().ToArray();
        }
    }
}
