using System;
using System.ComponentModel;
using System.Windows.Media;
using PS.WPF.Resources;
using PS.WPF.Theming;

namespace PS.WPF.ValueConverters
{
    public static class ImageConverters
    {
        public static readonly RelayValueConverter Generic;

        static ImageConverters()
        {
            Generic = new RelayValueConverter((value, targetType, parameter, culture) =>
            {
                if (value is ResourceDescriptor resource) value = resource.GetResource();

                if (value == null) return null;
                targetType = parameter as Type ?? targetType;

                var converter = TypeDescriptor.GetConverter(value.GetType());
                if (converter.CanConvertTo(targetType)) return converter.ConvertTo(value, targetType);
                if (typeof(ImageSource).IsAssignableFrom(targetType))
                {
                    if (value is Geometry geometry)
                    {
                        return new DrawingImage
                        {
                            Drawing = new GeometryDrawing
                            {
                                Brush = new SolidColorBrush(Theme.Current.Colors.MainForeground),
                                Geometry = geometry
                            }
                        };
                    }
                }

                return value;
            });
        }
    }
}
