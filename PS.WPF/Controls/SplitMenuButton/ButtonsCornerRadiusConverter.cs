using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PS.WPF.Controls
{
    public class SplitMenuButtonCornerRadiusConverter : IValueConverter
    {
        #region Constants

        public const string Left = nameof(Left);
        public const string LeftWithMenu = nameof(LeftWithMenu);
        public const string Right = nameof(Right);
        public const string RightWithMenu = nameof(RightWithMenu);
        public const string SingleWithMenu = nameof(SingleWithMenu);

        #endregion

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CornerRadius cornerRadius)
            {
                switch ((string)parameter)
                {
                    case Left:
                        return new CornerRadius(cornerRadius.TopLeft, 0, 0, cornerRadius.BottomLeft);
                    case Right:
                        return new CornerRadius(0, cornerRadius.TopRight, cornerRadius.BottomRight, 0);
                    case LeftWithMenu:
                        return new CornerRadius(cornerRadius.TopLeft, 0, 0, 0);
                    case RightWithMenu:
                        return new CornerRadius(0, cornerRadius.TopRight, 0, 0);
                    case SingleWithMenu:
                        return new CornerRadius(cornerRadius.TopLeft, cornerRadius.TopRight, 0, 0);
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}