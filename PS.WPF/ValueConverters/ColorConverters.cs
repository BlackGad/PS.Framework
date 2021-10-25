using System;
using System.Windows.Media;
using PS.WPF.Extensions;

namespace PS.WPF.ValueConverters
{
    public static class ColorConverters
    {
        #region Constants

        public static readonly RelayValueConverter ContrastForeground;
        public static readonly RelayValueConverter Generic;
        public static readonly RelayValueConverter Invert;
        public static readonly RelayValueConverter Opacity;
        public static readonly RelayValueConverter Shade;
        public static readonly RelayValueConverter Tint;

        #endregion

        #region Static members

        public static Color ApplyInvert(Color color)
        {
            return Color.FromArgb(color.A,
                                  (byte)(255 - color.R),
                                  (byte)(255 - color.G),
                                  (byte)(255 - color.B));
        }

        public static Color ApplyOpacity(Color color, double coefficient)
        {
            return Color.FromArgb((byte)Math.Max(Math.Min(color.A * coefficient, 255), 0),
                                  color.R,
                                  color.G,
                                  color.B);
        }

        public static Color ApplyShade(Color color, double coefficient)
        {
            return Color.FromArgb(color.A,
                                  (byte)Math.Max(Math.Min(color.R * (1 - coefficient), 255), 0),
                                  (byte)Math.Max(Math.Min(color.G * (1 - coefficient), 255), 0),
                                  (byte)Math.Max(Math.Min(color.B * (1 - coefficient), 255), 0));
        }

        public static Color ApplyTint(Color color, double coefficient)
        {
            return Color.FromArgb(color.A,
                                  (byte)Math.Max(Math.Min(color.R + (255 - color.R) * coefficient, 255), 0),
                                  (byte)Math.Max(Math.Min(color.G + (255 - color.G) * coefficient, 255), 0),
                                  (byte)Math.Max(Math.Min(color.B + (255 - color.B) * coefficient, 255), 0));
        }

        /// <summary>
        ///     https://www.w3.org/TR/AERT/#color-contrast
        /// </summary>
        private static double CalculateBrightness(Color color)
        {
            return (color.R * 299 + color.G * 587 + color.B * 114) / 1000d / 255;
        }

        /// <summary>
        ///     https://www.w3.org/TR/WCAG20-TECHS/G17.html
        /// </summary>
        private static double CalculateLuminance(Color color)
        {
            var colorR = color.R / 255f;
            var colorG = color.G / 255f;
            var colorB = color.B / 255f;

            var r = colorR <= 0.03928 ? colorR / 12.92 : Math.Pow((colorR + 0.055) / 1.055, 2.4);
            var g = colorG <= 0.03928 ? colorG / 12.92 : Math.Pow((colorG + 0.055) / 1.055, 2.4);
            var b = colorB <= 0.03928 ? colorB / 12.92 : Math.Pow((colorB + 0.055) / 1.055, 2.4);
            return 0.2126 * r + 0.7152 * g + 0.0722 * b;
        }

        private static object PrepareTargetColor(Color value, Type targetType)
        {
            if (targetType == typeof(Color))
            {
                return value;
            }

            if (typeof(Brush).IsAssignableFrom(targetType))
            {
                return new SolidColorBrush(value);
            }

            if (targetType == typeof(string))
            {
                return value.ToString();
            }

            return value;
        }

        #endregion

        #region Constructors

        static ColorConverters()
        {
            const double defaultCoefficient = 0.2;

            Opacity = new RelayValueConverter((value, targetType, parameter, culture) =>
            {
                if (!(parameter is double coefficient)) coefficient = defaultCoefficient;
                coefficient = Math.Max(Math.Min(coefficient, 1), 0);

                var sourceColor = value.ToColor();
                if (sourceColor == null) return null;

                var targetColor = ApplyOpacity(sourceColor.Value, coefficient);

                return PrepareTargetColor(targetColor, targetType ?? value.GetType());
            });

            Generic = new RelayValueConverter((value, targetType, parameter, culture) =>
            {
                var sourceColor = value.ToColor();
                if (sourceColor == null) return null;
                targetType = parameter as Type ?? targetType ?? value.GetType();
                return PrepareTargetColor(sourceColor.Value, targetType);
            });

            Shade = new RelayValueConverter((value, targetType, parameter, culture) =>
            {
                if (!(parameter is double coefficient)) coefficient = defaultCoefficient;
                coefficient = Math.Max(Math.Min(coefficient, 1), 0);

                var sourceColor = value.ToColor();
                if (sourceColor == null) return null;

                var targetColor = ApplyShade(sourceColor.Value, coefficient);

                return PrepareTargetColor(targetColor, targetType ?? value.GetType());
            });

            ContrastForeground = new RelayValueConverter((value, targetType, parameter, culture) =>
            {
                var sourceColor = value.ToColor();
                if (sourceColor == null) return null;

                var targetColor = Colors.Black;

                var brightness = CalculateBrightness(sourceColor.Value);
                var luminance = CalculateLuminance(sourceColor.Value);

                if (brightness < 0.5 && luminance < 0.5) targetColor = Colors.White;
                if (brightness > 0.5 && luminance > 0.5) targetColor = Colors.Black;
                if (brightness > 0.5 && luminance < 0.5) targetColor = Colors.White;
                if (brightness < 0.5 && luminance > 0.5) targetColor = Colors.Black;

                return PrepareTargetColor(targetColor, targetType ?? value.GetType());
            });

            Tint = new RelayValueConverter((value, targetType, parameter, culture) =>
            {
                if (!(parameter is double coefficient)) coefficient = defaultCoefficient;
                coefficient = Math.Max(Math.Min(coefficient, 1), 0);

                var sourceColor = value.ToColor();
                if (sourceColor == null) return null;

                var targetColor = ApplyTint(sourceColor.Value, coefficient);

                return PrepareTargetColor(targetColor, targetType ?? value.GetType());
            });

            Invert = new RelayValueConverter((value, targetType, parameter, culture) =>
            {
                var sourceColor = value.ToColor();
                if (sourceColor == null) return null;

                var targetColor = ApplyInvert(sourceColor.Value);

                return PrepareTargetColor(targetColor, targetType ?? value.GetType());
            });
        }

        #endregion
    }
}