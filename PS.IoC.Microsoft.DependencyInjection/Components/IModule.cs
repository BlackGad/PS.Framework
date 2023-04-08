using Microsoft.Extensions.DependencyInjection;

namespace PS.IoC.Components
{
    /// <summary>
    /// Represents a set of components and related functionality
    /// packaged together.
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// Apply the module to the component registry.
        /// </summary>
        void Configure(IServiceCollection serviceCollection);
    }
}