using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;
using PS.Extensions;
using PS.WPF.ValueConverters.SwitchValueConverter.Cases;

namespace PS.WPF.ValueConverters.SwitchValueConverter
{
    [ContentProperty(nameof(ConvertCases))]
    public class SwitchValueConverter : IValueConverter
    {
        #region Constructors

        public SwitchValueConverter()
        {
            ConvertBackCases = new ObservableCollection<SwitchCase>();
            ConvertCases = new ObservableCollection<SwitchCase>();
        }

        #endregion

        #region Properties

        public ObservableCollection<SwitchCase> ConvertBackCases { get; set; }
        public object ConvertBackDefault { get; set; }

        public ObservableCollection<SwitchCase> ConvertCases { get; set; }

        public object ConvertDefault { get; set; }

        #endregion

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConvertCases.Enumerate().Any()) throw new NotSupportedException();

            foreach (var @case in ConvertCases)
            {
                if (@case.IsValid(value)) return @case.Result;
            }

            return ConvertDefault;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConvertBackCases.Enumerate().Any()) throw new NotSupportedException();

            foreach (var @case in ConvertBackCases)
            {
                if (@case.IsValid(value)) return @case.Result;
            }

            return ConvertBackDefault;
        }

        #endregion
    }
}