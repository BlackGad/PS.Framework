using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using PS.IoC.Attributes;
using PS.IoC.InternalExtensions;

namespace PS.IoC.Extensions
{
    public static class ContainerBuilderExtensions
    {
        #region Static members

        public static ContainerBuilder RegisterAssemblyTypesWithAttributes(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            return builder.RegisterTypesWithAttributes(assemblies.SelectMany(t => t.GetAssemblyTypes()).ToArray());
        }

        public static ContainerBuilder RegisterTypesWithAttributes(this ContainerBuilder builder, params Type[] types)
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
                        var registrationBuilder = builder.RegisterGeneric(selection.Type)
                                                         .As(selection.GenericRegistrationTypes);
                        SetupLifetime(registrationBuilder, group.Key, selection.LifetimeScopes);
                        if (selection.AutoActivate) registrationBuilder.AutoActivate();
                    }

                    if (selection.RegistrationTypes.Any())
                    {
                        var registrationBuilder = builder.RegisterType(selection.Type)
                                                         .As(selection.RegistrationTypes);

                        SetupLifetime(registrationBuilder, group.Key, selection.LifetimeScopes);
                        if (selection.AutoActivate) registrationBuilder.AutoActivate();
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

        private static void SetupLifetime<TLimit, TActivatorData, TRegistrationStyle>(
            IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder,
            DependencyLifetime lifetime,
            object[] lifetimeScopes)
        {
            switch (lifetime)
            {
                case DependencyLifetime.InstanceSingle:
                    builder.SingleInstance();
                    break;
                case DependencyLifetime.InstancePerDependency:
                    builder.InstancePerDependency();
                    break;
                case DependencyLifetime.InstancePerLifetimeScope:
                    builder.InstancePerLifetimeScope();
                    break;
                case DependencyLifetime.InstancePerMatchingLifetimeScope:
                    builder.InstancePerMatchingLifetimeScope(lifetimeScopes);
                    break;
                case DependencyLifetime.InstancePerRequest:
                    builder.InstancePerRequest();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
    }
}