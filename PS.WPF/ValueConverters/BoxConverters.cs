using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using PS.Extensions;

namespace PS.WPF.ValueConverters
{
    public static class BoxConverters
    {
        public static readonly RelayValueConverter DecimalToString;
        public static readonly RelayValueConverter DoubleToGridLength;
        public static readonly RelayValueConverter Int32ToString;
        public static readonly RelayValueConverter StringToDecimal;
        public static readonly RelayValueConverter StringToInt32;

        private static object Convert<TFrom, TTo>(object value, bool isNullable, CultureInfo culture)
        {
            var fromType = typeof(TFrom);
            var targetType = typeof(TTo);

            if (value == null)
            {
                return isNullable ? null : targetType.GetSystemDefaultValue();
            }

            if (fromType == typeof(string))
            {
                var reflectParseMethod = targetType.GetMethods(BindingFlags.Static | BindingFlags.Public)
                                                   .FirstOrDefault(m => m.Name.AreEqual("TryParse") &&
                                                                        m.GetParameters().Length == 2);

                if (reflectParseMethod != null)
                {
                    var parameters = new[] { value, null };
                    var result = reflectParseMethod.Invoke(null, parameters);
                    if (result is bool boolResult && boolResult)
                    {
                        return parameters[1];
                    }
                }
            }

            if (targetType == typeof(string))
            {
                if (value is IFormattable formattable)
                {
                    return formattable.ToString(null, culture);
                }

                return value.ToString();
            }

            var typeConverter = TypeDescriptor.GetConverter(fromType);
            if (typeConverter.CanConvertTo(targetType))
            {
                return typeConverter.ConvertTo(value, targetType);
            }

            return null;
        }

        private static bool IsNullable(Type type)
        {
            if (type == null) return true;
            return !type.IsValueType || type.IsNullable();
        }

        static BoxConverters()
        {
            StringToInt32 = new RelayValueConverter(
                (value, targetType, parameter, culture) => Convert<String, Int32>(value, IsNullable(targetType), culture),
                (value, targetType, parameter, culture) => Convert<Int32, String>(value, IsNullable(targetType), culture));

            Int32ToString = new RelayValueConverter(
                (value, targetType, parameter, culture) => Convert<Int32, String>(value, IsNullable(targetType), culture),
                (value, targetType, parameter, culture) => Convert<String, Int32>(value, IsNullable(targetType), culture));

            StringToDecimal = new RelayValueConverter(
                (value, targetType, parameter, culture) => Convert<String, Decimal>(value, IsNullable(targetType), culture),
                (value, targetType, parameter, culture) => Convert<Decimal, String>(value, IsNullable(targetType), culture));

            DecimalToString = new RelayValueConverter(
                (value, targetType, parameter, culture) => Convert<Decimal, String>(value, IsNullable(targetType), culture),
                (value, targetType, parameter, culture) => Convert<String, Decimal>(value, IsNullable(targetType), culture));

            DoubleToGridLength = new RelayValueConverter(
                (value, targetType, parameter, culture) => value is double d ? new GridLength(d) : GridLength.Auto,
                (value, targetType, parameter, culture) => value is GridLength gridLength ? gridLength.Value : 0d);
        }
    }
}
