using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xaml;
using PS.Extensions;
using PS.MVVM.Services;

namespace PS.MVVM.Components.ModelResolver
{
    public abstract class ModelResolver : BaseResolver<IModelResolverService>,
                                          INotifyPropertyChanged
    {
        public static readonly DependencyProperty ServiceProperty =
            DependencyProperty.RegisterAttached("Service",
                                                typeof(IModelResolverService),
                                                typeof(ModelResolver),
                                                new PropertyMetadata(default(IModelResolverService)));

        private static readonly MethodInfo CreateBindingExpressionMethod;

        public static IModelResolverService GetService(DependencyObject element)
        {
            return (IModelResolverService)element.GetValue(ServiceProperty);
        }

        public static void SetService(DependencyObject element, IModelResolverService value)
        {
            element.SetValue(ServiceProperty, value);
        }

        private object _model;

        static ModelResolver()
        {
            CreateBindingExpressionMethod = typeof(BindingExpression).GetMethod("CreateBindingExpression", BindingFlags.NonPublic | BindingFlags.Static);
        }

        public IValueConverter Converter { get; set; }

        public object ConverterParameter { get; set; }

        public object Model
        {
            get { return _model; }
            set
            {
                if (_model.AreEqual(value)) return;
                _model = value;
                OnPropertyChanged();
            }
        }

        public object Region { get; set; }

        public sealed override object ProvideValue(IServiceProvider serviceProvider)
        {
            var rootObjectProvider = (IRootObjectProvider)serviceProvider.GetService(typeof(IRootObjectProvider));
            var provideValueTarget = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));

            var targetObject = provideValueTarget.TargetObject;
            var targetProperty = provideValueTarget.TargetProperty;

            var rootFrameworkElement = rootObjectProvider.RootObject as FrameworkElement;
            if (rootFrameworkElement == null) return null;

            var connectWithBinding = targetObject is DependencyObject && targetProperty is DependencyProperty;
            if (!connectWithBinding && !(targetProperty is PropertyInfo))
            {
                throw new NotSupportedException("Target property must be PropertyInfo or DependencyProperty");
            }

            void ElementOnLoaded(object sender, RoutedEventArgs args)
            {
                rootFrameworkElement.Loaded -= ElementOnLoaded;

                var modelResolverService = GetService(targetObject, ServiceProperty);
                if (modelResolverService == null)
                {
                    var message = FormatServiceErrorMessage(ServiceProperty);
                    throw new ArgumentException(message);
                }

                var observableModel = ProvideObservableModel(modelResolverService);
                if (connectWithBinding)
                {
                    Model = observableModel;
                    return;
                }

                if (targetProperty is PropertyInfo propertyInfo)
                {
                    if (observableModel is IObservableModelCollection) propertyInfo.SetValue(targetObject, observableModel);
                    if (observableModel is IObservableModelObject observableModelObject)
                    {
                        observableModelObject.ValueChanged += (o, eventArgs) => propertyInfo.SetValue(targetObject, eventArgs.NewValue);
                    }
                }
            }

            rootFrameworkElement.Loaded += ElementOnLoaded;

            if (connectWithBinding)
            {
                var binding = ProvideBinding(((DependencyProperty)targetProperty).ReadOnly);
                return CreateBindingExpressionMethod.Invoke(null, new[] { targetObject, targetProperty, binding, null });
            }

            return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected abstract Binding ProvideBinding(bool isReadOnly);

        protected abstract object ProvideObservableModel(IModelResolverService modelResolverService);
    }
}
