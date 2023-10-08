# Template, Style and Container resolvers example

This example demonstrates the application of `DataTemplate`, `Style`, and `ItemContainerTemplate` to ViewModel items within a WPF application. All definitions are conveniently housed within `App.xaml` for simplification.

## Key Points

1. **Registration of Templates, Styles, and Containers:**  
   In `App.xaml.cs`, we register data templates, styles, and item container templates to respective ViewModel types using `IViewResolverService`. This is carried out in the `OnStartup` method.

    ```csharp
    protected override void OnStartup(StartupEventArgs e)
    {
        var viewResolverService = GetViewResolverService();

        // Register views in default region
        viewResolverService.AssociateTemplate<Item1ViewModel>((DataTemplate)Resources["Item1DataTemplate"]);
        viewResolverService.AssociateStyle<Item2ViewModel>((Style)Resources["Item2Style"]);
        viewResolverService.AssociateContainer<Item3ViewModel>((ItemContainerTemplate)Resources["Item3ContainerTemplate"]);

        base.OnStartup(e);
    }
    ```

2. **ViewModels Association:**  
   ViewModel instances are created and associated to the menu within `App.xaml`.

    ```xml
    <x:Array x:Key="Items" Type="{x:Type system:Object}">
        <viewModels:Item1ViewModel />
        <viewModels:Item2ViewModel />
        <viewModels:Item3ViewModel />
    </x:Array>
    ```

3. **Template, Style, and Container Definitions:**  
   Definitions for `DataTemplate`, `Style`, and `ItemContainerTemplate` are done in `App.xaml`. Each template or style is associated with a ViewModel type.

    ```xml
    <!--  Define Item1ViewModel data template  -->
    <DataTemplate x:Key="Item1DataTemplate" DataType="{x:Type viewModels:Item1ViewModel}">
        <TextBlock Text="{Binding Path=Title}" />
    </DataTemplate>

    <!--  Define Item2ViewModel style  -->
    <Style x:Key="Item2Style" TargetType="MenuItem">
        <Setter Property="FontWeight" Value="Bold" />
        <Setter Property="Header" Value="{Binding Path=Title}" />
    </Style>

    <!--  Define Item3ViewModel container template  -->
    <ItemContainerTemplate x:Key="Item3ContainerTemplate">
        <RibbonMenuItem Header="{Binding Title}" />
    </ItemContainerTemplate>
    ```

4. **Menu Configuration:**  
   The `Menu` control in `MainWindow.xaml` is configured to use the defined templates, styles, and containers via `ItemTemplateSelector`, `ItemContainerStyleSelector`, and `ItemContainerTemplateSelector` respectively.

    ```xml
    <Menu HorizontalAlignment="Center"
          VerticalAlignment="Center"
          Background="LightBlue"
          ItemContainerStyleSelector="{mvvm:StyleResolver}"
          ItemContainerTemplateSelector="{mvvm:ContainerResolver}"
          ItemTemplateSelector="{mvvm:TemplateResolver}"
          ItemsSource="{StaticResource Items}"
          UsesItemContainerTemplate="True">
    </Menu>
    ```

These configurations demonstrate a clean separation of concerns between the view and the ViewModel, aligning with the MVVM architectural pattern, and simplifying the management of view-related definitions.
