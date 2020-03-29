using PS.IoC.Attributes;
using PS.MVVM.Services;

namespace PS.Shell.Models
{
    [DependencyRegisterAsInterface(typeof(IViewResolverService))]
    [DependencyLifetime(DependencyLifetime.InstanceSingle)]
    internal class ViewResolverService : MVVM.Services.ViewResolverService
    {
    }
}