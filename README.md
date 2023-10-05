# PS.Framework

| Branch | Build status | Package |
| ------ | ------------ | ------- |
|Master  | [![Build Status](https://dev.azure.com/BlackGad/PS.Framework/_apis/build/status/PS.Framework%20-%20Release?branchName=master)](https://dev.azure.com/BlackGad/PS.Framework/_build/latest?definitionId=4&branchName=master) | [![NuGet version (PS.MVVM.WPF)](https://img.shields.io/nuget/v/PS.MVVM.WPF?style=flat-square)](https://www.nuget.org/packages/PS.MVVM.WPF/) |
| CI     | [![Build Status](https://dev.azure.com/BlackGad/PS.Framework/_apis/build/status/PS.Framework%20-%20Release?branchName=ci)](https://dev.azure.com/BlackGad/PS.Framework/_build/latest?definitionId=4&branchName=ci) | [![MyGet version (PS.MVVM.WPF)](https://img.shields.io/myget/ps-projects/v/PS.MVVM.WPF.svg?style=flat-square&label=MyGet)](https://www.myget.org/feed/ps-projects/package/nuget/PS.MVVM.WPF) |

## Description
Lightweight and fast MVVM framework.

## Core Features
- **Built-in Selectors:** The framework provides native selectors for Data, Style, and Container. These selectors facilitate efficient data binding, style management, and container operations in WPF contexts.

- **DI Framework Integration:** PS.Framework integrates seamlessly with established Dependency Injection (DI) frameworks, specifically [Autofac](https://autofac.org/) and [Microsoft dependency injection](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection). This integration allows for dependency resolution and inversion of control, leading to modular and testable code structures.

- **Scalability:** PS.Framework is scalable, catering to both small-scale utilities and large enterprise applications. It ensures consistent performance and responsiveness across varying project sizes.

- **Toolset:** The framework includes a suite of tools and services that enhance WPF development, reducing boilerplate and improving efficiency.

## Installation
To install the PS Framework, use the following NuGet command:
```
Install-Package PS.MVVM.WPF
```

# MVVM Services

In essence, the **PS.MVVM** project provides tools and services to facilitate the implementation of the MVVM pattern, making it easier for developers to separate their application's logic from its presentation.

```
Install-Package PS.MVVM
```

## BroadcastService

This service provides functionality to broadcast events to subscribers.
It maintains a list of subscriptions and allows for broadcasting events to these subscribers.
The service provides methods to subscribe and unsubscribe from events.
It uses a synchronization context to ensure thread safety.

## CommandService

This service deals with commands in the MVVM pattern.
It maintains a registry of commands and provides methods to add commands, create views of commands, and find commands.
The service can handle activation of commands and provides event handling capabilities.

## ModelResolverService

This service provides functionality to resolve models.
It maintains storage for observable model collections and observable model objects.
The service provides methods to retrieve these observable collections and objects.

## ViewResolverService

This service is responsible for resolving views.
It maintains associations between consumer service types, view models, and regions.
The service provides methods to associate views and find associated views.

# MVVM WPF Adaptation

The `PS.MVVM.WPF` project within the **PS.Framework** repository provides a set of components, services, and view models tailored for WPF applications, particularly for implementing the MVVM (Model-View-ViewModel) pattern. Here's a brief overview of the components:

```
Install-Package PS.MVVM.WPF
```

## Resolvers

In the MVVM architecture within WPF applications, resolvers play a pivotal role in dynamic UI determination. They ensure the appropriate visual elements are chosen based on the underlying data contexts.

1. **BaseResolver**:
   - An abstract generic base class tailored for service interactions within XAML. It provides foundational mechanisms for consistent service retrieval across derived resolvers.

2. **BaseViewResolver**:
   - A specialized XAML resolver for operations with the `IViewResolverService`. The service interacts with three distinct selectors:
     - **StyleResolver**: Works with WPF's [`StyleSelector`](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.styleselector?view=windowsdesktop-7.0), determining the right style for views.
     - **ContainerResolver**: Engages with [`ItemContainerTemplateSelector`](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.itemcontainertemplateselector?view=windowsdesktop-7.0), ensuring the correct container for items in collection controls.
     - **TemplateResolver**: Operates with [`DataTemplateSelector`](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.datatemplateselector?view=windowsdesktop-7.0), selecting the appropriate data template based on the data context.

## WindowService:

Service offers methods to display windows, either in a standard or modal fashion, based on the associated view model. This ensures a clear separation of concerns between the view and its underlying logic, adhering to the principles of the MVVM pattern.
