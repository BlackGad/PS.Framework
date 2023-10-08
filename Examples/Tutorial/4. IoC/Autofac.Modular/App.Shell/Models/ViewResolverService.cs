using PS.IoC.Attributes;
using PS.MVVM.Services;

namespace App.Shell.Models;

[DependencyRegisterAsInterface(typeof(IViewResolverService))]
[DependencyLifetime(DependencyLifetime.InstanceSingle)]
internal class ViewResolverService : PS.MVVM.Services.ViewResolverService
{
}