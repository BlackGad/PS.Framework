using System;
using Microsoft.Extensions.DependencyInjection;

namespace PS.IoC.Bootstrapper
{
    sealed class ContainerCreateActions
    {
        public Action<IServiceProvider> PostContainerCreateAction { get; set; }

        public Action<IServiceCollection> PreContainerCreateAction { get; set; }
    }
}
