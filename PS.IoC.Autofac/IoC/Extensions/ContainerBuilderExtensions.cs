using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
using PS.InternalExtensions;
using PS.IoC.Attributes;

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
            var groupedPerLifetimeTypes = types.Where(t => !t.IsAbstract)
                                               .Select(t =>
                                               {
                                                   var lifetimeAttribute = t.GetCustomAttribute<DependencyLifetimeAttribute>();
                                                   return new
                                                   {
                                                       Type = t,
                                                       RegistrationTypes = GetRegistrationTypes(t),
                                                       Lifetime = lifetimeAttribute?.Lifetime ?? DependencyLifetime.InstancePerDependency,
                                                       LifetimeScopes = lifetimeAttribute?.Scopes
                                                   };
                                               })
                                               .Where(s => s.RegistrationTypes.Any())
                                               .ToLookup(s => s.Lifetime, s => s);

            var lifetimeActionMap =
                new Dictionary<DependencyLifetime, Action<IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>, object[]>>
                {
                    { DependencyLifetime.InstanceSingle, (o, t) => o.SingleInstance() },
                    { DependencyLifetime.InstancePerLifetimeScope, (o, t) => o.InstancePerLifetimeScope() },
                    {
                        DependencyLifetime.InstancePerMatchingLifetimeScope,
                        (o, t) => o.InstancePerMatchingLifetimeScope(t.ToArray())
                    },
                    { DependencyLifetime.InstancePerDependency, (o, t) => o.InstancePerDependency() },
                    { DependencyLifetime.InstancePerRequest, (o, t) => o.InstancePerRequest() }
                };

            foreach (var group in groupedPerLifetimeTypes)
            {
                foreach (var selection in group)
                {
                    var fluentBuilder = builder.RegisterTypes(selection.Type)
                                               .As(t => selection.RegistrationTypes);
                    lifetimeActionMap[group.Key](fluentBuilder, selection.LifetimeScopes);
                }
            }

            return builder;
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

        #endregion
    }
}