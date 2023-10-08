# MVVM Example with Dynamic and Static Data Templates

This example demonstrates how to propagate `IViewResolverService` service within a WPF application using either the application's resources or a variation of the service locator pattern. It also showcases how to register dynamic and static data templates associated with ViewModel types, and how to utilize the `{mvvm:TemplateResolver}` XAML markup extension for resolving data templates.

## Project Structure

- **App.xaml.cs**
  - Contains the consolidation of `IViewResolverService` instance and registration of data templates.
  
- **App.xaml**
  - Hosts ViewModel instantiation and definition of a predefined generic data template.
  
- **MainWindow.xaml**
  - Defines the main XAML layout containing a ListBox and container controls.
  
- **/ViewModels**
  - Contains definition of ViewModel types demonstrating inheritance capability from any base type.
  
- **/Views**
  - Hosts XAML layouts for dynamic data templates, showcasing `ViewDataTemplate` type usage.
  
- **/Views/Item2View.xaml**
  - Root element is `Grid` illustrating XAML tree reduction and ability to inherit any `FrameworkElement` as root.

## Noteworthy Code Snippets

### Dynamic Data Template Creation in App.xaml.cs
```csharp
private DataTemplate CreateDynamicDataTemplateForType<T>()
{
    // Demonstrates how to create DataTemplate for view model type using custom ViewDataTemplate from PS.WPF
    return new ViewDataTemplate
    {
        ViewType = typeof(T)
    };
}
```

### IViewResolverService Instance Consolidation and Data Template Registration in App.xaml.cs
```csharp
protected override void OnStartup(StartupEventArgs e)
{
    var viewResolverService = GetViewResolverService();

    // Register views in default region
    viewResolverService.AssociateTemplate<Item1ViewModel>(CreateDynamicDataTemplateForType<Item1View>());
    viewResolverService.AssociateTemplate<Item2ViewModel>(CreateDynamicDataTemplateForType<Item2View>());
    viewResolverService.AssociateTemplate<Item3ViewModel>((DataTemplate)Resources["Item3ViewDataTemplate"]);

    base.OnStartup(e);
}
```

### Template Resolution Usage in MainWindow.xaml
```xaml
<ContentControl Grid.Column="1"
                ContentTemplateSelector="{mvvm:TemplateResolver}"
                Content="{Binding ElementName=ListBox, Path=SelectedItem}" />
```