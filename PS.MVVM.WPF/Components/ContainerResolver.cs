using System;
using System.Windows.Controls;
using PS.WPF.ItemContainerTemplateSelector;
using PS.WPF.Resources;

namespace PS.MVVM.Components;

public class ContainerResolver : BaseViewResolver
{
    public ItemContainerTemplate Default { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var result = base.ProvideValue(serviceProvider);
        if (result != null) return result;

        var message = $"Invalid target property type. {nameof(ItemContainerTemplate)} expected";
        throw new ArgumentException(message);
    }

    protected override object CreateResult(Type propertyType, object targetObject)
    {
        if (typeof(ItemContainerTemplateSelector).IsAssignableFrom(propertyType))
        {
            return new RelayItemContainerTemplateSelector((item, container) =>
            {
                if (item == null) return Default;

                var viewModelType = item as Type;
                viewModelType = viewModelType ?? item.GetType();

                var viewRegistryService = GetService(targetObject, ServiceProperty);
                if (viewRegistryService == null)
                {
                    var message = FormatServiceErrorMessage(ServiceProperty);
                    throw new ArgumentException(message);
                }

                var association = viewRegistryService.Find(typeof(ContainerResolver), viewModelType, Region);
                if (association == null) return Default;

                if (association.Payload is ItemContainerTemplate itemContainerTemplate) return itemContainerTemplate;
                if (association.Payload is ResourceDescriptor descriptor) return descriptor.GetResource<ItemContainerTemplate>();

                return Default;
            });
        }

        return null;
    }
}
