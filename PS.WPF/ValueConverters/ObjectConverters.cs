using System;
using System.Globalization;
using System.Windows;
using PS.Extensions;

namespace PS.WPF.ValueConverters
{
    public static class ObjectConverters
    {
        public static readonly RelayValueConverter AssignableFrom;
        public static readonly RelayValueConverter Exist;
        public static readonly RelayValueConverter Format;
        public static readonly RelayValueConverter ParameterIfNull;
        public static readonly RelayValueConverter Type;
        public static readonly RelayValueConverter UnsetIfNull;

        private static object FormatValue(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return parameter.GetEffectiveString();

            var type = value.GetType().GetSourceType();
            if (type == typeof(TimeSpan))
            {
                var timespan = (TimeSpan)value;

                var result = timespan.ToString("hh\\:mm\\:ss");
                if (timespan.Days != 0) result = timespan.ToString("d\\:") + result;
                if (timespan.Milliseconds != 0) result = result + timespan.ToString("\\.fff");
                return result;
            }

            return value;
        }

        static ObjectConverters()
        {
            Type = new RelayValueConverter((value, targetType, parameter, culture) => value?.GetType(),
                                           (value, targetType, parameter, culture) =>
                                           {
                                               if (value is Type type) return Activator.CreateInstance(type);
                                               return null;
                                           });
            Exist = new RelayValueConverter((value, targetType, parameter, culture) => value != null);
            UnsetIfNull = new RelayValueConverter((value, targetType, parameter, culture) => value ?? DependencyProperty.UnsetValue);

            ParameterIfNull = new RelayValueConverter((value, targetType, parameter, culture) => value ?? parameter);

            AssignableFrom = new RelayValueConverter((value, targetType, parameter, culture) =>
            {
                var valueType = value?.GetType();
                var parameterType = parameter as Type;

                if (valueType == null || parameterType == null) return false;
                return parameterType.IsAssignableFrom(valueType);
            });

            Format = new RelayValueConverter(FormatValue);
        }
    }
}
