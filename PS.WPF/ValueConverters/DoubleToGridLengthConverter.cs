using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PS.WPF.ValueConverters
{
    public class DoubleToGridLengthConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d) return new GridLength(d);
            return GridLength.Auto;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is GridLength gridLength) return gridLength.Value;
            return 0;
        }

        #endregion
    }
}