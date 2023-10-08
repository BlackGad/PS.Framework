# WPF Application with Dependency Injection and MVVM

This document explains the structure and design of a WPF application employing the MVVM (Model-View-ViewModel) pattern, Dependency Injection (DI) via Autofac, and various services to manage views, models, and regions.

## Project Structure
The project is structured into several namespaces, each encapsulating a logical grouping of classes and functionalities.

- **App.Infrastructure**
  - Holds constants for region identifiers used across the application.

- **App.Module.Custom and App.Module.Main**
  - Contain custom and main modules respectively, which configure the DI container with necessary registrations.

- **App.Shell**
  - Contains the entry point (`CustomApplication` class) and `Bootstrapper` class for initializing the DI container.

- **App.Shell.Models**
  - Contains services for resolving models, views, and window services.

- **ViewModels and Views**
  - Represent the MVVM pattern, with various ViewModels and Views defined in `App.Module.Custom.ViewModels`, `App.Module.Custom.Views`, `App.Module.Main.ViewModels`, and `App.Module.Main.Views`.

## Key Components and Concepts

### Regions
Regions are defined in `Regions.cs` under `App.Infrastructure`, providing constant identifiers to help manage different parts of the UI.

### Dependency Injection with Autofac
Autofac is used to manage dependencies throughout the application. The `CustomModule` and `MainModule` classes in `App.Module.Custom` and `App.Module.Main` respectively, override Autofac's `Module` class to configure the DI container. These modules are registered in the `Bootstrapper` class in `App.Shell`.

### Model and View Resolver Services
`ModelResolverService` and `ViewResolverService` in `App.Shell.Models` are implementations of `IModelResolverService` and `IViewResolverService` interfaces respectively. They provide functionalities to resolve models and views, which are utilized in various parts of the application, including in the `CustomModule` and `MainModule` classes for associating templates with ViewModels.

### Window Service
`WindowService` in `App.Shell.Models` is an implementation of `IWindowService` interface, which is utilized in `CustomApplication` class in `App.Shell` to display windows (including the main shell of the application).

### IoC Data Template
`IoCDataTemplate` in `App.Shell.Models` is a class that implements `IDataTemplate` interface, providing a way to associate a View with a ViewModel using Autofac to resolve the View.

### XAML Resources
`XamlResources.xaml` and `XamlResources.xaml.cs` in `App.Module.Main` define a style for the main window and provide a `ResourceDescriptor` for accessing this style.

### Application Initialization
`CustomApplication` class in `App.Shell` is the entry point to the application. It creates an instance of `Bootstrapper`, initializes it, and displays the main shell of the application.

### Shell
`ShellView.xaml` in `App.Module.Main.Views` defines the main shell of the application, utilizing the MVVM pattern, and the `ModelResolverService` and `ViewResolverService` to bind and display items in a `ListBox` and a `ContentControl`.

## Noteworthy Code Snippets

```csharp
// Dependency Registrations in CustomModule.cs and MainModule.cs

// CustomModule.cs
service.Collection(Regions.SHELL_ITEMS).Add(scope.Resolve<Item2ViewModel>());
service.AssociateTemplate<Item2ViewModel>(scope.Resolve<IDataTemplate<Item2View>>());

// MainModule.cs
service.Object(Regions.SHELL_LAYOUT).Value = scope.Resolve<ShellViewModel>();
service.AssociateTemplate<Item1ViewModel>(scope.Resolve<IDataTemplate<Item1View>>());

// View and Style Associations in MainModule.cs
// MainModule.cs
service.Associate<ShellViewModel>(
    template: scope.Resolve<IDataTemplate<ShellView>>(),
    style: XamlResources.MainWindowStyle);

// Main Window Display in App.xaml.cs
// App.xaml.cs
private void ShowShell()
{
    // ...
    windowService.Show(modelResolverService.Object(Regions.SHELL_LAYOUT).Value);
    // ...
}

// Usage of `{mvvm:TemplateResolver}` in ShellView.xaml
// ShellView.xaml
<ListBox x:Name="ListBox"
         ItemTemplateSelector="{mvvm:TemplateResolver Region={x:Static infrastructure:Regions.SHELL_ITEMS_VIEW}}"
         ItemsSource="{mvvm:CollectionModelResolver Region={x:Static infrastructure:Regions.SHELL_ITEMS}}" />
```

These snippets exemplify how dependency registrations are done, how views and styles are associated with view models, how the main window is displayed, and how `{mvvm:TemplateResolver}` is used for template selection based on regions.