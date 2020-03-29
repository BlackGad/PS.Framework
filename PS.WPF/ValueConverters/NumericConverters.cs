using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using PS.Extensions;

namespace PS.WPF.ValueConverters
{
    public static class NumericConverters
    {
        #region Constants

        public static readonly RelayValueConverter Abs;
        public static readonly RelayValueConverter Add;
        public static readonly RelayValueConverter Invert;
        public static readonly RelayValueConverter Subtract;
        public static readonly RelayValueConverter ToGridLength;

        #endregion

        #region Static members

        private static object AbsFunc(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueType = value?.GetType();
            if (valueType == null) return null;

            return valueType.IsNumeric() ? Math.Abs((dynamic)value) : value;
        }

        private static object AddFunc(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueType = value?.GetType();
            if (valueType == null) return null;

            var parameterType = parameter?.GetType();
            if (parameterType == null) return value;

            return valueType.IsNumeric() ? -1 * (dynamic)value : value;
        }

        private static object InvertFunc(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueType = value?.GetType();
            if (valueType == null) return null;

            return valueType.IsNumeric() ? -1 * (dynamic)value : value;
        }

        private static object SubtractFunc(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueType = value?.GetType();
            if (valueType == null || !valueType.IsNumeric()) return value;

            var parameterType = parameter?.GetType();
            if (parameterType == null || !parameterType.IsNumeric()) return value;

            return (dynamic)value - (dynamic)parameter;
        }

        #endregion

        #region Constructors

        static NumericConverters()
        {
            Invert = new RelayValueConverter(InvertFunc, InvertFunc);
            Abs = new RelayValueConverter(AbsFunc, AbsFunc);

            Add = new RelayValueConverter(AddFunc, SubtractFunc);
            Subtract = new RelayValueConverter(SubtractFunc, AddFunc);

            ToGridLength = new RelayValueConverter((value, targetType, parameter, culture) =>
                                                   {
                                                       if (value == null) return GridLength.Auto;

                                                       var converter = TypeDescriptor.GetConverter(typeof(GridLength));
                                                       if (converter.CanConvertFrom(value.GetType())) return converter.ConvertFrom(value);

                                                       throw new NotSupportedException();
                                                   },
                                                   (value, targetType, parameter, culture) =>
                                                   {
                                                       if (value == null) return GridLength.Auto;

                                                       var converter = TypeDescriptor.GetConverter(typeof(GridLength));
                                                       if (converter.CanConvertFrom(value.GetType())) return converter.ConvertFrom(value);

                                                       throw new NotSupportedException();
                                                   });
        }

        #endregion
    }
}