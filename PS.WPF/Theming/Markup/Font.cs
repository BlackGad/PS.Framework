using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace PS.WPF.Theming.Markup
{
    public class Font : Binding
    {
        private ThemeFont _type;

        public Font(ThemeFont type)
        {
            Type = type;
            Mode = BindingMode.OneWay;
            Source = Theme.Current;
        }

        [ConstructorArgument("type")]
        public ThemeFont Type
        {
            get { return _type; }
            set
            {
                _type = value;
                Path = new PropertyPath(GeneratePath(_type));
            }
        }

        private string GeneratePath(ThemeFont font)
        {
            var result = nameof(Theme.Fonts) + "." + font;
            return result;
        }
    }
}
