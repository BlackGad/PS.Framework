using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;
using PS.Extensions;

namespace PS.WPF.ValueConverters
{
    [ContentProperty(nameof(ConvertSequence))]
    public class CascadeValueConverter : IValueConverter
    {
        public CascadeValueConverter()
        {
            ConvertBackSequence = new ObservableCollection<IValueConverter>();
            ConvertSequence = new ObservableCollection<IValueConverter>();
        }

        public ObservableCollection<IValueConverter> ConvertBackSequence { get; set; }

        public ObservableCollection<IValueConverter> ConvertSequence { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConvertSequence.Enumerate().Any()) throw new NotSupportedException();

            var result = value;
            foreach (var converter in ConvertSequence)
            {
                result = converter.Convert(result, targetType, parameter, culture);
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConvertBackSequence.Enumerate().Any()) throw new NotSupportedException();

            var result = value;
            foreach (var converter in ConvertBackSequence)
            {
                result = converter.ConvertBack(result, targetType, parameter, culture);
            }

            return result;
        }
    }
}
