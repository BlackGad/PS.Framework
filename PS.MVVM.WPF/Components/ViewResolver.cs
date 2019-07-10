using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using PS.MVVM.Extensions;
using PS.MVVM.Services;
using PS.WPF.DataTemplateSelector;
using PS.WPF.StyleSelector;

namespace PS.MVVM.Components
{
    public class ViewResolver : BaseResolver<IViewResolverService>
    {
        #region Property definitions

        public static readonly DependencyProperty ServiceProperty =
            DependencyProperty.RegisterAttached("Service",
                                                typeof(IViewResolverService),
                                                typeof(ViewResolver),
                                                new PropertyMetadata(default(IViewResolverService)));

        #endregion

        #region Static members

        public static IViewResolverService GetService(DependencyObject element)
        {
            return (IViewResolverService)element.GetValue(ServiceProperty);
        }

        public static void SetService(DependencyObject element, IViewResolverService value)
        {
            element.SetValue(ServiceProperty, value);
        }

        #endregion

        #region Properties

        public Style DefaultContainerStyle { get; set; }

        public DataTemplate DefaultTemplate { get; set; }

        public object Region { get; set; }

        private IDataTemplateFactoryService DataTemplateFactoryService
        {
            get { return GlobalServices.Get<IDataTemplateFactoryService>() ?? new DataTemplateFactoryService(); }
        }

        #endregion

        #region Override members

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var provideValueTarget = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));

            var targetObject = provideValueTarget.TargetObject;

            Type propertyType = null;
            if (provideValueTarget.TargetProperty is DependencyProperty dependencyProperty) propertyType = dependencyProperty.PropertyType;
            if (provideValueTarget.TargetProperty is PropertyInfo propertyInfo) propertyType = propertyInfo.PropertyType;
            if (propertyType == null) return null;

            if (typeof(System.Windows.Controls.DataTemplateSelector).IsAssignableFrom(propertyType))
            {
                return CreateTemplateSelector(targetObject);
            }

            if (typeof(StyleSelector).IsAssignableFrom(propertyType))
            {
                return CreateContainerStyleSelector(targetObject);
            }

            if (typeof(ViewSelectors).IsAssignableFrom(propertyType))
            {
                return new ViewSelectors(CreateTemplateSelector(targetObject), CreateContainerStyleSelector(targetObject));
            }

            var message = $"Invalid target property type. {nameof(System.Windows.Controls.DataTemplateSelector)}, " +
                          $"{nameof(StyleSelector)} or " +
                          $"{nameof(ViewSelectors)} expected";

            throw new ArgumentException(message);
        }

        #endregion

        #region Members

        private StyleSelector CreateContainerStyleSelector(object targetObject)
        {
            return new RelayStyleSelector((item, container) =>
            {
                if (item == null) return DefaultContainerStyle;

                var viewModelType = item as Type;
                viewModelType = viewModelType ?? item.GetType();

                var viewRegistryService = GetService(targetObject, ServiceProperty);
                if (viewRegistryService == null)
                {
                    var message = FormatServiceErrorMessage(ServiceProperty);
                    throw new ArgumentException(message);
                }

                var association = viewRegistryService.FindAssociation(viewModelType, Region);
                if (association == null) return DefaultContainerStyle;

                return association.GetContainerStyle() ?? DefaultContainerStyle;
            });
        }

        private RelayTemplateSelector CreateTemplateSelector(object targetObject)
        {
            return new RelayTemplateSelector((item, container) =>
            {
                if (item == null) return DefaultTemplate;

                var viewModelType = item as Type;
                viewModelType = viewModelType ?? item.GetType();

                var viewRegistryService = GetService(targetObject, ServiceProperty);
                if (viewRegistryService == null)
                {
                    var message = FormatServiceErrorMessage(ServiceProperty);
                    throw new ArgumentException(message);
                }

                var association = viewRegistryService.FindAssociation(viewModelType, Region);
                if (association == null) return DefaultTemplate;

                return DataTemplateFactoryService.ResolveDataTemplate(item, association);
            });
        }

        #endregion
    }
}