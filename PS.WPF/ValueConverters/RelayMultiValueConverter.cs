using System;
using System.Globalization;
using System.Windows.Data;

namespace PS.WPF.ValueConverters
{
    public class RelayMultiValueConverter : IMultiValueConverter
    {
        private Func<object, Type[], object, CultureInfo, object[]> _convertBackFunc;
        private Func<object[], Type, object, CultureInfo, object> _convertFunc;

        private IMultiValueConverter _instance;

        public RelayMultiValueConverter(Func<object[], Type, object, CultureInfo, object> convertFunc = null,
                                        Func<object, Type[], object, CultureInfo, object[]> convertBackFunc = null)
        {
            _convertFunc = convertFunc;
            _convertBackFunc = convertBackFunc;
        }

        public RelayMultiValueConverter()
        {
        }

        public IMultiValueConverter Instance
        {
            get { return _instance; }
            set
            {
                _instance = value;

                if (value != null)
                {
                    _convertFunc = value.Convert;
                    _convertBackFunc = value.ConvertBack;
                }
                else
                {
                    _convertFunc = null;
                    _convertBackFunc = null;
                }
            }
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (_convertFunc == null) throw new NotSupportedException();
            return _convertFunc(values, targetType, parameter, culture);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (_convertBackFunc == null) throw new NotSupportedException();
            return _convertBackFunc(value, targetTypes, parameter, culture);
        }
    }
}
