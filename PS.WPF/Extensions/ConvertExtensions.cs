using System;
using System.Windows.Media;

namespace PS.WPF.Extensions
{
    public static class ConvertExtensions
    {
        public static Brush ToBrush(this object source)
        {
            if (source == null) return null;
            switch (source)
            {
                case Brush brush:
                    return brush;
                case Color color:
                    return new SolidColorBrush(color);
                case string brushString:
                    var converter = new BrushConverter();
                    var parsed = converter.ConvertFromString(brushString);
                    if (parsed is Color parsedColor)
                    {
                        return new SolidColorBrush(parsedColor);
                    }
                    else
                    {
                        throw new ArgumentException($"'{brushString}' could not be converted to brush");
                    }

                default:
                    throw new NotSupportedException($"{source.GetType()} not supported as source type");
            }
        }

        public static Color? ToColor(this object source)
        {
            if (source == null) return null;

            switch (source)
            {
                case Color color:
                    return color;
                case SolidColorBrush brush:
                    return brush.Color;
                case string colorString:
                    var parsed = ColorConverter.ConvertFromString(colorString);
                    if (parsed is Color parsedColor)
                    {
                        return parsedColor;
                    }
                    else
                    {
                        throw new ArgumentException($"'{colorString}' could not be converted to color");
                    }

                default:
                    throw new NotSupportedException($"{source.GetType()} not supported as source type");
            }
        }

        public static Geometry ToGeometry(this object source)
        {
            if (source == null) return null;

            switch (source)
            {
                case Geometry geometry:
                    return geometry;
                case string geometryString:
                    return Geometry.Parse(geometryString);
                default:
                    throw new NotSupportedException($"{source.GetType()} not supported as source type");
            }
        }
    }
}
