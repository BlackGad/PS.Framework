# WPF MVVM Example with Custom Model Resolver Service

This project demonstrates the use of `IModelResolverService` to manage and resolve custom objects with metadata within a WPF application adhering to the MVVM pattern. The `IModelResolverService` interface and its implementations provide a way to set and retrieve custom objects and collections, with associated metadata, which can then be used within XAML.

## Project Structure

- **App.xaml / App.xaml.cs**
  - Entry point to the application.
  - Configuration and usage of `IModelResolverService`.
  
- **MainWindow.xaml**
  - Demonstrates the usage of custom model resolver in XAML.

- **IModelResolverService Definition**
  - Defines the contract for the model resolver service.

## Key Concepts

### IModelResolverService
`IModelResolverService` provides methods to manage custom objects and collections with metadata. It has two main methods:
1. `Collection(object region)` returns an observable collection for a specified region.
2. `Object(object region)` returns an observable object for a specified region.

The service allows custom objects to be added to collections with associated metadata, and these objects can be queried or filtered based on this metadata.

### Observable Models in XAML
The `{mvvm:CollectionModelResolver Region=Region}` and `{mvvm:ObjectModelResolver Region=CustomSelectedItem}` markup extensions are used within XAML to bind to the custom objects managed by `IModelResolverService`. These extensions allow for the direct use of these objects as observable items within XAML.

### Practical Usage
In real-world scenarios, sets of observable objects are often encapsulated in dedicated services, grouped logically according to their functionality. However, for fast prototyping or simpler solutions, `IModelResolverService` is very helpful as it provides a straightforward way to manage and resolve custom objects with metadata.

## Noteworthy Code Snippets

### Adding Custom Objects with Metadata in App.xaml.cs
```csharp
var modelResolverService = GetModelResolverService();

// Add objects to collection in CustomSourceItems region
modelResolverService.Collection("CustomSourceItems").Add("First item", (_, metadata) => metadata["key"] = 1);
modelResolverService.Collection("CustomSourceItems").Add("Second item");
modelResolverService.Collection("CustomSourceItems").Add("Third item", (_, metadata) => metadata["key"] = 3);

// Set item in CustomSelectedItem region
modelResolverService.Object("CustomSelectedItem").Value = "Second item";
```

### Using Custom Objects in XAML in MainWindow.xaml
```xml
<ListBox Grid.Column="0"
         ItemsSource="{mvvm:CollectionModelResolver Region=CustomSourceItems}"
         SelectedItem="{mvvm:ObjectModelResolver Region=CustomSelectedItem}" />

<TextBlock Grid.Column="1"
           HorizontalAlignment="Center"
           VerticalAlignment="Center"
           Text="{mvvm:ObjectModelResolver Region=CustomSelectedItem}" />
```

These snippets demonstrate how to add custom objects with metadata using `IModelResolverService`, and how to use these objects within XAML using the provided markup extensions.
