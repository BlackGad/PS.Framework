using System;
using System.Linq;
using Autofac;
using Autofac.Core;
using Autofac.Core.Resolving;
using PS.InternalExtensions;

namespace PS.IoC.Extensions
{
    public static class ComponentRegistrationExtensions
    {
        #region Static members

        public static void HandleActivation<TService>(this IComponentRegistration registration, Action<ILifetimeScope, TService> action)
        {
            var typedServices = registration.Services.Enumerate<TypedService>();
            if (typedServices.Any(s => s.ServiceType == typeof(TService)))
            {
                registration.Activated += (sender, args) =>
                {
                    var lookup = (IInstanceLookup)args.Context;
                    action?.Invoke(lookup.ActivationScope, (TService)args.Instance);
                };
            }
        }

        #endregion
    }
}