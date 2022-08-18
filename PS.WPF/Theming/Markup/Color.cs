using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace PS.WPF.Theming.Markup
{
    public class Color : Binding
    {
        internal static string GetOpacityPostfix(ThemeColorOpacity opacity)
        {
            var type = typeof(ThemeColorOpacity);
            var fieldInfo = type.GetField(opacity.ToString());
            return fieldInfo?.GetCustomAttribute<PostfixAttribute>()?.Value;
        }

        private ThemeColorOpacity _opacity;
        private ThemeColor _type;

        public Color(ThemeColor type)
        {
            Type = type;
            Mode = BindingMode.OneWay;
            Source = Theme.Current;
        }

        [ConstructorArgument("opacity")]
        public ThemeColorOpacity Opacity
        {
            get { return _opacity; }
            set
            {
                _opacity = value;
                Path = new PropertyPath(GeneratePath(_type, _opacity));
            }
        }

        [ConstructorArgument("type")]
        public ThemeColor Type
        {
            get { return _type; }
            set
            {
                _type = value;
                Path = new PropertyPath(GeneratePath(_type, _opacity));
            }
        }

        private string GeneratePath(ThemeColor color, ThemeColorOpacity opacity)
        {
            var result = nameof(Theme.Colors) + "." + color;
            if (opacity > ThemeColorOpacity.None)
            {
                result += GetOpacityPostfix(opacity);
            }

            return result;
        }
    }
}
