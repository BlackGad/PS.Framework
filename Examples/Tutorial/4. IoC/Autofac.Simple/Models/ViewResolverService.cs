using PS.IoC.Attributes;
using PS.MVVM.Services;

namespace Example.Models
{
    [DependencyRegisterAsInterface(typeof(IViewResolverService))]
    [DependencyLifetime(DependencyLifetime.InstanceSingle)]
    internal class ViewResolverService : PS.MVVM.Services.ViewResolverService
    {
    }
}
