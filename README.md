# Build

| Branch | Build status |
| ------ | ------------ |
|Master  | [![Build Status](https://dev.azure.com/BlackGad/PS.Framework/_apis/build/status/PS.Framework%20-%20Release?branchName=master)](https://dev.azure.com/BlackGad/PS.Framework/_build/latest?definitionId=4&branchName=master) |
| CI     | [![Build Status](https://dev.azure.com/BlackGad/PS.Framework/_apis/build/status/PS.Framework%20-%20Release?branchName=ci)](https://dev.azure.com/BlackGad/PS.Framework/_build/latest?definitionId=4&branchName=ci) |

# Packages
| Name                                  | Master                                                                                                                                                                                    | CI                                                                                                                                                                                                                                                                        |
| ------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| PS.Primitives                         | [![NuGet version (PS.Primitives)](https://img.shields.io/nuget/v/PS.Primitives?style=flat-square)](https://www.nuget.org/packages/PS.Primitives/)                                         | [![MyGet version (PS.Primitives)](https://img.shields.io/myget/ps-projects/v/PS.Primitives.svg?style=flat-square&label=MyGet)](https://www.myget.org/feed/ps-projects/package/nuget/PS.Primitives)                                                                        |
| PS.WPF                                | [![NuGet version (PS.WPF)](https://img.shields.io/nuget/v/PS.WPF?style=flat-square)](https://www.nuget.org/packages/PS.WPF/)                                                              | [![MyGet version (PS.WPF)](https://img.shields.io/myget/ps-projects/v/PS.WPF.svg?style=flat-square&label=MyGet)](https://www.myget.org/feed/ps-projects/package/nuget/PS.WPF)                                                                                             |
| PS.MVVM                               | [![NuGet version (PS.MVVM)](https://img.shields.io/nuget/v/PS.MVVM?style=flat-square)](https://www.nuget.org/packages/PS.MVVM/)                                                           | [![MyGet version (PS.MVVM)](https://img.shields.io/myget/ps-projects/v/PS.MVVM.svg?style=flat-square&label=MyGet)](https://www.myget.org/feed/ps-projects/package/nuget/PS.MVVM)                                                                                          |
| PS.MVVM.WPF                           | [![NuGet version (PS.MVVM.WPF)](https://img.shields.io/nuget/v/PS.MVVM.WPF?style=flat-square)](https://www.nuget.org/packages/PS.MVVM.WPF/)                                               | [![MyGet version (PS.MVVM.WPF)](https://img.shields.io/myget/ps-projects/v/PS.MVVM.WPF.svg?style=flat-square&label=MyGet)](https://www.myget.org/feed/ps-projects/package/nuget/PS.MVVM.WPF)                                                                              |
| PS.IoC                                | [![NuGet version (PS.IoC)](https://img.shields.io/nuget/v/PS.IoC?style=flat-square)](https://www.nuget.org/packages/PS.IoC/)                                                              | [![MyGet version (PS.IoC)](https://img.shields.io/myget/ps-projects/v/PS.IoC.svg?style=flat-square&label=MyGet)](https://www.myget.org/feed/ps-projects/package/nuget/PS.IoC)                                                                                             |
| PS.IoC.Autofac                        | [![NuGet version (PS.IoC.Autofac)](https://img.shields.io/nuget/v/PS.IoC.Autofac?style=flat-square)](https://www.nuget.org/packages/PS.IoC.Autofac/)                                      | [![MyGet version (PS.IoC.Autofac)](https://img.shields.io/myget/ps-projects/v/PS.IoC.Autofac.svg?style=flat-square&label=MyGet)](https://www.myget.org/feed/ps-projects/package/nuget/PS.IoC.Autofac)                                                                     |
| PS.IoC.Microsoft.DependencyInjection  | [![NuGet version (PS.IoC.Microsoft.DependencyInjection)](https://img.shields.io/nuget/v/PS.MVVM?style=flat-square)](https://www.nuget.org/packages/PS.IoC.Microsoft.DependencyInjection/) | [![MyGet version (PS.IoC.Microsoft.DependencyInjection)](https://img.shields.io/myget/ps-projects/v/PS.IoC.Microsoft.DependencyInjection.svg?style=flat-square&label=MyGet)](https://www.myget.org/feed/ps-projects/package/nuget/PS.IoC.Microsoft.DependencyInjection)   |
| PS.Windows.Interop                    | [![NuGet version (PS.Windows.Interop)](https://img.shields.io/nuget/v/PS.MVVM?style=flat-square)](https://www.nuget.org/packages/PS.Windows.Interop/)                                     | [![MyGet version (PS.Windows.Interop)](https://img.shields.io/myget/ps-projects/v/PS.Windows.Interop.svg?style=flat-square&label=MyGet)](https://www.myget.org/feed/ps-projects/package/nuget/PS.Windows.Interop)                                                         |

# Description

Lightweight and fast MVVM framework. This framework is designed to be generic, allowing for flexibility and adaptability. However, all future implementations will be specifically tailored for WPF (Windows Presentation Foundation).


# How It Works

To ensure the correct resolution of any element, it's essential to register it. There are two primary items to be aware of when working with this system:

## ViewModel Type

The core key for registration is the ViewModel `Type`. This allows you to register payload items for the ViewModel type you're testing, whether it's of the same type or inherited from it.

## Elements for View Rendering

There are three main kinds of elements that play a role in view rendering:

1. **DataTemplate** for **DataTemplateSelector**:
   
   - **DataTemplate**: A DataTemplate is used in WPF to define the visual representation of data. It describes how data should be displayed, allowing developers to create a template that defines how data objects are rendered in UI elements like ListBox, ComboBox, and more.
     
     **Usage Example**:
     ```csharp
     service.AssociateTemplate<ShellViewModel>(dataTemplateInstance);
     ```

   - **DataTemplateSelector**: This is a class in WPF that lets you choose a DataTemplate based on custom logic. It becomes particularly useful when you have multiple templates and you want to select one based on the properties of the data object.

2. **Style** for **StyleSelector**:

   - **Style**: Styles in WPF empower developers and designers to establish a consistent appearance across their applications. By defining the look and feel of UI elements, styles help in creating visually compelling and uniform effects across the application.
     
     **Usage Example**:
     ```csharp
     service.AssociateStyle<ShellViewModel>(XamlResources.ShellWindowStyle);
     ```

   - **StyleSelector**: This class in WPF is used to apply styles based on custom logic. It determines how a style is selected for a row or an item, contingent on specific conditions.

3. **ItemContainerTemplate** for **ItemContainerTemplateSelector**:

   - **ItemContainerTemplate**: This template in WPF provides the blueprint for producing a container for an ItemsControl object. It essentially dictates how each item's container should be structured and styled within controls like ListBox or ComboBox.
     
     **Usage Example**:
     ```csharp
     service.AssociateContainer<ShellViewModel>(itemContainerTemplateInstance);
     ```

   - **ItemContainerTemplateSelector**: This selector allows for the application of custom logic to pick an ItemContainerTemplate.

## Generic Association

For a more streamlined approach, you can use a generic method to associate all the elements at once:

**Usage Example**:
```csharp
service.Associate<ShellViewModel>(
    template: dataTemplateInstance,
    style: XamlResources.ShellWindowStyle,
    container: itemContainerTemplateInstance);
```

# Core Features
- **Built-in Selectors:** The framework provides native selectors for Data, Style, and Container. These selectors facilitate efficient data binding, style management, and container operations in WPF contexts.

- **DI Framework Integration:** PS.Framework integrates seamlessly with established Dependency Injection (DI) frameworks, specifically [Autofac](https://autofac.org/) and [Microsoft dependency injection](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection). This integration allows for dependency resolution and inversion of control, leading to modular and testable code structures.

- **Scalability:** PS.Framework is scalable, catering to both small-scale utilities and large enterprise applications. It ensures consistent performance and responsiveness across varying project sizes.

- **Toolset:** The framework includes a suite of tools and services that enhance WPF development, reducing boilerplate and improving efficiency.

# MVVM Services

In essence, the **PS.MVVM** project provides tools and services to facilitate the implementation of the MVVM pattern, making it easier for developers to separate their application's logic from its presentation.

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
