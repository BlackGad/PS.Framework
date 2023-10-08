# WPF MVVM Example with Dependency Injection

This project is a WPF application demonstrating the MVVM (Model-View-ViewModel) pattern, utilizing Autofac for dependency injection and `PS.MVVM` framework for MVVM infrastructure. The example showcases how to define, register, and resolve ViewModel and View associations using `IViewResolverService`, and how to manage application styles.

## Project Structure

- **App.xaml / App.xaml.cs**
  - Entry point to the application.
  - Registration of Views, ViewModels, and Styles.
  
- **MainView.xaml**
  - Main window layout with ListBox and ContentControl to display selected item details.
  
- **Item1View.xaml / Item2View.xaml**
  - Different item views illustrating how to bind ViewModel properties to View controls.
  
- **ViewModels**
  - Contains ViewModel classes: `MainViewModel`, `Item1ViewModel`, `Item2ViewModel`.
  
- **Models**
  - Contains service classes: `ModelsService`, `WindowService`, `IoCDataTemplate`, `ViewResolverService`.
  
- **Views**
  - Contains View classes: `MainView`, `Item1View`, `Item2View`.

## Key Concepts

### Dependency Injection
The `IoCDataTemplate` class provides a custom implementation for data templates, allowing dependencies to be injected into Views, ViewModels, and models. This is facilitated by the Autofac library which is used to register and resolve dependencies throughout the application.

### MVVM Pattern Unification
The process of displaying the main window is slightly more complex than using the `StartupUri` property in `App.xaml`. However, this approach allows the MVVM pattern to be applied uniformly to windows and dialogs. By managing the main window display programmatically in `App.xaml.cs`, it's easier to adhere to the MVVM pattern, ensuring a clean separation of concerns and a unified approach to managing windows and dialogs within the application.

## Noteworthy Code Snippets

### Dependency Registrations in App.xaml.cs
```csharp
var builder = new ContainerBuilder();
builder.RegisterAssemblyTypesWithAttributes(typeof(App).Assembly);
var container = builder.Build();
var viewResolverService = container.Resolve<IViewResolverService>();
Resources[typeof(IViewResolverService)] = viewResolverService;
```

### View and Style Associations in App.xaml.cs
```csharp
viewResolverService.AssociateTemplate<Item1ViewModel>(container.Resolve<IDataTemplate<Item1View>>());
viewResolverService.AssociateTemplate<Item2ViewModel>(container.Resolve<IDataTemplate<Item2View>>());
viewResolverService.AssociateTemplate<MainViewModel>(container.Resolve<IDataTemplate<MainView>>());
viewResolverService.AssociateStyle<MainViewModel>((Style)Resources["MainWindowStyle"]);
```

### Main Window Display in App.xaml.cs
```csharp
var windowService = container.Resolve<IWindowService>();
windowService.Show<MainViewModel>();
```

### Template Resolution Usage in MainView.xaml
```xml
<ContentControl Grid.Column="1"
                ContentTemplateSelector="{mvvm:TemplateResolver}"
                Content="{Binding ElementName=ListBox, Path=SelectedItem}" />
```

These snippets demonstrate key parts of the codebase, showcasing how dependencies are registered and resolved, how Views and ViewModels are associated, how the main window is displayed, and how the `{mvvm:TemplateResolver}` is used to dynamically resolve data templates based on the selected item in `MainView.xaml`.
