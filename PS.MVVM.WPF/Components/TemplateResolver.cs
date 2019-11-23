using System;
using System.Windows;
using PS.WPF.DataTemplateSelector;
using PS.WPF.Resources;
using DataTemplateSelector = System.Windows.Controls.DataTemplateSelector;

namespace PS.MVVM.Components
{
    public class TemplateResolver : BaseViewResolver
    {
        #region Properties

        public DataTemplate Default { get; set; }

        #endregion

        #region Override members

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var result = base.ProvideValue(serviceProvider);
            if (result != null) return result;

            var message = $"Invalid target property type. {nameof(DataTemplateSelector)} expected";
            throw new ArgumentException(message);
        }

        protected override object CreateResult(Type propertyType, object targetObject)
        {
            if (typeof(DataTemplateSelector).IsAssignableFrom(propertyType))
            {
                return new RelayTemplateSelector((item, container) =>
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

                    var association = viewRegistryService.Find(typeof(TemplateResolver), viewModelType, Region);
                    if (association == null) return Default;

                    if (association.Payload is DataTemplate dataTemplate) return dataTemplate;
                    if (association.Payload is ResourceDescriptor descriptor) return descriptor.GetResource<DataTemplate>();

                    return Default;
                });
            }

            return null;
        }

        #endregion
    }
}