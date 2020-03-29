using System;
using System.Windows;

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

        #endregion
    }
}