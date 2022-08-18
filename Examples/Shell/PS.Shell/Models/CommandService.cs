using PS.IoC.Attributes;
using PS.MVVM.Services.CommandService;

namespace PS.Shell.Models;

[DependencyRegisterAsInterface(typeof(ICommandService))]
[DependencyLifetime(DependencyLifetime.InstanceSingle)]
internal class CommandService : MVVM.Services.CommandService.CommandService
{
}
