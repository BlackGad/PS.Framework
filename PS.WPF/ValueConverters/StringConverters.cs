using System;
using System.Linq;
using System.Windows;
using PS.Extensions;

namespace PS.WPF.ValueConverters
{
    public static class StringConverters
    {
        public static readonly RelayValueConverter DisplayName;
        public static readonly RelayValueConverter EmptyWatermark;
        public static readonly RelayMultiValueConverter FirstNotEmpty;
        public static readonly RelayValueConverter IsEmpty;
        public static readonly RelayValueConverter UnsetIfEmpty;

        static StringConverters()
        {
            EmptyWatermark = new RelayValueConverter((value, targetType, parameter, culture) =>
            {
                var watermark = parameter?.ToString() ?? "<Empty>";
                var stringValue = value?.ToString();

                return string.IsNullOrEmpty(stringValue) ? watermark : stringValue;
            });

            UnsetIfEmpty = new RelayValueConverter((value, targetType, parameter, culture) =>
            {
                var stringValue = value?.ToString();
                return string.IsNullOrEmpty(stringValue) ? DependencyProperty.UnsetValue : stringValue;
            });

            IsEmpty = new RelayValueConverter((value, targetType, parameter, culture) =>
            {
                var stringValue = value?.ToString();
                return string.IsNullOrEmpty(stringValue);
            });

            DisplayName = new RelayValueConverter((value, targetType, parameter, culture) =>
            {
                if (value == null) return string.Empty;
                if (value is Type valueAsType) return valueAsType.GetDisplayName();

                var valueType = value.GetType();
                if (valueType.IsEnum)
                {
                    var fieldInfo = valueType.GetField(value.ToString());
                    if (fieldInfo != null) return fieldInfo.GetDisplayName();
                }

                return value.ToString();
            });

            FirstNotEmpty = new RelayMultiValueConverter((objects, type, parameter, culture) =>
            {
                var result = objects.Enumerate().FirstOrDefault(o => !string.IsNullOrEmpty(o.GetEffectiveString()));
                return result;
            });
        }
    }
}
