using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace PS.WPF.Theming.Markup
{
    public class FontSize : Binding
    {
        private ThemeFontSize _type;

        #region Constructors

        public FontSize(ThemeFontSize type)
        {
            Type = type;
            Mode = BindingMode.OneWay;
            Source = Theme.Current;
        }

        #endregion

        #region Properties

        [ConstructorArgument("type")]
        public ThemeFontSize Type
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

        private string GeneratePath(ThemeFontSize fontSize)
        {
            var result = nameof(Theme.FontSizes) + "." + fontSize;
            return result;
        }

        #endregion
    }
}