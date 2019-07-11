using System;
using System.Globalization;
using System.Windows.Data;

namespace PS.WPF.ValueConverters
{
    public class RelayValueConverter : IValueConverter
    {
        private Func<object, Type, object, CultureInfo, object> _convertBackFunc;
        private Func<object, Type, object, CultureInfo, object> _convertFunc;
        private IValueConverter _instance;

        #region Constructors

        public RelayValueConverter(Func<object, Type, object, CultureInfo, object> convertFunc = null,
                                   Func<object, Type, object, CultureInfo, object> convertBackFunc = null)
        {
            _convertFunc = convertFunc;
            _convertBackFunc = convertBackFunc;
        }

        public RelayValueConverter()
        {
        }

        #endregion

        #region Properties

        public IValueConverter Instance
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

        #endregion

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (_convertFunc == null) throw new NotSupportedException();
            return _convertFunc(value, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (_convertBackFunc == null) throw new NotSupportedException();
            return _convertBackFunc(value, targetType, parameter, culture);
        }

        #endregion
    }
}