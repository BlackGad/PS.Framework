using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using PS.Extensions;

namespace PS.WPF.ValueConverters
{
    public static class NumericConverters
    {
        public static readonly RelayValueConverter Abs;
        public static readonly RelayValueConverter Add;
        public static readonly RelayValueConverter Divide;
        public static readonly RelayValueConverter Invert;

        public static readonly RelayValueConverter IsBiggerThan;
        public static readonly RelayValueConverter IsBiggerThanOrEqual;
        public static readonly RelayValueConverter IsEqual;
        public static readonly RelayValueConverter IsLessThan;
        public static readonly RelayValueConverter IsLessThanOrEqual;

        public static readonly RelayValueConverter Max;
        public static readonly RelayValueConverter Min;
        public static readonly RelayMultiValueConverter MultiAdd;
        public static readonly RelayMultiValueConverter MultiMax;
        public static readonly RelayMultiValueConverter MultiMin;
        public static readonly RelayValueConverter Multiply;
        public static readonly RelayMultiValueConverter MultiSubtract;
        public static readonly RelayValueConverter NumericToString;
        public static readonly RelayValueConverter StringToNumeric;
        public static readonly RelayValueConverter Subtract;
        public static readonly RelayValueConverter ToGridLength;

        private static object AbsFunc(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueType = value?.GetType();
            if (valueType == null) return null;

            return valueType.IsNumeric() ? Math.Abs((dynamic)value) : value;
        }

        private static object AddFunc(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueType = value?.GetType();
            if (valueType == null || !valueType.IsNumeric()) return value;

            var parameterType = parameter?.GetType();
            if (parameterType == null || !parameterType.IsNumeric()) return value;

            return (dynamic)value + (dynamic)parameter;
        }

        private static object DivideFunc(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueType = value?.GetType();
            if (valueType == null || !valueType.IsNumeric()) return value;

            var parameterType = parameter?.GetType();
            if (parameterType == null || !parameterType.IsNumeric()) return value;

            return (dynamic)value / (dynamic)parameter;
        }

        private static object InvertFunc(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueType = value?.GetType();
            if (valueType == null) return null;

            return valueType.IsNumeric() ? -1 * (dynamic)value : value;
        }

        private static object MultiplyFunc(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueType = value?.GetType();
            if (valueType == null || !valueType.IsNumeric()) return value;

            var parameterType = parameter?.GetType();
            if (parameterType == null || !parameterType.IsNumeric()) return value;

            return (dynamic)value * (dynamic)parameter;
        }

        private static object NumericToStringFunc(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.GetEffectiveString();
        }

        private static object StringToNumericFunc(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == null) throw new ArgumentNullException(nameof(targetType));

            var stringValue = value.GetEffectiveString();
            var sourceType = targetType.GetSourceType();

            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return targetType.IsNullable() ? null : sourceType.GetSystemDefaultValue();
            }

            var converter = TypeDescriptor.GetConverter(sourceType);

            return converter.ConvertFromString(stringValue);
        }

        private static object SubtractFunc(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueType = value?.GetType();
            if (valueType == null || !valueType.IsNumeric()) return value;

            var parameterType = parameter?.GetType();
            if (parameterType == null || !parameterType.IsNumeric()) return value;

            return (dynamic)value - (dynamic)parameter;
        }

        static NumericConverters()
        {
            Invert = new RelayValueConverter(InvertFunc, InvertFunc);
            Abs = new RelayValueConverter(AbsFunc, AbsFunc);

            Add = new RelayValueConverter(AddFunc, SubtractFunc);
            Subtract = new RelayValueConverter(SubtractFunc, AddFunc);
            Multiply = new RelayValueConverter(MultiplyFunc, DivideFunc);
            Divide = new RelayValueConverter(DivideFunc, MultiplyFunc);
            StringToNumeric = new RelayValueConverter(StringToNumericFunc, NumericToStringFunc);
            NumericToString = new RelayValueConverter(NumericToStringFunc, StringToNumericFunc);

            MultiAdd = new RelayMultiValueConverter((objects, type, parameter, culture) =>
            {
                var queue = new Queue<object>(objects.Enumerate().Select(obj =>
                {
                    if (obj == null) return 0;
                    return obj.GetType().IsNumeric() ? obj : 0;
                }));

                if (!queue.Any()) return null;
                if (!queue.TryDequeue(out var result)) return null;

                while (queue.TryDequeue(out var sub))
                {
                    result = (dynamic)result + (dynamic)sub;
                }

                return result;
            });

            MultiSubtract = new RelayMultiValueConverter((objects, type, parameter, culture) =>
            {
                var queue = new Queue<object>(objects.Enumerate().Select(obj =>
                {
                    if (obj == null) return 0;
                    return obj.GetType().IsNumeric() ? obj : 0;
                }));

                if (!queue.Any()) return null;
                if (!queue.TryDequeue(out var result)) return null;

                while (queue.TryDequeue(out var sub))
                {
                    result = (dynamic)result - (dynamic)sub;
                }

                if (parameter is bool isPositive && isPositive)
                {
                    return (dynamic)result > 0 ? result : type.GetSystemDefaultValue();
                }

                return result;
            });

            Min = new RelayValueConverter((value, type, parameter, culture) =>
            {
                var left = (dynamic)(value ?? 0);
                var right = (dynamic)(parameter ?? 0);
                return left < right ? left : right;
            });

            Max = new RelayValueConverter((value, type, parameter, culture) =>
            {
                var left = (dynamic)(value ?? 0);
                var right = (dynamic)(parameter ?? 0);
                return left > right ? left : right;
            });

            MultiMin = new RelayMultiValueConverter((objects, type, parameter, culture) =>
            {
                var itemsToCompare = objects.Where(item => item != null && item.GetType().IsNumeric())
                                            .ToList();
                object result = null;
                foreach (var item in itemsToCompare)
                {
                    if (result == null) result = item;
                    if ((dynamic)item < (dynamic)result) result = item;
                }

                return result.ConvertNumericValueTo(type);
            });

            MultiMax = new RelayMultiValueConverter((objects, type, parameter, culture) =>
            {
                var itemsToCompare = objects.Where(item => item != null && item.GetType().IsNumeric())
                                            .ToList();
                object result = null;
                foreach (var item in itemsToCompare)
                {
                    if (result == null) result = item;
                    if ((dynamic)item > (dynamic)result) result = item;
                }

                return result.ConvertNumericValueTo(type);
            });

            IsLessThan = new RelayValueConverter((value, type, parameter, culture) => (dynamic)(value ?? 0) < (dynamic)(parameter ?? 0));
            IsLessThanOrEqual = new RelayValueConverter((value, type, parameter, culture) => (dynamic)(value ?? 0) <= (dynamic)(parameter ?? 0));
            IsBiggerThan = new RelayValueConverter((value, type, parameter, culture) => (dynamic)(value ?? 0) > (dynamic)(parameter ?? 0));
            IsBiggerThanOrEqual = new RelayValueConverter((value, type, parameter, culture) => (dynamic)(value ?? 0) >= (dynamic)(parameter ?? 0));
            IsEqual = new RelayValueConverter((value, type, parameter, culture) => (dynamic)(value ?? 0) == (dynamic)(parameter ?? 0));

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
    }
}
