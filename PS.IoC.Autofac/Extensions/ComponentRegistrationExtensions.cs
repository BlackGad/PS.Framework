using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;
using Autofac.Core.Resolving.Pipeline;
using PS.IoC.InternalExtensions;

namespace PS.IoC.Extensions
{
    public static class ComponentRegistrationExtensions
    {
        public static void HandleActivation<TService>(this IComponentRegistration registration,
                                                      Action<ILifetimeScope, TService> action,
                                                      Action<ILifetimeScope, IEnumerable<Parameter>> previewActivation = null)
        {
            var typedServices = registration.Services.Enumerate<TypedService>();
            if (typedServices.Any(s => s.ServiceType == typeof(TService)) && registration.Ownership == InstanceOwnership.OwnedByLifetimeScope)
            {
                registration.PipelineBuilding += (sender, pipeline) =>
                {
                    pipeline.Use(PipelinePhase.Activation,
                                 MiddlewareInsertionMode.EndOfPhase,
                                 (c, next) =>
                                 {
                                     previewActivation?.Invoke(c.ActivationScope, c.Parameters);
                                     next(c);
                                     action?.Invoke(c.ActivationScope, (TService)c.Instance);
                                 });
                };
            }
        }
    }
}
