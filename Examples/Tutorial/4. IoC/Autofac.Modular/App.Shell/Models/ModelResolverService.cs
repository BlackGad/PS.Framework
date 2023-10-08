using PS.IoC.Attributes;
using PS.MVVM.Services;

namespace App.Shell.Models;

[DependencyRegisterAsInterface(typeof(IModelResolverService))]
[DependencyLifetime(DependencyLifetime.InstanceSingle)]
internal class ModelResolverService : PS.MVVM.Services.ModelResolverService
{
}