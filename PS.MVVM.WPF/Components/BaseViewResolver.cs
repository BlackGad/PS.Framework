using System;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using PS.MVVM.Services;
using PS.WPF.ValueConverters;

namespace PS.MVVM.Components
{
    public abstract class BaseViewResolver : BaseResolver<IViewResolverService>
    {
        #region Property definitions

        public static readonly DependencyProperty ServiceProperty =
            DependencyProperty.RegisterAttached("Service",
                                                typeof(IViewResolverService),
                                                typeof(BaseViewResolver),
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

        public object Region { get; set; }

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

            var result = CreateResult(propertyType, targetObject);
            if (result != null) return result;

            if (targetObject is Setter setter)
            {
                ResolverSource = ResolverServiceSource.Global;
                return new Binding
                {
                    Converter = new RelayValueConverter((value, targetType, parameter, culture) => CreateResult(setter.Property.PropertyType, value))
                };
            }

            return null;
        }

        #endregion

        #region Members

        protected abstract object CreateResult(Type propertyType, object targetObject);

        #endregion
    }
}