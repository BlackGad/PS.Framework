using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace PS.WPF.Theming.Markup
{
    public class Font : Binding
    {
        private ThemeFont _type;

        #region Constructors

        public Font(ThemeFont type)
        {
            Type = type;
            Mode = BindingMode.OneWay;
            Source = Theme.Current;
        }

        #endregion

        #region Properties

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

        #endregion

        #region Members

        private string GeneratePath(ThemeFont font)
        {
            var result = nameof(Theme.Fonts) + "." + font;
            return result;
        }

        #endregion
    }
}