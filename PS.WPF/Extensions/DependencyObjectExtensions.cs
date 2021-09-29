using System;
using System.Windows;
using System.Windows.Data;

namespace PS.WPF.Extensions
{
    public static class DependencyObjectExtensions
    {
        #region Static members

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

        #endregion
    }
}