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
    public class CascadeMultiValueConverter : IMultiValueConverter
    {
        public CascadeMultiValueConverter()
        {
            ConvertBackSequence = new ObservableCollection<IValueConverter>();
            ConvertSequence = new ObservableCollection<IValueConverter>();
        }

        public IMultiValueConverter ConvertBackFinishConverter { get; set; }

        public ObservableCollection<IValueConverter> ConvertBackSequence { get; set; }

        public ObservableCollection<IValueConverter> ConvertSequence { get; set; }

        public IMultiValueConverter ConvertStartConverter { get; set; }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (ConvertStartConverter == null) throw new NotSupportedException();
            if (!ConvertSequence.Enumerate().Any()) throw new NotSupportedException();

            var result = ConvertStartConverter.Convert(values, targetType, parameter, culture);
            foreach (var converter in ConvertSequence)
            {
                result = converter.Convert(result, targetType, parameter, culture);
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (ConvertBackFinishConverter == null) throw new NotSupportedException();
            if (!ConvertBackSequence.Enumerate().Any()) throw new NotSupportedException();

            var result = value;
            foreach (var converter in ConvertBackSequence)
            {
                result = converter.ConvertBack(result, targetTypes.FirstOrDefault(), parameter, culture);
            }

            return ConvertBackFinishConverter.ConvertBack(result, targetTypes, parameter, culture);
        }
    }
}
