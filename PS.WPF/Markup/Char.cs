using System.Windows.Markup;

namespace PS.WPF.Markup
{
    [MarkupExtensionReturnType(typeof(char))]
    public class Char : BaseIntegerMarkupExtension<char>
    {
        #region Constructors

        public Char()
        {
        }

        public Char(char value)
            : base(value)
        {
        }

        #endregion
    }
}