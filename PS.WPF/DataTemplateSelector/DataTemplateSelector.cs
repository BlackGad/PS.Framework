using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using PS.Extensions;
using PS.WPF.DataTemplateSelector.Rules;

namespace PS.WPF.DataTemplateSelector
{
    [ContentProperty("Rules")]
    public class DataTemplateSelector : System.Windows.Controls.DataTemplateSelector,
                                        IValueConverter
    {
        public DataTemplateSelector()
        {
            Rules = new ObservableCollection<SelectRule>();
        }

        public System.Windows.DataTemplate DefaultTemplate { get; set; }

        public ObservableCollection<SelectRule> Rules { get; set; }

        public string ValuePath { get; set; }

        public override System.Windows.DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null || Rules == null) return DefaultTemplate;
            item = item.GetEffectiveValue(ValuePath);
            var template = Rules.FirstOrDefault(t => t.IsValid(item));
            return template != null ? template.Template : DefaultTemplate;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return SelectTemplate(value, null);
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
