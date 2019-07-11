using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PS.WPF.ValueConverters
{
    public class TranslatePointConverter : IValueConverter
    {
        #region Properties

        public FrameworkElement Element { get; set; }
        public FrameworkElement RelativeTo { get; set; }

        #endregion

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Point point)
            {
                return Element?.TranslatePoint(point, RelativeTo);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Point point)
            {
                return RelativeTo?.TranslatePoint(point, Element);
            }

            return value;
        }

        #endregion
    }
}