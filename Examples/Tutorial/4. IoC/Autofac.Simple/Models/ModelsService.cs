using System.Collections.ObjectModel;
using Autofac;
using Example.ViewModels;
using PS.IoC.Attributes;

namespace Example.Models;

[DependencyRegisterAsSelf]
[DependencyLifetime(DependencyLifetime.InstanceSingle)]
public class ModelsService
{
    public ModelsService(ILifetimeScope scope)
    {
        Items.Add(scope.Resolve<Item1ViewModel>());
        Items.Add(scope.Resolve<Item2ViewModel>());
    }

    public ObservableCollection<object> Items { get; } = new();
}
