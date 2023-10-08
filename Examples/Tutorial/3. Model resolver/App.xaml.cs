using System;
using System.Windows;
using PS.MVVM.Services;

namespace Example;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override void OnStartup(StartupEventArgs e)
    {
        var modelResolverService = GetModelResolverService();

        // Add objects to collection in CustomSourceItems region
        modelResolverService.Collection("CustomSourceItems").Add("First item", (_, metadata) => metadata["key"] = 1);
        modelResolverService.Collection("CustomSourceItems").Add("Second item");
        modelResolverService.Collection("CustomSourceItems").Add("Third item", (_, metadata) => metadata["key"] = 3);

        // Demonstration of collection filtering by metadata
        var filteredItemsByMetadata = modelResolverService.Collection("CustomSourceItems").Query((item, metadata) => metadata.ContainsKey("key"));
        foreach (var item in filteredItemsByMetadata)
        {
            Console.WriteLine(item);
        }

        // Set item in CustomSelectedItem region
        modelResolverService.Object("CustomSelectedItem").Value = "Second item";

        base.OnStartup(e);
    }

    private IModelResolverService GetModelResolverService()
    {
        var modelResolverService = new ModelResolverService();
        Resources[typeof(IModelResolverService)] = modelResolverService;
        return modelResolverService;
    }
}
