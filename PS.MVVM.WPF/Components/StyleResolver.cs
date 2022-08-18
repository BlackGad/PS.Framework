using System;
using System.Windows;
using System.Windows.Controls;
using PS.WPF.Resources;
using PS.WPF.StyleSelector;

namespace PS.MVVM.Components;

public class StyleResolver : BaseViewResolver
{
    public Style Default { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var result = base.ProvideValue(serviceProvider);
        if (result != null) return result;

        var message = $"Invalid target property type. {nameof(StyleSelector)} expected";
        throw new ArgumentException(message);
    }

    protected override object CreateResult(Type propertyType, object targetObject)
    {
        if (typeof(StyleSelector).IsAssignableFrom(propertyType))
        {
            return new RelayStyleSelector((item, container) =>
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

                var association = viewRegistryService.Find(typeof(StyleResolver), viewModelType, Region);
                if (association == null) return Default;

                if (association.Payload is Style style) return style;
                if (association.Payload is ResourceDescriptor descriptor) return descriptor.GetResource<Style>();

                return Default;
            });
        }

        return null;
    }
}
