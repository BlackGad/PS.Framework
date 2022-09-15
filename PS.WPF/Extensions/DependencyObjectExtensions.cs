using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace PS.WPF.Extensions
{
    public static class DependencyObjectExtensions
    {
        private static readonly ResourceReferenceExpressionConverter ResourceReferenceExpressionConverter;

        public static void CopySimilarValuesTo(this DependencyObject source, DependencyObject target, params DependencyProperty[] properties)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (target == null) throw new ArgumentNullException(nameof(target));
            foreach (var property in properties)
            {
                var value = target.GetValue(property);
                source.SetValue(property, value);
            }
        }

        public static bool IsDefaultValue(this DependencyObject source, DependencyProperty property)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (property == null) throw new ArgumentNullException(nameof(property));
            return source.ReadLocalValue(property) == DependencyProperty.UnsetValue;
        }

        public static void Override(this DependencyProperty property,
                                    Type elementType,
                                    Action<DependencyObject, DependencyPropertyChangedEventArgs, PropertyChangedCallback> changed = null,
                                    Func<DependencyObject, object, CoerceValueCallback, object> coerce = null,
                                    Action<PropertyMetadata, PropertyMetadata> factory = null)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            if (elementType == null) throw new ArgumentNullException(nameof(elementType));

            var oldPropertyMetadata = property.GetMetadata(elementType);
            var propertyMetadataType = oldPropertyMetadata.GetType();

            var newPropertyMetadata = (PropertyMetadata)Activator.CreateInstance(propertyMetadataType);
            newPropertyMetadata.DefaultValue = oldPropertyMetadata.DefaultValue;
            if (changed != null) newPropertyMetadata.PropertyChangedCallback = (o, args) => changed(o, args, oldPropertyMetadata.PropertyChangedCallback);
            if (coerce != null) newPropertyMetadata.CoerceValueCallback = (o, args) => coerce(o, args, oldPropertyMetadata.CoerceValueCallback);

            if (oldPropertyMetadata is UIPropertyMetadata oldUIPropertyMetadata &&
                newPropertyMetadata is UIPropertyMetadata newUIPropertyMetadata)
            {
                newUIPropertyMetadata.IsAnimationProhibited = oldUIPropertyMetadata.IsAnimationProhibited;
            }

            if (oldPropertyMetadata is FrameworkPropertyMetadata oldFrameworkPropertyMetadata &&
                newPropertyMetadata is FrameworkPropertyMetadata newFrameworkPropertyMetadata)
            {
                newFrameworkPropertyMetadata.AffectsArrange = oldFrameworkPropertyMetadata.AffectsArrange;
                newFrameworkPropertyMetadata.AffectsMeasure = oldFrameworkPropertyMetadata.AffectsMeasure;
                newFrameworkPropertyMetadata.AffectsParentArrange = oldFrameworkPropertyMetadata.AffectsParentArrange;
                newFrameworkPropertyMetadata.AffectsParentMeasure = oldFrameworkPropertyMetadata.AffectsParentMeasure;
                newFrameworkPropertyMetadata.AffectsRender = oldFrameworkPropertyMetadata.AffectsRender;
                newFrameworkPropertyMetadata.BindsTwoWayByDefault = oldFrameworkPropertyMetadata.BindsTwoWayByDefault;
                newFrameworkPropertyMetadata.DefaultUpdateSourceTrigger = oldFrameworkPropertyMetadata.DefaultUpdateSourceTrigger;
                newFrameworkPropertyMetadata.Inherits = oldFrameworkPropertyMetadata.Inherits;
                newFrameworkPropertyMetadata.IsNotDataBindable = oldFrameworkPropertyMetadata.IsNotDataBindable;
                newFrameworkPropertyMetadata.Journal = oldFrameworkPropertyMetadata.Journal;
                newFrameworkPropertyMetadata.SubPropertiesDoNotAffectRender = oldFrameworkPropertyMetadata.SubPropertiesDoNotAffectRender;
                newFrameworkPropertyMetadata.OverridesInheritanceBehavior = oldFrameworkPropertyMetadata.OverridesInheritanceBehavior;
            }

            factory?.Invoke(newPropertyMetadata, oldPropertyMetadata);

            property.OverrideMetadata(elementType, newPropertyMetadata);
        }

        public static void SetBindingIfDefault(this DependencyObject source, DependencyProperty property, Binding binding)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (property == null) throw new ArgumentNullException(nameof(property));
            if (binding == null) throw new ArgumentNullException(nameof(binding));
            if (source.IsDefaultValue(property)) BindingOperations.SetBinding(source, property, binding);
        }

        public static void SetBindingToAncestorIfDefault(this DependencyObject source, DependencyProperty property)
        {
            if (!source.IsDefaultValue(property)) return;

            var keyColumnWidthPropertySource = DependencyPropertyHelper.GetValueSource(source, property);
            if (keyColumnWidthPropertySource.BaseValueSource > BaseValueSource.Style) return;
            if (keyColumnWidthPropertySource.IsExpression) return;

            var result = source.TraverseVisualParentWhere(o => o.IsDefaultValue(property));
            if (result == null) return;

            BindingOperations.SetBinding(source,
                                         property,
                                         new Binding
                                         {
                                             Source = result,
                                             Path = new PropertyPath(property),
                                             Mode = BindingMode.TwoWay
                                         });
        }

        public static void SetValueIfDefault(this DependencyObject source, DependencyProperty property, object value)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (property == null) throw new ArgumentNullException(nameof(property));
            if (source.IsDefaultValue(property)) source.SetValue(property, value);
        }

        public static void TransferPropertyTo(this DependencyObject original,
                                              DependencyObject clone,
                                              DependencyProperty originalProperty,
                                              DependencyProperty cloneProperty = null,
                                              bool templateBoundOnly = false)
        {
            if (original == null) throw new ArgumentNullException(nameof(original));
            if (clone == null) throw new ArgumentNullException(nameof(clone));
            if (originalProperty == null) throw new ArgumentNullException(nameof(originalProperty));

            cloneProperty = cloneProperty ?? originalProperty;

            var originalValueSource = DependencyPropertyHelper.GetValueSource(original, originalProperty).BaseValueSource;
            var performTransfer = templateBoundOnly ? originalValueSource >= BaseValueSource.ParentTemplate : originalValueSource > BaseValueSource.Default;
            if (!performTransfer) return;

            var binding = BindingOperations.GetBindingBase(original, originalProperty);
            if (binding != null)
            {
                // Transfer Bindings
                BindingOperations.SetBinding(clone, cloneProperty, binding);
            }
            else
            {
                if (original.ReadLocalValue(originalProperty) is Expression expression)
                {
                    // Transfer DynamicResource
                    if (ResourceReferenceExpressionConverter.ConvertTo(expression, typeof(MarkupExtension)) is DynamicResourceExtension dynamicResource)
                    {
                        clone.SetValue(cloneProperty, dynamicResource.ProvideValue(null));
                    }
                }
                else
                {
                    // Transfer other DPs
                    var originalValue = original.GetValue(originalProperty);
                    clone.SetValue(cloneProperty, originalValue);
                }
            }
        }

        static DependencyObjectExtensions()
        {
            ResourceReferenceExpressionConverter = new ResourceReferenceExpressionConverter();
        }
    }
}
