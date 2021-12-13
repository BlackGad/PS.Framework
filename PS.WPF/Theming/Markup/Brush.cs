using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace PS.WPF.Theming.Markup
{
    public class Brush : Binding
    {
        private ThemeColorOpacity _opacity;
        private ThemeColor _type;

        #region Constructors

        public Brush(ThemeColor type)
        {
            Type = type;
            Mode = BindingMode.OneWay;
            Source = Theme.Current;
        }

        #endregion

        #region Properties

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

        #endregion

        #region Members

        private string GeneratePath(ThemeColor color, ThemeColorOpacity opacity)
        {
            var result = nameof(Theme.Brushes) + "." + color;
            if (opacity > ThemeColorOpacity.None)
            {
                result += Color.GetOpacityPostfix(opacity);
            }

            return result;
        }

        #endregion
    }
}