using PS.IoC.Attributes;
using PS.MVVM.Services;

namespace PS.Commander.Models
{
    [DependencyRegisterAsInterface(typeof(IModelResolverService))]
    [DependencyLifetime(DependencyLifetime.InstanceSingle)]
    internal class ModelResolverService : MVVM.Services.ModelResolverService
    {
    }
}
