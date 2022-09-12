using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using PS.Extensions;
using PS.WPF.ValueConverters.SwitchValueConverter.Cases;

namespace PS.WPF.ValueConverters.SwitchValueConverter
{
    [ContentProperty(nameof(ConvertCases))]
    public class SwitchValueConverter : Freezable,
                                        IValueConverter
    {
        public static readonly DependencyProperty ConvertBackCasesProperty =
            DependencyProperty.Register(nameof(ConvertBackCases),
                                        typeof(ObservableCollection<SwitchCase>),
                                        typeof(SwitchValueConverter),
                                        new FrameworkPropertyMetadata(default(ObservableCollection<SwitchCase>)));

        public static readonly DependencyProperty ConvertBackDefaultProperty =
            DependencyProperty.Register(nameof(ConvertBackDefault),
                                        typeof(object),
                                        typeof(SwitchValueConverter),
                                        new FrameworkPropertyMetadata(default(object)));

        public static readonly DependencyProperty ConvertCasesProperty =
            DependencyProperty.Register(nameof(ConvertCases),
                                        typeof(ObservableCollection<SwitchCase>),
                                        typeof(SwitchValueConverter),
                                        new FrameworkPropertyMetadata(default(ObservableCollection<SwitchCase>)));

        public static readonly DependencyProperty ConvertDefaultProperty =
            DependencyProperty.Register(nameof(ConvertDefault),
                                        typeof(object),
                                        typeof(SwitchValueConverter),
                                        new FrameworkPropertyMetadata(default(object)));

        public SwitchValueConverter()
        {
            ConvertBackCases = new ObservableCollection<SwitchCase>();
            ConvertCases = new ObservableCollection<SwitchCase>();
        }

        public ObservableCollection<SwitchCase> ConvertBackCases
        {
            get { return (ObservableCollection<SwitchCase>)GetValue(ConvertBackCasesProperty); }
            set { SetValue(ConvertBackCasesProperty, value); }
        }

        public object ConvertBackDefault
        {
            get { return GetValue(ConvertBackDefaultProperty); }
            set { SetValue(ConvertBackDefaultProperty, value); }
        }

        public ObservableCollection<SwitchCase> ConvertCases
        {
            get { return (ObservableCollection<SwitchCase>)GetValue(ConvertCasesProperty); }
            set { SetValue(ConvertCasesProperty, value); }
        }

        public object ConvertDefault
        {
            get { return GetValue(ConvertDefaultProperty); }
            set { SetValue(ConvertDefaultProperty, value); }
        }

        protected override Freezable CreateInstanceCore()
        {
            throw new NotSupportedException();
        }

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
    }
}
